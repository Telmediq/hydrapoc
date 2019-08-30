FROM    python:3.7-slim

# Ports exposed by default when this container is created (this is not port mapping).
EXPOSE  8000

RUN     set -ex; \
        apt-get update; \
        # Install permanent dependencies.
        # For pg_config / postgres
        apt-get install -y --no-install-recommends libpq-dev; \
        # For psql binary / postgres
        mkdir -p /usr/share/man/man1 /usr/share/man/man7; \
        apt-get install -y --no-install-recommends postgresql-client; \
        # Install temporary dependencies.
        apt-get install -y --no-install-recommends gcc;

COPY    ./requirements.txt /etc/
RUN     pip install --no-cache-dir -r /etc/requirements.txt;


COPY    . /opt/app

# Directory to run from.
WORKDIR /opt/app

CMD ["python", "manage.py", "runserver", "0.0.0.0:8000"]
