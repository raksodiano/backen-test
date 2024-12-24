# Usar la imagen base de .NET 8
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

# Usar la imagen de SDK de .NET para compilar la aplicación
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiar el archivo csproj y restaurar dependencias
COPY ../TodoApplicationApi/TodoApplicationApi.csproj TodoApplicationApi/
RUN dotnet restore "TodoApplicationApi/TodoApplicationApi.csproj"

# Copiar todo el proyecto y construir
COPY ../TodoApplicationApi/. ./TodoApplicationApi/
WORKDIR "/src/TodoApplicationApi"
RUN dotnet build "TodoApplicationApi.csproj" -c Release -o /app/build

# Publicar la aplicación
FROM build AS publish
RUN dotnet publish "TodoApplicationApi.csproj" -c Release -o /app/publish

# Final: usar la imagen base de ASP.NET
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TodoApplicationApi.dll"]
