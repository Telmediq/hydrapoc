import logging
import requests
from django.contrib.auth.views import LoginView as AuthLoginView, LogoutView as AuthLogoutView
from django.http import HttpResponseRedirect

from django.conf import settings

LOGIN_REQUEST_URI = f'{settings.HYDRA_ADMIN_ROOT}/oauth2/auth/requests/login'
ACCEPT_REQUEST_URI = f'{settings.HYDRA_ADMIN_ROOT}/oauth2/auth/requests/login/accept'
REJECT_REQUEST_URI = f'{settings.HYDRA_ADMIN_ROOT}/oauth2/auth/requests/login/reject'

logger = logging.getLogger('identityserver')


def fetch_auth_request(login_challenge):
    params = {
        'login_challenge': login_challenge
    }
    response = requests.get(LOGIN_REQUEST_URI, params=params)
    response.raise_for_status()
    return response.json()


def accept_auth_request(login_challenge, user, remember=False, remember_for=3600, acr=None):
    params = {
        'login_challenge': login_challenge
    }

    data = {
        'subject': str(user.id),
        'remember': remember,
        'remember_for': remember_for,
    }

    if acr:
        data['acr'] = acr

    response = requests.put(ACCEPT_REQUEST_URI, params=params, json=data)

    if response.status_code == 400:
        error = response.json()
        logger.info("Error: %s - %s", error['error'], error['error_description'])

    response.raise_for_status()

    return response.json()


def deny_auth_request(login_challenge, error, error_description):
    params = {
        'login_challenge': login_challenge
    }

    data = {
        'error': error,
        'error_dsecription': error_description,
    }
    response = requests.put(REJECT_REQUEST_URI, params=params, json=data)
    response.raise_for_status()
    return response.json()


class LoginView(AuthLoginView):
    template_name = 'identityserver/login.html'

    def get_login_challenge(self):
        if self.request.method == 'POST':
            return self.request.POST.get('login_challenge')
        return self.request.GET.get('login_challenge')

    def fetch_auth_request(self):
        return fetch_auth_request(self.get_login_challenge())

    def accept_auth_request(self, user):
        response = accept_auth_request(
            login_challenge=self.get_login_challenge(),
            user=user,
            remember=False,
            remember_for=300)

        return response['redirect_to']

    def deny_auth_request(self, error, error_description):
        response = deny_auth_request(error, error_description)
        return response['redirect_to']

    def get(self, request, *args, **kwargs):
        auth_request = self.fetch_auth_request()
        logger.info("Login challenge: %s", auth_request)
        return super().get(request, *args, **kwargs)

    def form_valid(self, form):
        """Security check complete. Log the user in."""
        redirect_url = self.accept_auth_request(form.get_user())
        return HttpResponseRedirect(redirect_url)

    def get_context_data(self, **kwargs):
        return super().get_context_data(login_challenge=self.get_login_challenge())


class LogoutView(AuthLogoutView):
    template_name = 'identityserver/logged_out.html'


