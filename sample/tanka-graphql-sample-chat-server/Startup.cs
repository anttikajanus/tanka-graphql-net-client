using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using tanka.graphql.server;
using Tanka.GraphQL.Sample.Chat.Server.Infrastructure;

namespace Tanka.GraphQL.Sample.Chat.Server
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            // Add schema initializer
            services.AddSingleton<IAsyncInitializer>(
                provider => provider.GetRequiredService<ChatSchemaInitializer>());
            services.AddSingleton<ChatSchemaInitializer>();

            services.AddSingleton(
                provider => provider.GetRequiredService<ChatSchemaInitializer>().Schema);

            // Add SignalR and add Tanka hub
            services.AddSignalR(options => { options.EnableDetailedErrors = true; })
                .AddQueryStreamHubWithTracing();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();

            // Use Tanka SignalR server hub
            app.UseSignalR(routes => routes.MapHub<QueryStreamHub>("/hubs/graphql"));

            // Use mvc
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller}/{action=Index}/{id?}");
            });
        }
    }
}
