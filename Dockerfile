# Attention:
# The Dockerfile is not ready as of now.
#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.
 
FROM mcr.microsoft.com/dotnet/core/aspnet:5.0-alpine AS base
WORKDIR /app

# API
FROM mcr.microsoft.com/dotnet/core/sdk:5.0-alpine AS build
WORKDIR /src
COPY ["MUNityAngular/MUNityAngular.csproj", "MUNityAngular/"]
RUN dotnet restore "MUNityAngular/MUNityAngular.csproj"
COPY . .
RUN ls
WORKDIR "/src/MUNityAngular"
RUN dotnet build "MUNityAngular.csproj" -c Release -o /app/build


FROM build AS publish
RUN dotnet publish "MUNityAngular.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV ASPNETCORE_URLS http://*:5000
ENTRYPOINT ["dotnet", "MUNityAngular.dll"]
