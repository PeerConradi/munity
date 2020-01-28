# munity
MUN Crossplatform WebApplication

## About
MUNity is a .NET Core 3.0 Web-API with an Angular Front-End. It is designed to be used live on conferences and be hosted inside any
Windows or Linux environment. The Server can be hosted anywhere by anyone. The project owner will host one free to use instance of this
server on mun-tools.com.

## Status
This application is under development and cannot be used at the moment. Please check later to see some progress.

## To-Dos Before Open Source:
These Things have to be finished before this project could go OpenSource: 
* [ ] Generate/Update the Database itself
* [ ] API Services for the Speakerlists
* [ ] API Services for the Resolution Editor
* [X] Login and Registration
* [ ] User Management (Admin, User, Visitors)

## To-Dos Before Beta
* [X] Login&Register
* [X] Create Conference Area
* [ ] Edit Conference Area
* [ ] Conference Team Area
* [ ] Delegation Area
* [ ] Resolution Editor
* [ ] Speakerlist Editor
* [ ] Resolution and Speakerlist Views
* [ ] imprint
* [ ] privacy statement

## Components

### Public Views
Some Areas should be visible to everyone

### ID Views (ID-Access)
Some Areas like Speakerlists and Resolutions should be visible to everyone if the owner of the document/speakerlist 
allowed the ID-Access.

### Login Area
Some features only work when the user has an account.
The Basic account only requires an email adress, username and password, everything else should be optional.

## Features
* Speakerlists
* Resolution Editor
* Delegation Editor
* Conference Configuration
