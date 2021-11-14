#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.
 
FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /app

# API
FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /
RUN dotnet restore "src/MUNity.BlazorServer/MUNity.BlazorServer.csproj"
COPY . .
#RUN dotnet build "MUNityCore/MUNityCore.csproj" -c Release -o /app/build


FROM build AS publish
RUN dotnet publish "src/MUNity.BlazorServer/MUNity.BlazorServer.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV ASPNETCORE_URLS http://*:5000
EXPOSE 5000
ENTRYPOINT ["dotnet", "MUNity.BlazorServer.dll"]
