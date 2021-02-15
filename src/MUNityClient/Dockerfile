FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build-env
WORKDIR /app
COPY . ./
RUN dotnet publish -c Release -o output
WORKDIR /var/www/web
COPY --from=build-env /app/output/wwwroot .
EXPOSE 80