# Base
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8084
EXPOSE 8085

# Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copia os arquivos de projeto
COPY ["WeatherForecast/WeatherForecast.csproj", "WeatherForecast/"]

# Restaura as dependências
RUN dotnet restore "WeatherForecast/WeatherForecast.csproj"

# Copia o restante dos arquivos
COPY . .

# Define o diretório de trabalho para o projeto principal
WORKDIR "/src/WeatherForecast"

# Publica o projeto
RUN dotnet publish -c Release -o /app/publish

# Runtime
FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "WeatherForecast.dll"]
