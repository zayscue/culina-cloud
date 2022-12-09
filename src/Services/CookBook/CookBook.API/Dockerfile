#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
ARG dll_version=0.1.0.0
WORKDIR /src
COPY ["src/Services/CookBook/CookBook.API/CookBook.API.csproj", "src/Services/CookBook/CookBook.API/"]
RUN dotnet restore "src/Services/CookBook/CookBook.API/CookBook.API.csproj"
COPY . .
WORKDIR "/src/src/Services/CookBook/CookBook.API"
RUN dotnet build "CookBook.API.csproj" -c Release -o /app/build -p:Version=$dll_version

FROM build AS publish
RUN dotnet publish "CookBook.API.csproj" -c Release -o /app/publish -p:Version=$dll_version

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CulinaCloud.CookBook.API.dll"]
