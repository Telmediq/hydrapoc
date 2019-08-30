import os

import django

os.environ.setdefault('OAUTHLIB_INSECURE_TRANSPORT', 'True')

from oauthlib.oauth2 import BackendApplicationClient
from requests_oauthlib import OAuth2Session

CLIENT_ID = 'test-client'
CLIENT_SECRET = 'abcdefSuperSecret123'
TOKEN_URL = 'http://localhost:4444/oauth2/token'


def run():

    client = BackendApplicationClient(CLIENT_ID)
    oauth = OAuth2Session(client=client)
    oauth.fetch_token(
        audience=CLIENT_ID,
        token_url=TOKEN_URL,
        client_secret=CLIENT_SECRET)

    response = oauth.get('http://localhost:8000/protected/')
    response.raise_for_status()

    print(response.json())


if __name__ == '__main__':
    run()
