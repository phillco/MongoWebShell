MongoWebShell
=============

This project lets you **run a full-featured mongo shell from within the browser.**

Why do this? In a browser, we can make the existing shell far more powerful, by adding UI elements like drop-down autocompletion and integrated documentation that simply aren't possible in a Terminal-wrapped shell.

**How it works:** The server project (in the "Server" folder), starts an embedded HTTP server on localhost:29017 that serves the JavaScript-based front-end client (in the "Client" folder). The server handles wrapping the mongo.exe shell and provides RESTful endpoints to the client to get extra data.

Credits
=========

Developed/maintained by Phillip Cohen; owned by 10gen, Inc.