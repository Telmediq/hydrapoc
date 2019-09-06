
## OAuth2 IdP implementation proof of concept

This is a proof of concept to show a login flow and associated implementation utilizing:
* [IdentityServer](https://identityserver.io/) .NET Core OIDC/OAuth2 IdP; or
* [ORY Hydra](https://github.com/ory/hydra) as an Oauth2 server with bare bones IDP and resource servers written in Python using Django.

The **IdentityServer** demo comes with the optional default UI, which is full featured, but would normally be customized/branded to the business that it is being used for. In a strict service-to-service setup without interactive logins, a login portal isn't necessarily needed at all.

Not all **Hydra** requirements are implemented.  Currently not doing:
* Logout
* Denied requests
* Remembered / skipped auth requests

### Dependencies

You'll need docker.  And docker compose.  Somewhat recent versions.

### Setup

Just run:

`make setup`

`make run`

### Django using IdentityServer or Hydra Demo
Visit [http://localhost:7500/login](http://localhost:7500/login).

Switch between IdentityServer (default) or Hydra by setting the USE_IDENTITY_SERVER environment variable on the resource service.

### Single Page JavaScript Application using IdentityServer Demo
Visit [http://localhost:5002/](http://localhost:5002/).

A service-to-service demo is also available at:

`IdSrvDemo/src/ConsoleClient/run.sh`

#### To come

Implementation of protected APIs using Oathkeeper and Keto
