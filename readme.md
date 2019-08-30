
## Hydra implementation proof of concept

This is a proof of concept to show a login flow and associated implementation utilizing [ORY Hydra](https://github.com/ory/hydra) as an Oauth2 server with bare bones IDP and resource servers written in Python using Django.

Not all Hydra requirements are implemented.  Currently not doing:
* Logout
* Denied requests
* Remembered / skipped auth requests

#### Dependencies

You'll need docker.  And docker compose.  Somewhat recent versions.

#### Setup

Just run:

`make setup`

`make run`

Then visit [http://localhost:7500/login](http://localhost:7500/login).

#### To come

Implementation of protected APIs using Oathkeeper and Keto
