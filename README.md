# munity
MUN Crossplatform WebApplication

## About
MUNity is a .NET Core 3.0 Web-API with an Angular Front-End. It is designed to be used live on conferences and be hosted inside any
Windows or Linux environment. The Server can be hosted anywhere by anyone. The project owner will host one free to use instance of this
server on mun-tools.com.

## Status
This application is under development and cannot be used at the moment. Please check later to see some progress.

You can open a preview at www.mun-tools.com.

## Setup Development Environment
Requiered: 
* Visual Studio Community Edtion 2019 or higher
* MariaDb (MySql Version: 10.1.26)
* NodeJS (12.16.xx) und AngularCLI (latest)
* MongoDb (latest) 

Look into the appsettings.json to change Database Connection Strings!
After Starting the Databases for MySQL (MariaDb) and MongoDb will create itself.

## Swagger
The project uses swagger. Go to localhost:"port"/swagger to see the auto generated API Documentation.

## Roadmap

### Done
* Registration
* Resolution Editor structure
* Speakerlist structure
* Conference Management
  * Create a conference
  * Manage Team (Roles and Members)
  * Manage Committees and Delegations

### Todo
* Notices/Comments on Resolutions and Preambles
* Custom Component for Notices with Tags
* View that displays speakerlist and Resolution at the same time
* Conference-rights management
