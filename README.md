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
To start developing MUNity you need to clone the repository and have a mariaDb or mySql up and running.
The basic Database structure can be found under ```MUNityTest\NeededFiles\munity.sql``` note that this is also the Test Database and which already contains someData (Countries and Delegations). Feel free to delete them.

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
