using IdentityServer4.Models;
using IdentityServer4.Test;

namespace Identity
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddIdentityServer()
                .AddInMemoryClients(new List<Client>())
                .AddInMemoryIdentityResources(new List<IdentityResource>())
                .AddInMemoryApiResources(new List<ApiResource>())
                .AddInMemoryApiScopes(new List<ApiScope>())
                .AddTestUsers(new List<TestUser>())
                .AddDeveloperSigningCredential();

        }
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment()) 
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseRouting();

            app.UseIdentityServer();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet(pattern: "/", requestDelegate: async context =>
                {
                    await context.Response.WriteAsync("Hello World!");
                });
            });
        }
    }
}
