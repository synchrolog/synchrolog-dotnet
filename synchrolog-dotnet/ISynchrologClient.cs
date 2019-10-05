using System;
using System.Threading.Tasks;

namespace synchrolog_dotnet
{
    interface ISynchrologClient
    {
        Task TrackErrorAsync(string anonymousId, string userId, DateTime timeStamp, Exception exception
            , string requestIpAddress, string requestUserAgent);
        Task TrackLogAsync(string anonymousId, string userId, DateTime timeStamp, string message);
    }
}