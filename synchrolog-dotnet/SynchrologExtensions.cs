using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace synchrolog_dotnet
{
    public static class SynchrologExtensions
    {
        public static IApplicationBuilder UseSynchrolog(this IApplicationBuilder applicationBuilder)
        {
            return applicationBuilder.UseMiddleware<SynchrologMiddleware>();
        }

        public static IServiceCollection AddSynchrolog(this IServiceCollection services, string accessToken)
        {
            services.AddHttpClient<ISynchrologClient, SynchrologClient>(client =>
            {
                client.DefaultRequestHeaders.Add("Authorization", $"Basic {accessToken}");
            });

            return services;
        }
    }
}
