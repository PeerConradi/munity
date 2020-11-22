# munity
Model United Nations REST API

## About
MUNity is a work in progress to create and manage Model United Nations Conferences. It also contains a lot of software solutions for documents like resolutions, aswell as a solution to create speakerlists.

> Note that this Software is still under development.

[![Coverage Status](https://coveralls.io/repos/github/PeerConradi/munity/badge.svg?branch=master)](https://coveralls.io/github/PeerConradi/munity?branch=master)

## General structure

MUNity works with a REST Backend that is contained inside this project. There is also a MUNity Angular project for a frontend.

![](/Documentation/img/GeneralStructure.png)

## Status
This application is under development and cannot be used at the moment. Please check later to see some progress.


| Name                | Status                              | Stage                                                            |
| ------------------- | ----------------------------------- | ---------------------------------------------------------------- |
| User Managment      | ![10%](https://progress-bar.dev/10) | Started                                                          |
| ConferenceManagment | ![21%](https://progress-bar.dev/21) | API Routes needed                                                |
| ResaEditor          | ![40%](https://progress-bar.dev/40) | Move And Add Amendment missing. Performance improvments neeeded. |
| Speakerlist         | ![50%](https://progress-bar.dev/50) | Complete Rework in progress                                      |
| Simulation          | ![5%](https://progress-bar.dev/5)   | __ON HOLD__                                                      |
| Administration      | ![5%](https://progress-bar.dev/5)   | First API Endpoints                                              |
| Dockerize           | ![80%](https://progress-bar.dev/80) | Environment Configurations needed for DbConnection Strings       |

## Setup Development Environment
Requiered: 
* Visual Studio Community Edtion 2019 or higher
* MariaDb (MySql Version: 10.1.26)
* MongoDb (latest) 

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

__You can Take a look at a Demo [here](https://mun-tools.com).__

__Api References: https://mun-tools.com/swagger/index.html__

_The Live Version may not match the current GitHub Version_

## License Information

You are not allowed to sell this project as a SaaS or PaaS without permission. You can use it for development or any of your conferences.


