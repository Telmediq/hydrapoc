using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ResourceApi.Controllers
{
    [Route("authtest")]
    [Authorize]
    public class AuthTestController : ControllerBase
    {
        /// <summary>
        /// Echos back the claims seen on the bearer token
        /// </summary>
        [Route("echo-my-claims")]
        [HttpGet]
        public IActionResult EchoMyClaims()
        {
            return new JsonResult(from c in User.Claims select new { c.Type, c.Value });
        }

        /// <summary>
        /// The API calls itself using client credentials to output the server token's claims
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        [Route("echo-servers-claims")]
        [HttpGet]
        public async Task<IActionResult> EchoServersClaims()
        {
            string token = null;
            using (var client = new HttpClient())
            {
                var tokenResponse = await client.RequestClientCredentialsTokenAsync(
                    new ClientCredentialsTokenRequest
                    {
                        Address = EnvironmentConfig.TokenEndpoint,
                        ClientId = EnvironmentConfig.ClientCredentialsClientId,
                        ClientSecret = EnvironmentConfig.ClientSecret,
                        Scope = EnvironmentConfig.DummyScope
                    });

                if (tokenResponse.IsError)
                {
                    throw new Exception(tokenResponse.Error);
                }

                token = tokenResponse.AccessToken;
            }
            
            using (var client = new HttpClient())
            {
                client.SetBearerToken(token);

                var response = await client.GetAsync($"{EnvironmentConfig.DotNetApiUrl}/authtest/echo-my-claims");
                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception($"API call failed: {await response.Content.ReadAsStringAsync()}");
                }
                
                return Ok(await response.Content.ReadAsStringAsync());
            }
        }
    }
}