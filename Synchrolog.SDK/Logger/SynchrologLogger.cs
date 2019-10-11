using Microsoft.Extensions.Logging;
using Synchrolog.SDK.Client;
using Synchrolog.SDK.Helper;
using System;

namespace Synchrolog.SDK.Logger
{
    class SynchrologLogger : ILogger
    {
        private LogLevel _logLevel;
        private ISynchrologClient _synchrologClient;
        private IHttpContextWrapper _httpContextWrapper;

        public SynchrologLogger(LogLevel logLevel, ISynchrologClient synchrologClient, IHttpContextWrapper httpContextWrapper)
        {
            _logLevel = logLevel;
            _synchrologClient = synchrologClient;
            _httpContextWrapper = httpContextWrapper;
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return logLevel >= _logLevel;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            var anonymousId = _httpContextWrapper.GetAnonymousIs();
            var userId = _httpContextWrapper.GetUserId();
            var requestIpAddress = _httpContextWrapper.GetRequestIpAddress();
            var requestUserAgent = _httpContextWrapper.GetUserAgent();
            var timeStamp = DateTime.UtcNow;

            if (exception == null)
            {
                _synchrologClient.TrackLogAsync(anonymousId, userId, timeStamp, formatter(state, exception));
            }
            else
            {
                _synchrologClient.TrackErrorAsync(anonymousId, userId, timeStamp, exception, requestIpAddress, requestUserAgent);
            }
        }
    }
}
