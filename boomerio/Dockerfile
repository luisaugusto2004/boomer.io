FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["boomerio.csproj", "."]
RUN dotnet restore "./boomerio.csproj"
COPY . .
RUN dotnet build "./boomerio.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./boomerio.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app

RUN mkdir /data

COPY boomerio.db /data/boomerio.db

COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "boomerio.dll"]