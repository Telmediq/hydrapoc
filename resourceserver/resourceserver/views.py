import json
import secrets
import uuid
import jwt
import requests

from django.conf import settings
from django.core.cache import cache
from django.http import JsonResponse, HttpResponseRedirect
from django.shortcuts import render_to_response
from jwt.algorithms import RSAAlgorithm
from requests_oauthlib import OAuth2Session

OAUTH_AUTH_URI = f'{settings.HYDRA_PUBLIC_ROOT}/oauth2/auth'
OAUTH_TOKEN_URI = f'{settings.HYDRA_PUBLIC_ROOT}/oauth2/token'

OAUTH_AUTH_URI = f'{settings.HYDRA_PUBLIC_ROOT}/oauth2/auth'
OAUTH_TOKEN_URI = f'http://hydra:4444/oauth2/token'


def get_oauth2_session() -> OAuth2Session:
    return OAuth2Session(
        client_id=settings.OAUTH_CLIENT_ID,
        redirect_uri=settings.OAUTH_CALLBACK_URI,
        scope=['openid'])


def protected(request):
    auth_header = request.headers['authorization']

    auth = auth_header.split(' ')

    if len(auth) != 2:
        return JsonResponse(status=401)

    token = auth[1]

    header = jwt.get_unverified_header(token)

    key = get_public_key(header['kid'])

    payload = jwt.decode(
        jwt=token,
        key=key,
        algorithms='RS256',
        audience=settings.CLIENT_ID)

    return JsonResponse({
        'success': True,
        'header': payload,
    })


def get_public_key(kid):
    key_data = get_keyset()[kid]
    key_json = json.dumps(key_data)
    return RSAAlgorithm.from_jwk(key_json)


def get_keyset():
    response = requests.get(settings.KEYSET_URL)
    response.raise_for_status()
    return {key['kid']: key for key in response.json()['keys']}


def login(request):
    return render_to_response('resourceserver/login.html')


def oauth_start(request):
    state = uuid.uuid1().hex
    client = get_oauth2_session()
    auth_url, state = client.authorization_url(
        url=OAUTH_AUTH_URI,
        state=state)
    return HttpResponseRedirect(auth_url)


def oauth_finish(request):
    state = request.GET.get('state')
    code = request.GET.get('code')
    scope = request.GET.get('scope')

    session = get_oauth2_session()
    token = session.fetch_token(
        token_url=OAUTH_TOKEN_URI,
        authorization_response=request.build_absolute_uri(),
        client_secret=settings.OAUTH_CLIENT_SECRET)

    identifier = secrets.token_urlsafe(16)
    cache.set(f'token:{identifier}', token)

    return HttpResponseRedirect(f'/token/{identifier}')


def view_token(request, identifier):
    token = cache.get(f'token:{identifier}')
    if not token:
        return JsonResponse({'detail': 'not found'}, status=404)
    return JsonResponse(token)
