using Microsoft.AspNetCore.Http;

namespace Synchrolog.SDK.Helper
{
    class HttpContextWrapper : IHttpContextWrapper
    {
        const string ANONYMOUS_ID = "synchrolog_anonymous_id";
        const string USER_ID = "synchrolog_user_id";
        const string USER_AGENT = "User-Agent";

        private IHttpContextAccessor _httpContextAccessor;

        public HttpContextWrapper(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string GetRequestIpAddress()
        {
            return _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
        }

        public string GetUserAgent()
        {
            return _httpContextAccessor.HttpContext.Request.Headers[USER_AGENT].ToString();
        }

        public string GetAnonymousIs()
        {
            _httpContextAccessor.HttpContext.Request.Cookies.TryGetValue(ANONYMOUS_ID, out string anonymousId);

            return anonymousId ?? string.Empty;
        }

        public string GetUserId()
        {
            _httpContextAccessor.HttpContext.Request.Cookies.TryGetValue(USER_ID, out string userId);

            return userId ?? string.Empty;
        }
    }
}
