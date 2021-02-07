# Getting started

Welcome to the Munity project. To help you get started in this project
here is a guide on how to setup the development environment.

## Setup

You need to have the following software installed:

__Visual Studio 2019__

It up to you if you want to use the Community, Professional or Enterprise Version.
Make sure that when installing you have checked the ASP.NET Webdevelopment package and 
the Crossplatform .NET Development.

__.NET Core Runtime and SDK__
Should the Runtime and SDK not be installed automatically with Visual Studio
you can go here to Install the latest Version: https://dotnet.microsoft.com/download
The project uses .NET 5

__MariaDB__

The Project uses a MariaDB. You can get it from here: https://mariadb.org/download/
or use a tool like [Xampp](https://www.apachefriends.org/de/index.html).

__MongoDB__

For documents this projects uses a MongoDB. You can get the community edition from here: 
https://docs.mongodb.com/manual/tutorial/install-mongodb-on-windows/.

## The Projects

After cloning the repository and opening the Solution File ```MunityCore.sln```.

### MUNityClient

This is a Blazor Web-Assembly Client. MUNity switched late 2020 from Angular to Blazor. The Blazor project is organized into a the main Folders:
* Pages - contains all Components that have a route to them
* Services - Injectable Services that communicate with the API and Storage
* Shared - contains the different sub components
* ViewModel - contains ViewModels that store data, and listens to websockets or the storage.

### MUNityCore
Is the WebAPI 2.0 project that access the MariaDB and MongoDB and will serve the Data over REST.

### MUNitySchema
The Shared code between the MUNityCore and MUNityClient. The Schema can also be found as a nuget package: https://www.nuget.org/packages/MUNityBase/
This package is maintained by Peer Conradi.

__Every project also has an Unit Test Project.__

## Developing and Debugging

You should be able to start the project.

> You need to have the MariaDB and MongoDB installed and running in the background.

On the first start the application will create the Databases MUNityCore and MUNityBase. 

The start will open a browser window with a not reachable page. This is correct since there is 
no frontend inside this project. Note that it will also not run with  port 80. You can still reach the
swagger API documentation by going to /swagger/index.html