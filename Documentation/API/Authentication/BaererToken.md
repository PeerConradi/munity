# Baerer Token Authentication

## About

To validate and authenticate the user the API uses jwt Baerer Tokens. These tokens are send within the header of a request.
To make your request have the Authentication. You will get the token from the login request, with some more information.

Add the attribute "Authorization" with the value "Barer " + [The Token given by the login] in your HttpHeader.

The frontend in Angular uses the AuthInterceptor to do this. This way you don't need to set the HttpHeader yourself.

