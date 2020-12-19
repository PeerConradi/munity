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

# Building MUNityCore + MunityFrontend (Angular)

## Step One
Clone the backend

```
git clone https://github.com/PeerConradi/munity.git
```

## Stop Two // TODO
Clone the Frontend

```

```

## Step Three
Setup appsettings.json
```
{
  "MySqlSettings": {
    "ConnectionString": "server=localhost;userid=root;password='';database='munitybase'"
  },
  "AppSettings": {
    "Secret": "REMEMBER TO CHANGE THIS KEY IN PRODUCTION EVERY TIME"
  },
  "MunityMongoDatabaseSettings": {
    "ResolutionCollectionName": "Resolutions",
    "PresenceCollectionName": "Presence",
    "ConnectionString": "mongodb://localhost:27017",
    "DatabaseName": "MunityDb"
  },
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

## Step Four
Create the Backend Docker image
```
cd munity
docker build -t munity-backend .
```

## Step Five
```
Create the Frontend Image
Go back to home
cd ~
cd MunityFrontend
docker build -t munity-frontend .
```
