using System;

namespace ConsoleClient
{
    public static class EnvironmentConfig
    {
        public static string TokenEndpoint => Environment.GetEnvironmentVariable("IDSRV_TOKEN_ENDPOINT");
        public static string DotNetApiUrl => Environment.GetEnvironmentVariable("DOTNET_API_URL");

        public static string ClientSecret => Environment.GetEnvironmentVariable("IDSRV_CLIENT_SECRET");
        public static string ClientCredentialsClientId => Environment.GetEnvironmentVariable("IDSRV_CLIENT_CREDENTIALS_CLIENTID");
        public static string DummyScope => Environment.GetEnvironmentVariable("IDSRV_DUMMY_SCOPE");
        
    }
}