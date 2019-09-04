docker run \
    -e "IDSRV_TOKEN_ENDPOINT=http://idsrv/connect/token" \
    -e "DOTNET_API_URL=http://dotnet-api" -e "IDSRV_CLIENT_SECRET=poc-secret" \
    -e "IDSRV_CLIENT_CREDENTIALS_CLIENTID=poc-client" \
    -e "IDSRV_DUMMY_SCOPE=api1" \
    --network=hydrapoc_default \
    hydrapoc_console-client