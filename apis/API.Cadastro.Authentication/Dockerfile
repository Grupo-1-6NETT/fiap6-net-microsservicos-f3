# Base
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base

WORKDIR /app
EXPOSE 8082
EXPOSE 8083

# Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copia os arquivos de projeto
COPY ["Cadastro.Auth.API/Cadastro.Auth.API.csproj", "Cadastro.Auth.API/"]
COPY ["Application/Cadastro.Auth.Application.csproj", "Application/"]
COPY ["Cadastro.Auth.Domain/Cadastro.Auth.Domain.csproj", "Cadastro.Auth.Domain/"]
COPY ["Cadastro.Auth.Infra/Cadastro.Auth.Infra.csproj", "Cadastro.Auth.Infra/"]

# Restaura as dependências
RUN dotnet restore "Cadastro.Auth.API/Cadastro.Auth.API.csproj"

# Copia o restante dos arquivos
COPY . .

# Define o diretório de trabalho para o projeto principal
WORKDIR "/src/Cadastro.Auth.API"

# Publica o projeto
RUN dotnet publish -c Release -o /app/publish

# Runtime
FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Cadastro.Auth.API.dll"]