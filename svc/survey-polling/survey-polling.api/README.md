# Morum OSS - Live Survey Polling API

The API for this project that is built using ASP.NET Core 3.0, SignalR and Auth0.

## Features

1.	**Asynchronous, Concurrent Functionality**
    1.	This is a live polling system, we should avoid any blocking behaviors in this application.
    2.	We also need to support web sockets, essentially after a TCP connection is made, the visual interface continuously receives messages instead of polling the API for changes.
2.	**Authentication (You can add this functionality later, if you wish)**
    1.	We are using Auth0 for authentication and as a result, you can assume this API will receive a JWT token in the initial request header, which the API will then use to validate the user.
3.	**One Vote per User**
    1.	There should be a limit to how many messages can be sent per second.