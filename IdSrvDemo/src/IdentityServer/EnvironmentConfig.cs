using System;

namespace IdentityServer
{
    public static class EnvironmentConfig
    {
        public static string TokenAuthority => Environment.GetEnvironmentVariable("IDSRV_ISSUER");
        public static string TokenEndpoint => Environment.GetEnvironmentVariable("IDSRV_TOKEN_ENDPOINT");
        public static string DotNetApiUrl => Environment.GetEnvironmentVariable("DOTNET_API_URL");
        
        public static string[] RedirectUris => Environment.GetEnvironmentVariable("IDSRV_REDIRECT_URIS").Split(',');
        public static string[] PostLogoutUris => Environment.GetEnvironmentVariable("IDSRV_POSTLOGOUT_URIS").Split(',');
        public static string[] AllowedCors => Environment.GetEnvironmentVariable("IDSRV_ALLOWED_CORS").Split(',');
        
        public static string ClientSecret => Environment.GetEnvironmentVariable("IDSRV_CLIENT_SECRET");
        public static string ClientCredentialsClientId => Environment.GetEnvironmentVariable("IDSRV_CLIENT_CREDENTIALS_CLIENTID");
        public static string AuthCodeClientId => Environment.GetEnvironmentVariable("IDSRV_AUTH_CODE_CLIENTID");
        public static string DummyScope => Environment.GetEnvironmentVariable("IDSRV_DUMMY_SCOPE");
    }
}