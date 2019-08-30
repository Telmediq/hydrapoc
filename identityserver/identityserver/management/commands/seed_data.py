from django.conf import settings
from django.contrib.auth.models import User
from django.core.management import BaseCommand
from hydra_client import HydraAdmin
from hydra_client.exceptions import NotFound

hydra = HydraAdmin(settings.HYDRA_ADMIN_ROOT)


class Command(BaseCommand):

    def handle(self, *args, **options):

        User.objects.update_or_create(
            username='bob@telmediq.com',
            defaults={
                'first_name': 'Bob',
                'last_name': 'Saget',
                'password': 'pbkdf2_sha256$150000$xRVIPjFux19s$OKNh+bs4Eo/a/QPKyT/g0xhwlLFM7eYWiH3ckG5Y0vo='
            })

        User.objects.update_or_create(
            username='admin@telmediq.com',
            defaults={
                'is_superuser': True,
                'is_staff': True,
                'password': 'pbkdf2_sha256$150000$dNR1eTpKMxM1$X9q6qm+604eImNzGYTNh/cqQBnoNk6bEYvPuemf+f0I='
            })

        try:
            client = hydra.client(settings.OAUTH_CLIENT_ID)
        except NotFound:
            hydra.create_client(
                client_id=settings.OAUTH_CLIENT_ID,
                client_secret=settings.OAUTH_CLIENT_SECRET,
                scope='openid offline',
                audience=[settings.OAUTH_CLIENT_ID],
                redirect_uris=[settings.OAUTH_CALLBACK_URI],
                subject_type='public')
        else:
            client.update(
                client_secret=settings.OAUTH_CLIENT_SECRET,
                scope='openid offline',
                audience=[settings.OAUTH_CLIENT_ID],
                redirect_uris=[settings.OAUTH_CALLBACK_URI],
                subject_type='public')
