namespace Synchrolog.SDK.Helper
{
    interface IHttpContextWrapper
    {
        bool HasAnonymousId();
        string GetAnonymousId();
        string GetRequestIpAddress();
        string GetUserAgent();
        string GetUserId();
    }
}