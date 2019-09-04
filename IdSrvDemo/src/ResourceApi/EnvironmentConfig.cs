using System;

namespace ResourceApi
{
    public static class EnvironmentConfig
    {
        public static string TokenAuthority => Environment.GetEnvironmentVariable("IDSRV_ISSUER");
        public static string TokenEndpoint => Environment.GetEnvironmentVariable("IDSRV_TOKEN_ENDPOINT");
        public static string DotNetApiUrl => Environment.GetEnvironmentVariable("DOTNET_API_URL");
        public static string DummyScope => Environment.GetEnvironmentVariable("IDSRV_DUMMY_SCOPE");
        public static string ClientSecret => Environment.GetEnvironmentVariable("IDSRV_CLIENT_SECRET");
        public static string ClientCredentialsClientId => Environment.GetEnvironmentVariable("IDSRV_CLIENT_CREDENTIALS_CLIENTID");
    }
}