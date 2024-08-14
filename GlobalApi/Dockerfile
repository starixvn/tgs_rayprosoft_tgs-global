#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.


FROM hub.lumigame.com/base/dotnet/core/sdk:8.0 AS build
ARG BuildConfiguration=Release
RUN dotnet nuget remove source 'Microsoft Visual Studio Offline Packages' || true
WORKDIR /src
COPY ["NuGet.config", "."]
COPY ["GlobalApi/GlobalApi.csproj", "GlobalApi/"]
COPY ["Global.Core/Global.Core.csproj", "Global.Core/"]
COPY ["Global.Repositories/Global.Repositories.csproj", "Global.Repositories/"]
RUN dotnet restore "GlobalApi/GlobalApi.csproj"
COPY . .
WORKDIR "/src/GlobalApi"
RUN dotnet build "GlobalApi.csproj" -c $BuildConfiguration -o /app/build

FROM build AS publish
RUN dotnet publish "GlobalApi.csproj" -c $BuildConfiguration -o /app/publish

FROM hub.lumigame.com/base/dotnet/core/aspnet:8.0-alpine-24h1.1 AS base
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Tgs.GlobalApi.dll"]