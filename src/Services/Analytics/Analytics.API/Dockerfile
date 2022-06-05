#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
ARG dll_version=0.1.0.0
WORKDIR /src
COPY ["src/Services/Analytics/Analytics.API/Analytics.API.csproj", "src/Services/Analytics/Analytics.API/"]
COPY ["src/BuildingBlocks/Authorization/Authorization.csproj", "src/BuildingBlocks/Authorization/"]
COPY ["src/Services/Analytics/Analytics.Application/Analytics.Application.csproj", "src/Services/Analytics/Analytics.Application/"]
COPY ["src/BuildingBlocks/PostMaster/PostMaster.Persistence/PostMaster.Persistence.csproj", "src/BuildingBlocks/PostMaster/PostMaster.Persistence/"]
COPY ["src/BuildingBlocks/PostMaster/PostMaster.Abstractions/PostMaster.Abstractions.csproj", "src/BuildingBlocks/PostMaster/PostMaster.Abstractions/"]
COPY ["src/BuildingBlocks/Common/Common/Common.csproj", "src/BuildingBlocks/Common/Common/"]
COPY ["src/Services/Analytics/Analytics.Domain/Analytics.Domain.csproj", "src/Services/Analytics/Analytics.Domain/"]
COPY ["src/BuildingBlocks/Application/Application.Common/Application.Common.csproj", "src/BuildingBlocks/Application/Application.Common/"]
COPY ["src/BuildingBlocks/CurrentUser/CurrentUser.Abstractions/CurrentUser.Abstractions.csproj", "src/BuildingBlocks/CurrentUser/CurrentUser.Abstractions/"]
COPY ["src/Services/Analytics/Analytics.Infrastructure/Analytics.Infrastructure.csproj", "src/Services/Analytics/Analytics.Infrastructure/"]
COPY ["src/BuildingBlocks/CurrentUser/CurrentUser/CurrentUser.csproj", "src/BuildingBlocks/CurrentUser/CurrentUser/"]
COPY ["src/BuildingBlocks/WebHostCustomization/WebHost.Customization/WebHost.Customization.csproj", "src/BuildingBlocks/WebHostCustomization/WebHost.Customization/"]
COPY ["src/BuildingBlocks/API/API/API.csproj", "src/BuildingBlocks/API/API/"]
RUN dotnet restore "src/Services/Analytics/Analytics.API/Analytics.API.csproj"
COPY . .
WORKDIR "/src/src/Services/Analytics/Analytics.API"
RUN dotnet build "Analytics.API.csproj" -c Release -o /app/build -p:Version=$dll_version

FROM build AS publish
RUN dotnet publish "Analytics.API.csproj" -c Release -o /app/publish -p:Version=$dll_version

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CulinaCloud.Analytics.API.dll"]
