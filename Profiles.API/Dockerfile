FROM mcr.microsoft.com/dotnet/aspnet:10.0-alpine AS base
WORKDIR /app
EXPOSE 8080

USER root

RUN apk add --no-cache icu-libs

ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false

USER app

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["Profiles.API/Profiles.Presentation.csproj", "Profiles.API/"]
COPY ["Profiles.Application/Profiles.Application.csproj", "Profiles.Application/"]
COPY ["Profiles.Infrastructure/Profiles.Infrastructure.csproj", "Profiles.Infrastructure/"]
COPY ["Profiles.Domain/Profiles.Domain.csproj", "Profiles.Domain/"]
COPY ["nuget.config", "./"]

RUN dotnet nuget add source "https://nuget.pkg.github.com/XNeedX/index.json" \
    --name "github" \
    --username "$GITHUB_USERNAME" \
    --password "$GITHUB_PASSWORD" \
    --store-password-in-clear-text \
    --configfile nuget.config
# ------------------

RUN dotnet restore "./Profiles.API/Profiles.Presentation.csproj"

COPY . .
WORKDIR "/src/Profiles.API"
RUN dotnet build "./Profiles.Presentation.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Profiles.Presentation.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "Profiles.Presentation.dll"]