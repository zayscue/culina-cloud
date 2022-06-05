#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
ARG dll_version=0.1.0.0
WORKDIR /src
COPY ["src/Services/Users/Users.API/Users.API.csproj", "src/Services/Users/Users.API/"]
COPY ["src/BuildingBlocks/Authorization/Authorization.csproj", "src/BuildingBlocks/Authorization/"]
COPY ["src/Services/Users/Users.Infrastructure/Users.Infrastructure.csproj", "src/Services/Users/Users.Infrastructure/"]
COPY ["src/BuildingBlocks/Authentication/Authentication/Authentication.csproj", "src/BuildingBlocks/Authentication/Authentication/"]
COPY ["src/BuildingBlocks/Authentication/Authentication.Abstractions/Authentication.Abstractions.csproj", "src/BuildingBlocks/Authentication/Authentication.Abstractions/"]
COPY ["src/BuildingBlocks/Common/Common/Common.csproj", "src/BuildingBlocks/Common/Common/"]
COPY ["src/Services/Users/Users.Application/Users.Application.csproj", "src/Services/Users/Users.Application/"]
COPY ["src/Services/Users/Users.Domain/Users.Domain.csproj", "src/Services/Users/Users.Domain/"]
COPY ["src/BuildingBlocks/Application/Application.Common/Application.Common.csproj", "src/BuildingBlocks/Application/Application.Common/"]
COPY ["src/BuildingBlocks/CurrentUser/CurrentUser.Abstractions/CurrentUser.Abstractions.csproj", "src/BuildingBlocks/CurrentUser/CurrentUser.Abstractions/"]
COPY ["src/BuildingBlocks/CurrentUser/CurrentUser/CurrentUser.csproj", "src/BuildingBlocks/CurrentUser/CurrentUser/"]
COPY ["src/BuildingBlocks/WebHostCustomization/WebHost.Customization/WebHost.Customization.csproj", "src/BuildingBlocks/WebHostCustomization/WebHost.Customization/"]
COPY ["src/BuildingBlocks/API/API/API.csproj", "src/BuildingBlocks/API/API/"]
RUN dotnet restore "src/Services/Users/Users.API/Users.API.csproj"
COPY . .
WORKDIR "/src/src/Services/Users/Users.API"
RUN dotnet build "Users.API.csproj" -c Release -o /app/build -p:Version=$dll_version

FROM build AS publish
RUN dotnet publish "Users.API.csproj" -c Release -o /app/publish -p:Version=$dll_version

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CulinaCloud.Users.API.dll"]