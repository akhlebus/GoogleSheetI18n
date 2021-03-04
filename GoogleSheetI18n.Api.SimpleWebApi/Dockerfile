#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src
RUN apt-get update && \
    apt-get install -y wget && \
    apt-get install -y gnupg2 && \
    wget -qO- https://deb.nodesource.com/setup_15.x | bash - && \
    apt-get install -y build-essential nodejs
COPY ["GoogleSheetI18n.Api.SimpleWebApi/GoogleSheetI18n.Api.SimpleWebApi.csproj", "GoogleSheetI18n.Api.SimpleWebApi/"]
COPY ["GoogleSheetI18n/GoogleSheetI18n.Api.csproj", "GoogleSheetI18n/"]
RUN dotnet restore "GoogleSheetI18n.Api.SimpleWebApi/GoogleSheetI18n.Api.SimpleWebApi.csproj"
COPY . .
WORKDIR "/src/GoogleSheetI18n.Api.SimpleWebApi"
RUN dotnet build "GoogleSheetI18n.Api.SimpleWebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "GoogleSheetI18n.Api.SimpleWebApi.csproj" -c Release -o /app/publish
COPY ["GoogleSheetI18n.Api.SimpleWebApi/ClientApp", "/app/publish/ClientApp/"]
WORKDIR "/app/publish/ClientApp/"
RUN npm ci
RUN npm run build


FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish ./
ENTRYPOINT ["dotnet", "GoogleSheetI18n.Api.SimpleWebApi.dll"]
