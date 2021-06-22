﻿FROM mcr.microsoft.com/dotnet/core/runtime:3.1 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /src
COPY ["telegram-spamer.csproj", "./"]
RUN dotnet restore "telegram-spamer.csproj"
COPY . .
WORKDIR "/src/"
RUN dotnet build "telegram-spamer.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "telegram-spamer.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "telegram-spamer.dll"]
