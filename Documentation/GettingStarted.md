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


## Configure the Database

For this step you can use either SQLite or MariaDb/MySQL Databases.

Open up the src/MUNityCore/Startup.cs file

Activate either one of the functions:

```c#
SetupDatabaseWithMySql(services);
//SetupDatabaseWithSQLite(services);
```

SQLite will allow you to start the app, without having any other application installed. Note that SQLite is not
suiteable for a production system since it will not be farst enought to handle all incoming changes. For developing and testing its fine.

### Further configurate MySQL

To setup the MariaDB better, go into the appsettings.json file and set your mysql user and password:

```json
"MySqlSettings": {
    "ConnectionString": "server=localhost;userid=root;password='';database='munitybase'"
  },
```

## Start the app

You can use the Visual Studio Debugger to start the app. If you are using VSCode bring up the console and go into
src/MUNityCore and call: ````dotnet watch run``` this will start the app and always reloads it, when you make changes.

This will start the server at localhost:5001 and may already open a browser.

# Install

If this is the first time you setup the application, it will generate the database. Now you need to create an account.
Go to: ```https://localhost:5001/Identity/Account/Register``` and create an account.

This will bring you back to the startpage with you now signed in.

Go to ```https://localhost:5001/install``` to start the installation.