# Etapa 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copia os arquivos .csproj para restaurar dependências
COPY FIAPCloudGames.WebApi/FIAPCloudGames.WebApi.csproj ./FIAPCloudGames.WebApi/
COPY FIAPCloudGames.Application/FIAPCloudGames.Application.csproj ./FIAPCloudGames.Application/
COPY FIAPCloudGames.Domain/FIAPCloudGames.Domain.csproj ./FIAPCloudGames.Domain/
COPY FIAPCloudGames.Infrastructure/FIAPCloudGames.Infrastructure.csproj ./FIAPCloudGames.Infrastructure/

# Restaura os pacotes
RUN dotnet restore ./FIAPCloudGames.WebApi/FIAPCloudGames.WebApi.csproj

# Copia o restante dos arquivos
COPY . .

# Define o diretório de trabalho da aplicação
WORKDIR /src/FIAPCloudGames.WebApi

# Publica a aplicação
RUN dotnet publish FIAPCloudGames.WebApi.csproj -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Etapa 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 80
ENTRYPOINT ["dotnet", "FIAPCloudGames.WebApi.dll"]
