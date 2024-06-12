#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["nuget.config", "."]
COPY ["w1/w1.csproj", "w1/"]
COPY ["w1.Common/w1.Common.csproj", "w1.Common/"]
COPY ["w1.Service/w1.Service.csproj", "w1.Service/"]
COPY ["w1.Repository/w1.Repository.csproj", "w1.Repository/"]
RUN dotnet restore "./w1/w1.csproj"
COPY . .
WORKDIR "/src/w1"
RUN dotnet build "./w1.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./w1.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "w1.dll"]