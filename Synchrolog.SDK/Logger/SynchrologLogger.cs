using Microsoft.Extensions.Logging;
using Synchrolog.SDK.Client;
using Synchrolog.SDK.Helper;
using System;

namespace Synchrolog.SDK.Logger
{
    class SynchrologLogger : ILogger
    {
        private LogLevel _logLevel;
        private string _categoryName;
        private ISynchrologClient _synchrologClient;
        private IHttpContextWrapper _httpContextWrapper;

        public SynchrologLogger(LogLevel logLevel, string categoryName, ISynchrologClient synchrologClient, IHttpContextWrapper httpContextWrapper)
        {
            _logLevel = logLevel;
            _categoryName = categoryName;
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
            if (_httpContextWrapper.HasAnonymousId() && IsEnabled(logLevel)
                && !_categoryName.StartsWith("System.Net.Http.HttpClient.ISynchrologClient"))
            {
                var anonymousId = _httpContextWrapper.GetAnonymousId();
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
}
