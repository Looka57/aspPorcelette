# Stage 1 : Build
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copier le projet et restaurer
COPY ["ASPPorcelette.API.csproj", "./"]
RUN dotnet restore "./ASPPorcelette.API.csproj"

# Copier tout et publier
COPY . .
RUN dotnet publish -c Release -o /app/publish --no-restore

# Stage 2 : Runtime
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:5000
EXPOSE 5000

ENTRYPOINT ["dotnet", "ASPPorcelette.API.dll"]
