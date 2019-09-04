using System;
using System.Net.Http;
using System.Threading.Tasks;
using IdentityModel.Client;
using Newtonsoft.Json.Linq;

namespace ConsoleClient
{
    class Program
    {
        static async Task<string> GetToken()
        {
            using (var client = new HttpClient())
            {               
                var tokenResponse = await client.RequestClientCredentialsTokenAsync(
                    new ClientCredentialsTokenRequest
                    {
                        Address =  EnvironmentConfig.TokenEndpoint,
                        ClientId = EnvironmentConfig.ClientCredentialsClientId,
                        ClientSecret = EnvironmentConfig.ClientSecret,
                        Scope = EnvironmentConfig.DummyScope
                    });

                if (tokenResponse.IsError)
                {
                    throw new Exception(tokenResponse.Error);
                }

                return tokenResponse.AccessToken;
            }
        }

        static async Task CallApi(string token)
        {
            using (var client = new HttpClient())
            {
                client.SetBearerToken(token);

                var response = await client.GetAsync($"{EnvironmentConfig.DotNetApiUrl}/authtest/echo-my-claims");
                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine(response.StatusCode);
                }
                else
                {
                    var content = await response.Content.ReadAsStringAsync();
                    Console.WriteLine(JArray.Parse(content));
                }
            }
        }

        static async Task Main(string[] args)
        {
            foreach (var v in Environment.GetEnvironmentVariables().Keys)
            {
                Console.WriteLine($"{v}: {Environment.GetEnvironmentVariable(v.ToString())}");
            }

            var token = await GetToken();
            Console.WriteLine($"Got token: {token}");
            await CallApi(token);
        }
    }
}
