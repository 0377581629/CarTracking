#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["src/Zero.Web.Public/Zero.Web.Public.csproj", "src/Zero.Web.Public/"]
COPY ["src/Zero.Web.Core/Zero.Web.Core.csproj", "src/Zero.Web.Core/"]
COPY ["src/Zero.Application/Zero.Application.csproj", "src/Zero.Application/"]
COPY ["src/Zero.Application.Shared/Zero.Application.Shared.csproj", "src/Zero.Application.Shared/"]
COPY ["src/Zero.Core.Shared/Zero.Core.Shared.csproj", "src/Zero.Core.Shared/"]
COPY ["src/Zero.Core/Zero.Core.csproj", "src/Zero.Core/"]
COPY ["src/Zero.EntityFrameworkCore/Zero.EntityFrameworkCore.csproj", "src/Zero.EntityFrameworkCore/"]
COPY ["src/Zero.GraphQL/Zero.GraphQL.csproj", "src/Zero.GraphQL/"]
RUN dotnet restore "src/Zero.Web.Public/Zero.Web.Public.csproj"
COPY . .
WORKDIR "/src/src/Zero.Web.Public"
RUN dotnet build "Zero.Web.Public.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Zero.Web.Public.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Zero.Web.Public.dll"]
