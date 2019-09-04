using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace ResourceApi
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvcCore()
                .AddAuthorization()
                .AddJsonFormatters();

            Microsoft.IdentityModel.Logging.IdentityModelEventSource.ShowPII = true;
            
            services.AddAuthentication("Bearer")
                .AddJwtBearer("Bearer", options =>
                {
                    options.Authority = EnvironmentConfig.TokenAuthority;
                    options.RequireHttpsMetadata = false;

                    options.Audience = EnvironmentConfig.DummyScope;
                });
            
            services.AddCors(options =>
            {
                options.AddPolicy("default", policy =>
                {
                    policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
                });
            });
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseAuthentication();
            app.UseCors("default");
            app.UseMvc();
        }    
    }
}
