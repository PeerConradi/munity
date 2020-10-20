# Attention:
# The Dockerfile is not ready as of now.
#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.
 
# API
FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["MUNityAngular.csproj", "MUNityAngular/"]
RUN dotnet restore "MUNityAngular/MUNityAngular.csproj"
COPY . .
WORKDIR "/src/MUNityAngular"
RUN dotnet build "MUNityAngular.csproj" -c Release -r ubuntu.18.04-x64 -o /app/build


FROM build AS publish
RUN dotnet publish "MUNityAngular.csproj" -c Release -r ubuntu.18.04-x64 -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV ASPNETCORE_URLS http://*:5000
ENTRYPOINT ["dotnet", "MUNityAngular"]
