# munity
Model United Nations REST API and Blazor Client

## About
MUNity is a work in progress to create and manage Model United Nations Conferences. It also contains a lot of software solutions for documents like resolutions, aswell as a solution to create speakerlists.

> Note that this Software is still under development.

[![Coverage Status](https://coveralls.io/repos/github/PeerConradi/munity/badge.svg?branch=master)](https://coveralls.io/github/PeerConradi/munity?branch=master)

## General structure

There are 6 different projects in this project solution. 
* The Backend, a WebAPI 2.0 (with a Test Project)
* The Frontend, a Blazor Web-Assembly Project (with a Test Project)
* The MunitySchema, a shared code containing structures and logic that is shared between the FrontEnd and Backend. You can also find this one on nuget. (also with a Test Project)

## Status
This application is under development and cannot be used at the moment. Please check later to see some progress.


| Name                | Status                              | Stage                                                            |
| ------------------- | ----------------------------------- | ---------------------------------------------------------------- |
| User Managment      | ![10%](https://progress-bar.dev/10) | Started                                                          |
| ConferenceManagment | ![21%](https://progress-bar.dev/21) | API Routes needed                                                |
| ResaEditor          | ![40%](https://progress-bar.dev/50) | Rework at offline Mode needed. |
| Speakerlist         | ![50%](https://progress-bar.dev/90) | __NEEDS Tests__                                     |
| Simulation          | ![5%](https://progress-bar.dev/20)   | Reworking the different functions.                              |
| Administration      | ![5%](https://progress-bar.dev/5)   | First API Endpoints                                              |
| Dockerize           | ![80%](https://progress-bar.dev/80) | Environment Configurations needed for DbConnection Strings       |

## Setup Development Environment
Requiered: 
* Visual Studio Community Edtion 2019 or higher or Visual Studio Code https://visualstudio.microsoft.com/de/downloads/
* MariaDb (MySql Version: 10.1.26) https://mariadb.org/download/
* MongoDb (latest) https://www.mongodb.com/try/download/community

Look into the appsettings.json to change Database Connection Strings!
After Starting the Databases for MySQL (MariaDb) and MongoDb will create itself.

For more information check out the [Getting started](Documentation/GettingStarted.md)

## Installing MUNity on Linux

A guide on how to install the application on a Linux System can 
be found [here](Documentation/installation.md)

A __NEW__ guide on how to run the MunityCore and Angular FrontEnd can be found: [here](Documentation/docker.md)

## Swagger

The project uses swagger. Go to localhost:[port]/swagger to see the auto generated API Documentation.

## Demo

Find a Demo here (MUN-TOOLS)[https://mun-tools.com/]

_The Live Version may not match the current GitHub Version_

## License Information

You are not allowed to sell this project as a SaaS or PaaS without permission. You are allowed use it for development under GPL-3 or install it on your own servers for your Model United Nations Conference.


