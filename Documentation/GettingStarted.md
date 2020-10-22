# Getting started

Welcome to the Munity project. To help you get started in this project
here is a guide on how to setup the development environment.

## Setup

You need to have the following software installed:

__Visual Studio 2019__

It up to you if you want to use the Community, Professional or Ultimate Version.
Make sure that when installing you have checked the ASP.NET Webdevelopment package and 
the Crossplatform .NET Development.

__.NET Core Runtime and SDK__
Should the Runtime and SDK not be installed automatically with Visual Studio
you can go here to Install the latest Version: https://dotnet.microsoft.com/download
The project is created with Version 3.1.

__MariaDB__

The Project uses a MariaDB. Until we are able to host this inside a Docker you
have to start a MariaDB Service. You can get it from here: https://mariadb.org/download/
or use a tool like [Xampp](https://www.apachefriends.org/de/index.html).

__MongoDB__

For documents this projects uses a MongoDB. You can get the community edition from here: 
https://docs.mongodb.com/manual/tutorial/install-mongodb-on-windows/.

## The Projects

After cloning the repository and opening the Solution File ```MunityAngular.sln``` you should find
two project inside the Project-Explorer: MUNityAngular and MUNityTest.

### MUNityAngular

MunityAngular is a frontend for the API. It can be found here: https://github.com/PeerConradi/MunityFrontend

### MUNityTest

This project contains the Tests for the MUNity Project. Note that only the Backend Tests are inside
this Project. Tests for the Angular (ClientApp) are inside ```MUNityAngular/ClientApp/```.