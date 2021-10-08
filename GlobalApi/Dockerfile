#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
ARG BuildConfiguration=Release
RUN --mount=type=secret,id=nuget,dst=/pat cp /pat ~/.nuget/NuGet/NuGet.Config
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

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Tgs.GlobalApi.dll"]