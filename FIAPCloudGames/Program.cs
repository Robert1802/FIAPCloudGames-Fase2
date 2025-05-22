using Core.Repository;
using Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;
using FIAPCloudGamesApi.Configurations;
using FIAPCloudGamesApi.Services;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Context;
using Serilog.Sinks.MSSqlServer;
using Serilog.Events;
using System.Collections.ObjectModel;
using System.Data;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.ApplicationInsights;
using Core.Utils;
using Core.Entity;

var builder = WebApplication.CreateBuilder(args);

var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .Build();

// Add services to the container
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "FIAPCloudGamesApi", Version = "v1" });

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

var connection = configuration.GetConnectionString("ConnectionString");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(connection);
    options.UseLazyLoadingProxies();
}, ServiceLifetime.Scoped);

builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<IJogoRepository, JogoRepository>();
builder.Services.AddScoped<IUsuarioJogoRepository, UsuarioJogoRepository>();

builder.Services.Configure<JwtSettings>(
    builder.Configuration.GetSection("Jwt")
);

// Auth
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

// Configure Serilog
var columnOptions = new ColumnOptions
{
    Store = new Collection<StandardColumn>
    {
        StandardColumn.Id,
        StandardColumn.Message,
        StandardColumn.MessageTemplate,
        StandardColumn.Level,
        StandardColumn.TimeStamp,
        StandardColumn.Exception,
        StandardColumn.Properties
    }
};


builder.Host.UseSerilog((context, services, loggerConfig) =>
{
    loggerConfig
        .WriteTo.MSSqlServer(
            connectionString: context.Configuration.GetConnectionString("ConnectionString"),
            sinkOptions: new MSSqlServerSinkOptions
            {
                TableName = "Logs",
                AutoCreateSqlTable = false
            },
            columnOptions: columnOptions
        );
});

var app = builder.Build();

//Verifica, sincroniza as migrations e adiciona o usuário admin caso não exista
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    dbContext.Database.Migrate();

    var config = scope.ServiceProvider.GetRequiredService<IConfiguration>();

    var adminEmail = config["SeedAdmin:Email"];
    var adminSenha = config["SeedAdmin:Senha"];
    var adminNome = config["SeedAdmin:Nome"];

    if (!dbContext.Usuario.Any(u => u.Email == adminEmail))
    {
        var admin = new Usuario
        {
            Nome = adminNome,
            Email = adminEmail,
            Senha = PasswordHelper.HashSenha(adminSenha!),
            NivelAcesso = "Admin",
            Saldo = 0
        };

        dbContext.Usuario.Add(admin);
        dbContext.SaveChanges();
    }    
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
