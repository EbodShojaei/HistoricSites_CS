FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

ENV ASPNETCORE_URLS=http://+:8080

USER app
FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG configuration=Release
WORKDIR /src
COPY ["ExoticHistoricSites.csproj", "./"]
RUN dotnet restore "ExoticHistoricSites.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "ExoticHistoricSites.csproj" -c $configuration -o /app/build

FROM build AS publish
ARG configuration=Release
RUN dotnet publish "ExoticHistoricSites.csproj" -c $configuration -o /app/publish /p:UseAppHost=false

# Final stage with runtime configuration
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Use non-root user for security
USER app

ENTRYPOINT ["dotnet", "ExoticHistoricSites.dll"]
