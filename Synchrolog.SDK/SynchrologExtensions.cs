using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Synchrolog.SDK.Client;
using Synchrolog.SDK.Helper;
using Synchrolog.SDK.Logger;
using Synchrolog.SDK.Middleware;

namespace Synchrolog.SDK
{
    public static class SynchrologExtensions
    {
        public static IApplicationBuilder UseSynchrolog(this IApplicationBuilder applicationBuilder)
        {
            return applicationBuilder.UseMiddleware<SynchrologMiddleware>();
        }

        public static IServiceCollection AddSynchrolog(this IServiceCollection services, string accessToken)
        {
            services.AddHttpContextAccessor();

            services.AddSingleton<IHttpContextWrapper, HttpContextWrapper>();

            services.AddHttpClient<ISynchrologClient, SynchrologClient>(client =>
            {
                client.DefaultRequestHeaders.Add("Authorization", $"Basic {accessToken}");
            });

            return services;
        }

        public static ILoggerFactory AddSynchrologProvider(this ILoggerFactory loggerFactory, IApplicationBuilder app, LogLevel logLevel)
        {
            loggerFactory.AddProvider(new SynchrologLoggerProvider(app.ApplicationServices.GetService<ISynchrologClient>()
                , logLevel, app.ApplicationServices.GetService<IHttpContextWrapper>()));

            return loggerFactory;
        }
    }
}
