using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace synchrolog_dotnet
{
    class SynchrologMiddleware
    {
        private readonly RequestDelegate _next;
        const string ANONYMOUS_ID = "synchrolog_anonymous_id";
        const string USER_ID = "synchrolog_user_id";

        public SynchrologMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, ISynchrologClient synchrologClient)
        {
            if(context.Request.Path.Value == "/synchrolog-time")
            {
                context.Response.StatusCode = 200;
                context.Response.ContentType = "application/json";

                await context.Response.WriteAsync(JsonConvert.SerializeObject(new { time = DateTime.UtcNow }));
            }
            else
            {
                if (!context.Request.Cookies.TryGetValue(ANONYMOUS_ID, out string anonymousId))
                {
                    anonymousId = Guid.NewGuid().ToString();

                    context.Response.Cookies.Append(ANONYMOUS_ID, anonymousId);
                }

                if(!context.Request.Cookies.TryGetValue(USER_ID, out string userId))
                {
                    userId = context.Request.HttpContext.User.Identity.Name;

                    if (!string.IsNullOrWhiteSpace(userId))
                    {
                        context.Response.Cookies.Append(USER_ID, userId);
                    }
                }

                try
                {
                    await _next(context);
                }
                catch (Exception ex)
                {
                    var requestIpAddress = context.Request.HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
                    var userAgent = context.Request.Headers["User-Agent"].ToString();

                    await synchrologClient.TrackErrorAsync(anonymousId, userId, DateTime.UtcNow, ex
                        , requestIpAddress, userAgent);

                    throw;
                }
            }
        }
    }
}
