# Services

## About

MUNity works with services to provide data. The Services are injected into the Controllers. There are a lot of different services to handle different types of tasks.

## Auth Service

The Auth Serves checks user authorization. It contains a lot of methods to check if a user is allowed to edit a specific conference or is allowed to perferm a certain task. Note that the AuthService inherits from IAuthService.

The IAuthService is Provided as __scoped__ inside the DependencyInjector, with the implementation of ```Services/AuthService```.