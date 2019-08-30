import logging

from django import forms
from django.conf import settings
from django.contrib.auth.views import LogoutView as AuthLogoutView
from django.http import HttpResponseRedirect
from django.views.generic import FormView
from hydra_client import HydraAdmin


logger = logging.getLogger('consentserver')


hydra = HydraAdmin(settings.HYDRA_ADMIN_ROOT)


class ConsentForm(forms.Form):
    pass


class ConsentView(FormView):
    template_name = 'identityserver/consent.html'
    form_class = ConsentForm

    def get_consent_challenge(self):
        if self.request.method == 'POST':
            return self.request.POST.get('consent_challenge')
        return self.request.GET.get('consent_challenge')

    def fetch_consent_request(self):
        return hydra.consent_request(self.get_consent_challenge())

    def accept_consent_request(self, user):
        consent_request = self.fetch_consent_request()
        redirect_to = consent_request.accept(
            remember=True,
            grant_scope=consent_request.requested_scope,
            grant_access_token_audience=consent_request.requested_access_token_audience,
            session={
            })

        return redirect_to

    def get(self, request, *args, **kwargs):
        consent_request = self.fetch_consent_request()
        logger.info("Consent challenge: %s", consent_request.challenge)
        return super().get(request, *args, **kwargs)

    def form_valid(self, form):
        """Security check complete. Log the user in."""
        redirect_url = self.accept_consent_request(None)
        return HttpResponseRedirect(redirect_url)

    def get_context_data(self, **kwargs):
        consent_request = self.fetch_consent_request()
        return super().get_context_data(
            consent_challenge=self.get_consent_challenge(),
            consent_request=consent_request.__dict__)
