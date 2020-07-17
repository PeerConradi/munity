Linux Server aufsetzen
# Installation von .NET

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

# Installation von MariaDB

```
sudo apt update
sudo apt install mariadb-server
(Verify work: sudo systemctl status mariadb)

# Login
mysql -u root -p
```

# Install node.js

```
sudo apt-get install nodejs

```

# Install npm

```
curl -L https://npmjs.org/install.sh | sudo sh
```


# Install Angular-CLI

```
npm install -g @angular/cli  
```

# On every deploy: 

# Build the MUNity Project
```
cd MUNityAngular
dotnet publish -c release -r ubuntu.18.04-x64

#rename folder to munity
```

# Move project to Server

On Windows Command Prompt
```
#Bei Windows:
pscp -r I:\MUN-Tools\munity\MUNityAngular\bin\Release\netcoreapp3.0 root@h2868180.stratoserver.net:/opt/munity

#Im PuTTy
chmod 777 ./MUNityAngular
./appname
```

# Update Database
```
#Datenbank anlegen
mysql -u root -p $rootpassword

create database munity default character set utf8 default collate utf8_bin;

mysql -u root -p munity < db.sql

#Set the resolution Directory
mysql -u root -p
UPDATE munity.settings SET value='/home/munity' WHERE name='RESOLUTION_PATH';
```

# Start NetCore app
```
cd /opt/munity/munity/[version]/publish
./MUNityAngular

# Reload the Proxy
service nginx reload
