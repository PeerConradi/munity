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

__Node JS__

For the frontend you will need to have NodeJS installed. The Projects uses the Version
12.16.xx you can download NodeJs at: https://nodejs.org/en/

__Angular CLI__

For the frontend application you need to have the angular CLI installed.
You can get it from here: https://cli.angular.io/

## The Projects

After cloning the repository and opening the Solution File ```MunityAngular.sln``` you should find
two project inside the Project-Explorer: MUNityAngular and MUNityTest.

### MUNityAngular

This project contains the frontend and the backend application of this project.
It should be selected as the default start project and the name should appear bold.

The folder ```ClientApp``` contains the code for the Angular9 frontend. Everything around it is the 
backend code.

### MUNityTest

This project contains the Tests for the MUNity Project. Note that only the Backend Tests are inside
this Project. Tests for the Angular (ClientApp) are inside ```MUNityAngular/ClientApp/```.