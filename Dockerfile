# Attention:
# The Dockerfile is not ready as of now.
#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.
 
FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /app

# API
FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src
COPY ["MUNityCore/MUNityCore.csproj", "MUNityCore/"]
RUN dotnet restore "MUNityCore/MUNityCore.csproj"
COPY . .
RUN dotnet build "MUNityCore.csproj" -c Release -o /app/build


FROM build AS publish
RUN dotnet publish "MUNityCore.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV ASPNETCORE_URLS http://*:5000
ENTRYPOINT ["dotnet", "MUNityCore.dll"]
