#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
ARG dll_version=0.1.0.0
WORKDIR /src
COPY ["src/Services/Interactions/Interactions.API/Interactions.API.csproj", "src/Services/Interactions/Interactions.API/"]
COPY ["src/BuildingBlocks/PostMaster/PostMaster.BackgroundService/PostMaster.BackgroundService.csproj", "src/BuildingBlocks/PostMaster/PostMaster.BackgroundService/"]
COPY ["src/BuildingBlocks/PostMaster/PostMaster/PostMaster.csproj", "src/BuildingBlocks/PostMaster/PostMaster/"]
COPY ["src/BuildingBlocks/EventStore/EventStore.Abstractions/EventStore.Abstractions.csproj", "src/BuildingBlocks/EventStore/EventStore.Abstractions/"]
COPY ["src/BuildingBlocks/Common/Common/Common.csproj", "src/BuildingBlocks/Common/Common/"]
COPY ["src/BuildingBlocks/PostMaster/PostMaster.Persistence/PostMaster.Persistence.csproj", "src/BuildingBlocks/PostMaster/PostMaster.Persistence/"]
COPY ["src/BuildingBlocks/PostMaster/PostMaster.Abstractions/PostMaster.Abstractions.csproj", "src/BuildingBlocks/PostMaster/PostMaster.Abstractions/"]
COPY ["src/Services/Interactions/Interactions.Application/Interactions.Application.csproj", "src/Services/Interactions/Interactions.Application/"]
COPY ["src/Services/Interactions/Interactions.Domain/Interactions.Domain.csproj", "src/Services/Interactions/Interactions.Domain/"]
COPY ["src/BuildingBlocks/Application/Application.Common/Application.Common.csproj", "src/BuildingBlocks/Application/Application.Common/"]
COPY ["src/BuildingBlocks/CurrentUser/CurrentUser.Abstractions/CurrentUser.Abstractions.csproj", "src/BuildingBlocks/CurrentUser/CurrentUser.Abstractions/"]
COPY ["src/BuildingBlocks/CurrentUser/CurrentUser/CurrentUser.csproj", "src/BuildingBlocks/CurrentUser/CurrentUser/"]
COPY ["src/BuildingBlocks/WebHostCustomization/WebHost.Customization/WebHost.Customization.csproj", "src/BuildingBlocks/WebHostCustomization/WebHost.Customization/"]
COPY ["src/Services/Interactions/Interactions.Infrastructure/Interactions.Infrastructure.csproj", "src/Services/Interactions/Interactions.Infrastructure/"]
COPY ["src/BuildingBlocks/Authentication/Authentication/Authentication.csproj", "src/BuildingBlocks/Authentication/Authentication/"]
COPY ["src/BuildingBlocks/Authentication/Authentication.Abstractions/Authentication.Abstractions.csproj", "src/BuildingBlocks/Authentication/Authentication.Abstractions/"]
COPY ["src/BuildingBlocks/EventStore/EventStore/EventStore.csproj", "src/BuildingBlocks/EventStore/EventStore/"]
COPY ["src/BuildingBlocks/API/API/API.csproj", "src/BuildingBlocks/API/API/"]
RUN dotnet restore "src/Services/Interactions/Interactions.API/Interactions.API.csproj"
COPY . .
WORKDIR "/src/src/Services/Interactions/Interactions.API"
RUN dotnet build "Interactions.API.csproj" -c Release -o /app/build -p:Version=$dll_version

FROM build AS publish
RUN dotnet publish "Interactions.API.csproj" -c Release -o /app/publish -p:Version=$dll_version

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CulinaCloud.Interactions.API.dll"]