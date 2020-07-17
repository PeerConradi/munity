# munity
MUN Crossplatform WebApplication

## About
MUNity is a work in progress to create and manage Model United Nations Conferences.
This plattform will also offer a lot of tools that you can use to host such conferences.

## Status
This application is under development and cannot be used at the moment. Please check later to see some progress.

| Name | Status | Stage |
|------|--------|-------|
| User Managment | ![10%](https://progress-bar.dev/10) | Started |
| ConferenceManagment | ![20%](https://progress-bar.dev/20) | API Routes needed |
| ResaEditor | ![15%](https://progress-bar.dev/15) | Complete Rework needed |
| Speakerlist | ![20%](https://progress-bar.dev/20) | Complete Rework in progress |
| Simulation | ![5%](https://progress-bar.dev/5) | __ON HOLD__ |
| Administration | ![0%](https://progress-bar.dev/0) | __Not stareted__ |
| Dockerize | ![0%](https://progress-bar.dev/0) | __Not stareted__ |

## Setup Development Environment
Requiered: 
* Visual Studio Community Edtion 2019 or higher
* MariaDb (MySql Version: 10.1.26)
* NodeJS (12.16.xx) and AngularCLI (latest)
* MongoDb (latest) 

Look into the appsettings.json to change Database Connection Strings!
After Starting the Databases for MySQL (MariaDb) and MongoDb will create itself.

For more information check out the [Getting started](Documentation/GettingStarted.md)

## Installing MUNity on Linux

A guide on how to install the application on a Linux System can 
be found [here](Documentation/installation.md)

## Swagger

The project uses swagger. Go to localhost:"port"/swagger to see the auto generated API Documentation.


