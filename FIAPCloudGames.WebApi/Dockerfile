# Etapa 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copia os arquivos de projeto para restaurar dependências
COPY ["FIAPCloudGames.WebApi/FIAPCloudGames.WebApi.csproj", "FIAPCloudGames.WebApi/"]
COPY ["FIAPCloudGames.Domain/FIAPCloudGames.Domain.csproj", "FIAPCloudGames.Domain/"]
COPY ["FIAPCloudGames.Application/FIAPCloudGames.Application.csproj", "FIAPCloudGames.Application/"]
COPY ["FIAPCloudGames.Infrastructure/FIAPCloudGames.Infrastructure.csproj", "FIAPCloudGames.Infrastructure/"]

# Restaura os pacotes NuGet
RUN dotnet restore "FIAPCloudGames.WebApi/FIAPCloudGames.WebApi.csproj"

# Copia todo o restante do código
COPY . .

# Define o diretório de trabalho como o da Web API
WORKDIR "/src/FIAPCloudGames.WebApi"

# Compila o projeto
RUN dotnet publish "FIAPCloudGames.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Etapa 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# Copia os binários compilados da etapa anterior
COPY --from=build /app/publish .

# Expõe a porta padrão do Azure
EXPOSE 80

# Comando de inicialização da aplicação
ENTRYPOINT ["dotnet", "FIAPCloudGames.WebApi.dll"]
