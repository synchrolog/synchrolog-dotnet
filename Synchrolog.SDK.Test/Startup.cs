using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Synchrolog.SDK.Test
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSynchrolog("API_KEY");
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            app.UseSynchrolog();

            loggerFactory.AddSynchrologProvider(app);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            var logger = loggerFactory.CreateLogger<Startup>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Synchrolog first page!");

                    logger.LogInformation("Testing SDK {SDK}...", "Synchrolog");
                });

                endpoints.MapGet("/error", async context =>
                {
                    await context.Response.WriteAsync("Synchrolog error page!");

                    throw new System.Exception("Testing Synchrolog SDK exception logging...");
                });
            });
        }
    }
}
