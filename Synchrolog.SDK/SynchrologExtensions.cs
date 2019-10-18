using Microsoft.AspNetCore.Builder;
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
        /// <summary>
        /// Adds the Synchrolog middleware to your application
        /// </summary>
        /// <param name="applicationBuilder">The applications builder used to configure middlewares</param>
        /// <returns>Application builder</returns>
        public static IApplicationBuilder UseSynchrolog(this IApplicationBuilder applicationBuilder)
        {
            return applicationBuilder.UseMiddleware<SynchrologMiddleware>();
        }

        /// <summary>
        /// Adds the Synchrolog service dependencies
        /// </summary>
        /// <param name="services">The service collection used to add service dependencies</param>
        /// <param name="accessToken">Your application API key to access Synchrolog service</param>
        /// <returns></returns>
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

        /// <summary>
        /// Adds the logger provider used to send logs to Synchrolog
        /// </summary>
        /// <param name="loggerFactory">The object used to add logger providers</param>
        /// <param name="app">The application builder used to retrive service Synchrolog dependencies</param>
        /// <param name="logLevel">The log levlel used to filter logs greater or equal to logLevel. Defaults to Trace (first level)
        /// that includes all levels.</param>
        /// <returns>Logger factory</returns>
        public static ILoggerFactory AddSynchrologProvider(this ILoggerFactory loggerFactory, IApplicationBuilder app
            , LogLevel logLevel = LogLevel.Trace)
        {
            loggerFactory.AddProvider(new SynchrologLoggerProvider(app.ApplicationServices.GetService<ISynchrologClient>()
                , logLevel, app.ApplicationServices.GetService<IHttpContextWrapper>()));

            return loggerFactory;
        }
    }
}
