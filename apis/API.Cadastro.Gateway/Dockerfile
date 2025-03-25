# Etapa 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copia os arquivos do projeto e restaura dependências
COPY src/Cadastro.Gateway.API/Cadastro.Gateway.API.csproj ./Cadastro.Gateway.API/
RUN dotnet restore Cadastro.Gateway.API/Cadastro.Gateway.API.csproj

# Copia o código restante e compila a aplicação
COPY src/Cadastro.Gateway.API/. ./Cadastro.Gateway.API/
WORKDIR /app/Cadastro.Gateway.API
RUN dotnet publish -c Release -o /out

# Etapa 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /out ./

# Expõe a porta que o serviço utiliza
EXPOSE 8080
ENTRYPOINT ["dotnet", "Cadastro.Gateway.API.dll"]
