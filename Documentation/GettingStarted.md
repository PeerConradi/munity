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

## Developing and Debugging

You should be able to start the project.

> You need to have the MariaDB and MongoDB up and running in the background.

On the first start the application will create the Databases MUNityCore and MUNityBase. 

The start will open a browser window with a not reachable page. This is correct since there is 
no frontend inside this project. Note that it will also not run with  port 80. You can still reach the
swagger API documentation by going to /swagger/index.html