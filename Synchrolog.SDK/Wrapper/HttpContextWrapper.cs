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

        public bool HasAnonymousId()
        {
            return !string.IsNullOrEmpty(GetAnonymousId());
        }

        public string GetRequestIpAddress()
        {
            return _httpContextAccessor.HttpContext?.Connection.RemoteIpAddress.MapToIPv4().ToString();
        }

        public string GetUserAgent()
        {
            return _httpContextAccessor.HttpContext?.Request.Headers[USER_AGENT].ToString();
        }

        public string GetAnonymousId()
        {
            var anonymousId = default(string);

            if (IsInsideHttpContext())
            {
                _httpContextAccessor.HttpContext.Request.Cookies.TryGetValue(ANONYMOUS_ID, out anonymousId);
            }

            return anonymousId ?? string.Empty;
        }

        public string GetUserId()
        {
            var userId = default(string);

            if (IsInsideHttpContext())
            {
                _httpContextAccessor.HttpContext.Request.Cookies.TryGetValue(USER_ID, out userId);
            }

            return userId ?? string.Empty;
        }

        bool IsInsideHttpContext()
        {
            return _httpContextAccessor.HttpContext != null;
        }
    }
}
