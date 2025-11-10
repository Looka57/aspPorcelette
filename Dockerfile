# Stage 1 : build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy project file(s) and restore
COPY ["ASPPorcelette.API/ASPPorcelette.API.csproj", "ASPPorcelette.API/"]
RUN dotnet restore "ASPPorcelette.API/ASPPorcelette.API.csproj"

# Copy everything and publish
COPY . .
WORKDIR /src/ASPPorcelette.API
RUN dotnet publish -c Release -o /app/publish --no-restore

# Stage 2 : runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
ENV ASPNETCORE_URLS=http://+:5000
EXPOSE 5000

COPY --from=build /app/publish .

# Simple healthcheck using curl; si curl introuvable dans l'image, on pourra l'enlever.
HEALTHCHECK --interval=30s --timeout=5s --start-period=10s \
    CMD curl -f http://localhost:5000/health || exit 1

ENTRYPOINT ["dotnet", "ASPPorcelette.API.dll"]
