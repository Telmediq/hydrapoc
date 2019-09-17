// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4.Models;
using System.Collections.Generic;
using IdentityServer4;
using IdentityServer4.Test;

namespace IdentityServer
{
    public static class Config
    {
        /// <summary>
        /// In real code, this would be coming from a configuration database for IdentityServer,
        /// e.g. Postgres
        /// </summary>
        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new IdentityResource[]
            {
                new IdentityResources.OpenId(), new IdentityResources.Profile()
            };
        }

        /// <summary>
        /// In real code, this would be coming from a configuration database for IdentityServer,
        /// e.g. Postgres
        /// </summary>
        public static IEnumerable<ApiResource> GetApis()
        {
            return new List<ApiResource>
            {
                new ApiResource("api1", "My API")
            };
        }

        /// <summary>
        /// In real code, this would be coming from a configuration database for IdentityServer,
        /// e.g. Postgres
        /// </summary>
        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>
            {
                // For service-to-service authentication
                new Client
                {
                    ClientId = EnvironmentConfig.ClientCredentialsClientId,
                    
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    
                    ClientSecrets =
                    {
                        new Secret(EnvironmentConfig.ClientSecret.Sha256())
                    },
                    
                    AllowedScopes = { EnvironmentConfig.DummyScope }
                },
                
                // For interactive authentication by user
                new Client
                {
                    ClientId = EnvironmentConfig.AuthCodeClientId,
                    ClientName = "JavaScript Client",
                    AllowedGrantTypes = GrantTypes.Code,
                    RequirePkce = false,
                    RequireClientSecret = false,

                    RedirectUris =           EnvironmentConfig.RedirectUris,
                    PostLogoutRedirectUris = EnvironmentConfig.PostLogoutUris,
                    AllowedCorsOrigins =     EnvironmentConfig.AllowedCors,

                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        EnvironmentConfig.DummyScope
                    }
                }
            };
        }

        /// <summary>
        /// Test users. In real code, these would either be stored in some local storage,
        /// like Postgres or AD, or we'd be using some upstream OIDC provider like AzureAD.
        /// </summary>
        public static List<TestUser> GetUsers()
        {
            return new List<TestUser>
            {
                new TestUser
                {
                    SubjectId = "1",
                    Username = "bob@telmediq.com",
                    Password = "password123"
                },
                new TestUser
                {
                    SubjectId = "2",
                    Username = "bill@perfectserve.net",
                    Password = "password123"
                }
            };
        }
    }
}