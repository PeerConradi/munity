# Attention:
# The Dockerfile is not ready as of now.
# Needed to be done
# Build the project for linux
# Build the Angular frontend
# Install and Run MongoDb
# Install and Run MariaDb

#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
RUN apt-get update -yq \
    && apt-get install curl gnupg -yq \
    && curl -sL https://deb.nodesource.com/setup_10.x | bash \
    && apt-get install nodejs -yq
WORKDIR /app
EXPOSE 80
EXPOSE 443

EXPOSE 4200 49153

## MariaDB 10.3 with SSH
## Pull the mariadb latest image
#FROM mariadb:latest
## List all the packages that we want to install
#ENV PACKAGES openssh-server openssh-client
## Install Packages
#RUN apt-get update && apt-get install -y $PACKAGES

## MongoDB
#FROM debian:jessie-slim
#RUN apt-get update && \
#apt-get install -y ca-certificates && \
#rm -rf /var/lib/apt/lists/*
#RUN gpg --keyserver ha.pool.sks-keyservers.net --recv-keys 0C49F3730359A14518585931BC711F9BA15703C6 && \
#gpg --export $GPG_KEYS > /etc/apt/trusted.gpg.d/mongodb.gpg
#ARG MONGO_PACKAGE=mongodb-org
#ARG MONGO_REPO=repo.mongodb.org
#ENV MONGO_PACKAGE=${MONGO_PACKAGE} MONGO_REPO=${MONGO_REPO}
#ENV MONGO_MAJOR 3.4
#ENV MONGO_VERSION 3.4.18
#RUN echo "deb http://$MONGO_REPO/apt/debian jessie/${MONGO_PACKAGE%-unstable}/$MONGO_MAJOR main" | tee "/etc/apt/sources.list.d/${MONGO_PACKAGE%-unstable}.list"
#RUN echo "/etc/apt/sources.list.d/${MONGO_PACKAGE%-unstable}.list"
#RUN apt-get update
#RUN apt-get install -y ${MONGO_PACKAGE}=$MONGO_VERSION
#VOLUME ["/data/db"]
#WORKDIR /data
#EXPOSE 27017
#CMD ["mongod", "--smallfiles"]

# API
FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["MUNityAngular/MUNityAngular.csproj", "MUNityAngular/"]
RUN dotnet restore "MUNityAngular/MUNityAngular.csproj"
COPY . .
WORKDIR "/src/MUNityAngular"
RUN dotnet build "MUNityAngular.csproj" -c Release -r ubuntu.18.04-x64 -o /app/build


FROM build AS publish
RUN dotnet publish "MUNityAngular.csproj" -c Release -r ubuntu.18.04-x64 -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MUNityAngular"]

