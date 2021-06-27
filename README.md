# munity
A Model United Nations online software that offers functionality to organize MUNs and tools to use on the conference like a list of speakers, resolution editor.

## About
MUNity is a work in progress to create and manage Model United Nations Conferences. It also contains a lot of software solutions for documents like resolutions, aswell as a solution to create speakerlists.

> Note that this Software is still under development.

## General structure

There are 4 different projects in this project solution. 
* The Backend, a WebAPI 2.0 (with a Test Project)
* The MunitySchema, a shared code containing structures and logic that is shared between the FrontEnd and Backend. You can also find this one on nuget. (also with a Test Project)

## Status
This application is under development and cannot be used at the moment. Please check later to see some progress.


| Name                | Status                              | Stage                                                            |
| ------------------- | ----------------------------------- | ---------------------------------------------------------------- |
| User Managment      | ![10%](https://progress-bar.dev/10) | Started                                                          |
| ConferenceManagment | ![21%](https://progress-bar.dev/21) | API Routes needed                                                |
| ResaEditor          | ![70%](https://progress-bar.dev/70) | Can't create sub operative paragraphs. |
| Speakerlist         | ![100%](https://progress-bar.dev/100) | Done                                 |
| Simulation          | ![90%](https://progress-bar.dev/90)   | Simulation internal done needs some more REST API Endpoints.                           |
| Administration      | ![20%](https://progress-bar.dev/20)   | Base Administration done.                                            |
| Dockerize           | ![80%](https://progress-bar.dev/80) | Environment Configurations needed for DbConnection Strings       |

## Documentation

For the developer documentation see: [MUNIty Documentation](Documentation/docs.md)

## Setup Development Environment

Check out the [Getting started](Documentation/GettingStarted.md) to learn how you can start developing munity.

## Installing MUNity on Linux

A guide on how to install munity an an Ubuntu system without using docker can be found here: [here](Documentation/installation.md)

If you want to you can also install munity using docker. Find a tutorial for that [here](Documentation/docker.md)

## Swagger

The project uses swagger. Go to localhost:[port]/swagger to see the auto generated API Documentation.

## Demo

Find a Demo here (MUN-TOOLS)[https://mun-tools.com/]

_The Live Version may not match the current GitHub Version_

## License Information

You are not allowed to sell this project as a SaaS or PaaS without permission. You are allowed use it for development under GPL-3 or install it on your own servers for your Model United Nations Conference.


