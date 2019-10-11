namespace Synchrolog.SDK.Helper
{
    interface IHttpContextWrapper
    {
        string GetAnonymousIs();
        string GetRequestIpAddress();
        string GetUserAgent();
        string GetUserId();
    }
}