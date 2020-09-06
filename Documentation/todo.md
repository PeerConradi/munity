# Todos

## Backend



## Frontend

### Front-End Rework

#### New Structure

The front-end can be made a lot simpler by creating more components but make a simpler path for injection.

At the moment a lot of components use the Router Module to work with links etc. this can be made a lot simpler by
creating a better structure.

In future implementations there should be the following types:

* Dialog/Form - Component
* Display - Components
* Page
* Modules

The __Page__ is like the layout only allows you to implement the other components, but shouldn't use one of the services. The page is
allowed to be routed to and use an ActiveRoute etc. It can inject the active route into the components it uses, for example the id into
a conference. Every Page has a route unlike the components.

__Modules__ should follow this guide: https://malcoded.com/posts/angular-fundamentals-modules/