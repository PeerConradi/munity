# Deployment and Installing

A guide on how to install the application on your server.

## Setup the environment

### Install .NET-Core Runtime

```
wget -q https://packages.microsoft.com/config/ubuntu/18.04/packages-microsoft-prod.deb -O packages-microsoft-prod.deb
sudo dpkg -i packages-microsoft-prod.deb

sudo add-apt-repository universe
sudo apt-get update
sudo apt-get install apt-transport-https
sudo apt-get update
sudo apt-get install dotnet-sdk-3.1

sudo apt-get update
sudo apt-get install aspnetcore-runtime-3.1

sudo apt-get update
sudo apt-get install dotnet-runtime-3.1
```

### Installation von MariaDB

```
sudo apt update
sudo apt install mariadb-server
(Verify work: sudo systemctl status mariadb)

# Login
mysql -u root -p
```

### Install node.js

```
sudo apt-get install nodejs
```

### Install npm

```
curl -L https://npmjs.org/install.sh | sudo sh
```


### Install Angular-CLI

```
npm install -g @angular/cli  
```


## Install MUNity 

### Clone the repository

This will checkout the latest version of the application. Since this
is a work in progress that is no stable version yet!

```
https://github.com/PeerConradi/munity.git
```

### Publish the application

This command will build the application

```
cd munity/MUNityAngular
dotnet publish -c release -r ubuntu.18.04-x64 --output /opt/munity
```

### Open the application to your users
```
chmod 777 ./MUNityAngular
```

### Update Database

On the first start of the application it should create the Databasestructure on its own.

### Start the app
```
cd /opt/munity/munity/[version]/publish
./MUNityAngular
```

## Setup Nginx

To use this application it can be necessary to setup the nginx.

Update your nginx configuration like followed.


```
server {
    listen        80;
    server_name   example.com *.example.com;
    location / {
        proxy_pass         http://localhost:5000;
        proxy_http_version 1.1;
        proxy_set_header   Upgrade $http_upgrade;
        proxy_set_header   Connection keep-alive;
        proxy_set_header   Host $host;
        proxy_cache_bypass $http_upgrade;
        proxy_set_header   X-Forwarded-For $proxy_add_x_forwarded_for;
        proxy_set_header   X-Forwarded-Proto $scheme;
    }

    location /resasocket {
        proxy_pass http://localhost:5000;
        proxy_http_version 1.1;
        proxy_set_header Upgrade $http_upgrade;
        proxy_set_header Connection "upgrade";
        proxy_set_header Host $host;
        proxy_cache_bypass $http_upgrade;
    }

    location /slsocket {
        proxy_pass http://localhost:5000;
        proxy_http_version 1.1;
        proxy_set_header Upgrade $http_upgrade;
        proxy_set_header Connection "upgrade";
        proxy_set_header Host $host;
        proxy_cache_bypass $http_upgrade;
    }

    location /simsocket {
        proxy_pass http://localhost:5000;
        proxy_http_version 1.1;
        proxy_set_header Upgrade $http_upgrade;
        proxy_set_header Connection "upgrade";
        proxy_set_header Host $host;
        proxy_cache_bypass $http_upgrade;
    }
}

```

### Reload the Proxy/nginx

```
service nginx reload
```
