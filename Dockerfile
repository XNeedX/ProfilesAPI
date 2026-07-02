FROM mcr.microsoft.com/dotnet/aspnet:10.0-alpine AS base
WORKDIR /app
EXPOSE 8080
# Явно указываем встроенного пользователя без привилегий (безопасность)
USER app 

# 2. Этап сборки (здесь можно оставить стандартный SDK на Linux)
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release

ARG GITHUB_USERNAME
ARG GITHUB_PASSWORD
ENV GITHUB_USERNAME=$GITHUB_USERNAME
ENV GITHUB_PASSWORD=$GITHUB_PASSWORD

WORKDIR /src
# Кэширование зависимостей (оставляем как было)
COPY ["Profiles.API/Profiles.Presentation.csproj", "Profiles.API/"]
COPY ["Profiles.Application/Profiles.Application.csproj", "Profiles.Application/"]
COPY ["Profiles.Infrastructure/Profiles.Infrastructure.csproj", "Profiles.Infrastructure/"]
COPY ["Profiles.Domain/Profiles.Domain.csproj", "Profiles.Domain/"]
COPY ["nuget.config", "./"]
RUN dotnet restore "./Profiles.API/Profiles.Presentation.csproj"

# Сборка проекта
COPY . .
WORKDIR "/src/Profiles.API"
RUN dotnet build "./Profiles.Presentation.csproj" -c $BUILD_CONFIGURATION -o /app/build

# 3. Публикация
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Profiles.Presentation.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# 4. Финальный образ
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "Profiles.Presentation.dll"]