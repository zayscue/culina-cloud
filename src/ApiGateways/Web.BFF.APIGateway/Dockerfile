#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/ApiGateways/Web.BFF.APIGateway/Web.BFF.APIGateway.csproj", "src/ApiGateways/Web.BFF.APIGateway/"]
COPY ["src/BuildingBlocks/CurrentUser/CurrentUser/CurrentUser.csproj", "src/BuildingBlocks/CurrentUser/CurrentUser/"]
COPY ["src/BuildingBlocks/CurrentUser/CurrentUser.Abstractions/CurrentUser.Abstractions.csproj", "src/BuildingBlocks/CurrentUser/CurrentUser.Abstractions/"]
COPY ["src/BuildingBlocks/Authentication/Authentication/Authentication.csproj", "src/BuildingBlocks/Authentication/Authentication/"]
COPY ["src/BuildingBlocks/Authentication/Authentication.Abstractions/Authentication.Abstractions.csproj", "src/BuildingBlocks/Authentication/Authentication.Abstractions/"]
COPY ["src/BuildingBlocks/Common/Common/Common.csproj", "src/BuildingBlocks/Common/Common/"]
COPY ["src/BuildingBlocks/Application/Application.Common/Application.Common.csproj", "src/BuildingBlocks/Application/Application.Common/"]
COPY ["src/BuildingBlocks/Authorization/Authorization.csproj", "src/BuildingBlocks/Authorization/"]
RUN dotnet restore "src/ApiGateways/Web.BFF.APIGateway/Web.BFF.APIGateway.csproj"
COPY . .
WORKDIR "/src/src/ApiGateways/Web.BFF.APIGateway"
RUN dotnet build "Web.BFF.APIGateway.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Web.BFF.APIGateway.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CulinaCloud.Web.BFF.APIGateway.dll"]