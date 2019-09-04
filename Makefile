


build:
	docker-compose build

deps:
	docker-compose up -d postgresd
	docker-compose run --rm wait_for_postgres
	docker-compose run --rm hydra-migrate
	docker-compose up -d hydra
	docker-compose up -d idsrv

migrate:
	docker-compose exec postgresd createdb -U hydra resource || true
	docker-compose exec postgresd createdb -U hydra identity || true

	docker-compose run --rm resource python manage.py migrate
	docker-compose run --rm identity python manage.py migrate

	docker-compose run --rm identity python manage.py seed_data

setup: build deps migrate

run:
	docker-compose up -d identity
	docker-compose up -d resource
	docker-compose up -d dotnet-api
	docker-compose up -d js-spa

clean:
	docker-compose down -v

rebuild: setup run
