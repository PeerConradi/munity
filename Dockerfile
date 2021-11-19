#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.
 
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app

# API
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /
COPY . .
RUN dotnet restore "src/MUNity.BlazorServer/MUNity.BlazorServer.csproj"
#RUN dotnet build "MUNityCore/MUNityCore.csproj" -c Release -o /app/build


FROM build AS publish
RUN dotnet publish "src/MUNity.BlazorServer/MUNity.BlazorServer.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
# !IMPORTANT: This is only for the test to take the demo Database. Remove
# this when going into production it will take a demo database that is
# filled with test users.
COPY src/MUNity.BlazorServer/demo.db .
# COPY src/MUNity.BlazorServer/demo.db-shm .
# COPY src/MUNity.BlazorServer/demo.db-wal .
ENV ASPNETCORE_URLS http://*:5000
EXPOSE 5000
ENTRYPOINT ["dotnet", "MUNity.BlazorServer.dll"]
