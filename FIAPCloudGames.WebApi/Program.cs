using DotNetEnv;
using FIAPCloudGames.Application.Services;
using FIAPCloudGames.Application.Utils;
using FIAPCloudGames.Application.Validators;
using FIAPCloudGames.Domain.Entity;
using FIAPCloudGames.Domain.Repository;
using FIAPCloudGames.Infrastructure.Middleware;
using FIAPCloudGames.Infrastructure.Repository;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Sinks.ApplicationInsights.TelemetryConverters;
using Serilog.Sinks.MSSqlServer;
using System.Collections.ObjectModel;
using System.Text;

// Carrega variáveis do .env
Env.Load();

var builder = WebApplication.CreateBuilder(args);


// Configuração: carrega appsettings.json + variáveis de ambiente
builder.Configuration
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddEnvironmentVariables();

var connection = builder.Configuration.GetConnectionString("DefaultConnection");

// Configure Serilog
var columnOptions = new ColumnOptions
{
    Store = new Collection<StandardColumn>
    {
        StandardColumn.Id,
        StandardColumn.Message,
        StandardColumn.Level,
        StandardColumn.TimeStamp,
        StandardColumn.Properties
    }
};

builder.Host.UseSerilog((context, services, loggerConfig) =>
{
    loggerConfig
        .WriteTo.MSSqlServer(
            connectionString: connection,
            sinkOptions: new MSSqlServerSinkOptions
            {
                TableName = "Logs",
                AutoCreateSqlTable = false
            },
            columnOptions: columnOptions
        )
        .WriteTo.ApplicationInsights(
            services.GetRequiredService<TelemetryConfiguration>(),
            new TraceTelemetryConverter()
        )
        .MinimumLevel.Information()
        .MinimumLevel.Override("Microsoft", Serilog.Events.LogEventLevel.Warning)
        .MinimumLevel.Override("Microsoft.AspNetCore", Serilog.Events.LogEventLevel.Warning)
        .Enrich.FromLogContext();
});

// Add services
builder.Services.AddControllers();
builder.Services.AddApplicationInsightsTelemetry();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = $"FIAP Cloud Games Api - {builder.Configuration["SwaggerEnvironment"]}", Version = "v1" });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "**IMPORTANTE:** Cole apenas o token JWT. O prefixo 'Bearer' será adicionado automaticamente."
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "Bearer",
                Name = "Authorization",
                In = ParameterLocation.Header
            },
            new List<string>()
        }
    });
});

// DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(connection);
    options.UseLazyLoadingProxies();
}, ServiceLifetime.Scoped);

// Repositórios
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<IJogoRepository, JogoRepository>();
builder.Services.AddScoped<IUsuarioJogoRepository, UsuarioJogoRepository>();
builder.Services.AddScoped<IPromocaoRepository, PromocaoRepository>();
builder.Services.AddScoped<IJogosPromocoesRepository, JogosPromocoesRepository>();

// Validação
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<JogosPromocoesRequestValidator>();

// JWT
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("Jwt"));

var jwtSettings = builder.Configuration.GetSection("Jwt").Get<JwtSettings>();
builder.Services.AddSingleton<TokenService>();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings!.Issuer,
            ValidAudience = jwtSettings!.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings!.ChaveSecreta))
        };
    });

builder.Services.AddAuthorization();
builder.Services.AddSingleton<TelemetryClient>();

var app = builder.Build();

// Força binding para que o Docker acesse via EXPOSE 80
app.Urls.Add("http://0.0.0.0:80");

// Verifica, sincroniza as migrations e adiciona o usuário admin caso não exista
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    dbContext.Database.Migrate();

    var config = scope.ServiceProvider.GetRequiredService<IConfiguration>();

    var adminEmail = config["SeedAdmin:Email"];
    var adminSenha = Encoding.UTF8.GetString(Convert.FromBase64String(config["SeedAdmin:Senha"]!));
    var adminNome = config["SeedAdmin:Nome"];

    if (!dbContext.Usuario.Any(u => u.Email == adminEmail))
    {
        var admin = new Usuario
        {
            Nome = adminNome!,
            Email = adminEmail!,
            Senha = PasswordHelper.HashSenha(adminSenha!),
            NivelAcesso = "Admin",
            Saldo = 0
        };

        dbContext.Usuario.Add(admin);
        dbContext.SaveChanges();
    }
}

// Middleware e pipeline
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "FIAPCloudGames API V1");
    c.RoutePrefix = "swagger";
});

app.UseHttpsRedirection();
app.UseMiddleware<ExceptionMiddleware>();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
