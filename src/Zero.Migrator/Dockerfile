#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["src/Zero.Migrator/Zero.Migrator.csproj", "src/Zero.Migrator/"]
COPY ["src/Zero.EntityFrameworkCore/Zero.EntityFrameworkCore.csproj", "src/Zero.EntityFrameworkCore/"]
COPY ["src/Zero.Core/Zero.Core.csproj", "src/Zero.Core/"]
COPY ["src/Zero.Core.Shared/Zero.Core.Shared.csproj", "src/Zero.Core.Shared/"]
RUN dotnet restore "src/Zero.Migrator/Zero.Migrator.csproj"
COPY . .
WORKDIR "/src/src/Zero.Migrator"
RUN dotnet build "Zero.Migrator.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Zero.Migrator.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Zero.Migrator.dll"]
