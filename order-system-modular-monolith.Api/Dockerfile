# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["order-system-modular-monolith.Api/order-system-modular-monolith.Api.csproj", "order-system-modular-monolith.Api/"]
COPY ["order-system-modular-monolith.BuildingBlocks/order-system-modular-monolith.BuildingBlocks.csproj", "order-system-modular-monolith.BuildingBlocks/"]
COPY ["order-system-modular-monolith.Identity/order-system-modular-monolith.Identity.csproj", "order-system-modular-monolith.Identity/"]
COPY ["order-system-modular-monolith.Product/order-system-modular-monolith.Product.csproj", "order-system-modular-monolith.Product/"]
COPY ["order-system-modular-monolith.ServiceDefaults/order-system-modular-monolith.ServiceDefaults.csproj", "order-system-modular-monolith.ServiceDefaults/"]
RUN dotnet restore "./order-system-modular-monolith.Api/order-system-modular-monolith.Api.csproj"
COPY . .
WORKDIR "/src/order-system-modular-monolith.Api"
RUN dotnet build "./order-system-modular-monolith.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./order-system-modular-monolith.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "order-system-modular-monolith.Api.dll"]