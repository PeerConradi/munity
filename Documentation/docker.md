# Prerequirements

## Install git

```
sudo apt install git-all
```

## Install Docker
```
sudo apt-get update

sudo apt-get install \
    apt-transport-https \
    ca-certificates \
    curl \
    gnupg-agent \
    software-properties-common
	
curl -fsSL https://download.docker.com/linux/ubuntu/gpg | sudo apt-key add -

sudo apt-key fingerprint 0EBFCD88	

sudo add-apt-repository \
   "deb [arch=amd64] https://download.docker.com/linux/ubuntu \
   $(lsb_release -cs) \
   stable"
   
sudo apt-get update

sudo apt-get install docker-ce docker-ce-cli containerd.io
```
## Install Pip3
```
sudo apt update

sudo apt install python3-pip
```

## Install docker-compose

```
pip3 install docker-compose
```

# Install MUNity

## Clone the repository
Clone Munity to your machine using git.

```
git clone https://github.com/PeerConradi/munity.git
```

## Setup the .env file

Create a .env file and add 

```
MUNITY_DB_PASS={YOUR DATABASE PASSWORD HERE}
```

## Create a directory

Create the directory: ```/var/volumes/munity``` using ```mkdir```

### Additional setup

MUNity loads additional MySQL information from the environment, you can specify the following:

| Parameter | Default |
|-----------|---------|
|MYSQL_SERVER|localhost|
|MYSQL_USER|root|
|MYSQL_PASS|[EMPTY STRING]|
|MYSQL_PORT|3306|
|MYSQL_DATABASE|munitybase|

> Note: When you use another mysql user than root make sure that this user is allowed to create the database: munitybase or this database already already exists and the user is allowed to create, drop and alter tables.

## Configure the appsettings.json
Setup appsettings.json this is used for a key that is important for authentication.
```
{
  "AppSettings": {
    "Secret": "REMEMBER TO CHANGE THIS KEY IN PRODUCTION EVERY TIME"
  }
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft": "Warning",
      "Microsoft.Hosting.Lifetime": "Information"
    }
  },
  "AllowedHosts": "*"
}
```

## Compose
Build the docker containers

```
docker-compose up -d --build
```

