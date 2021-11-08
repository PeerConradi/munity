# munity
A Model United Nations online software that offers functionality to organize MUNs and tools to use on the conference like a list of speakers, resolution editor.

## About
MUNity is a work in progress to create and manage Model United Nations Conferences. It also contains a lot of software solutions for documents like resolutions, aswell as a solution to create speakerlists.

> Note that this Software is still under development.

## Coverage

Last checked: 16.08.2021

| Project | Description | Status | Test-Coverage |
|---------|----------|------------------|-----|
| MUNityBase | Contains base enums and interfaces for the Database and Schema | Mostly done | 100% | 
| MUNityDatabase | Contains the Database Model and Context | monstly done writing docs and tests | 37,77 % |
| MUNitySchema | Contains the Data-Transfer-Objects and ViewModels | TODO | TODO |
| MUNityServices | Contains logic to access the Database and generate Data-Transfer-Objects | TODO | TODO |
| MUNityBlazorServer | A blazor Server UI for MUNity using the MUNityServices |TODO | TODO |
| MUNityWebAPI  | A Web API for MUNity using the MUNityServices | TODO | TODO |


## Documentation

For the developer documentation see: [MUNity Documentation](Documentation/docs.md)

## Setup Development Environment

Check out the [Getting started](Documentation/GettingStarted.md) to learn how you can start developing munity.

## Installing MUNity on Linux

If you want to you can also install munity using docker. Find a tutorial for that [here](Documentation/docker.md)

## License Information

You are not allowed to sell this project as a SaaS or PaaS without permission. You are allowed use it for development under GPL-3 or install it on your own servers for your Model United Nations Conference.


