# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:10.0-preview AS build
WORKDIR /src

# Copier le fichier .csproj et restaurer les dépendances
COPY ["ASPPorcelette.API.csproj", "./"]
RUN dotnet restore "ASPPorcelette.API.csproj"

# Copier tout le reste et builder
COPY . .
RUN dotnet build "ASPPorcelette.API.csproj" -c Release -o /app/build

# Stage 2: Publish
FROM build AS publish
RUN dotnet publish "ASPPorcelette.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Stage 3: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:10.0-preview AS final
WORKDIR /app
EXPOSE 8080
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ASPPorcelette.API.dll"]