# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["DK.Ordering.API/DK.Ordering.API.csproj", "DK.Ordering.API/"]
COPY ["DK.Common.Logging/DK.Common.Logging.csproj", "DK.Common.Logging/"]
COPY ["DK.EventBus.Messages/DK.EventBus.Messages.csproj", "DK.EventBus.Messages/"]
COPY ["DK.Ordering.Application/DK.Ordering.Application.csproj", "DK.Ordering.Application/"]
COPY ["DK.Ordering.Core/DK.Ordering.Core.csproj", "DK.Ordering.Core/"]
COPY ["DK.Ordering.Infrastructure/DK.Ordering.Infrastructure.csproj", "DK.Ordering.Infrastructure/"]
RUN dotnet restore "./DK.Ordering.API/DK.Ordering.API.csproj"
COPY . .
WORKDIR "/src/DK.Ordering.API"
RUN dotnet build "./DK.Ordering.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./DK.Ordering.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DK.Ordering.API.dll"]