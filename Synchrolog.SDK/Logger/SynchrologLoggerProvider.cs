using Microsoft.Extensions.Logging;
using Synchrolog.SDK.Client;
using Synchrolog.SDK.Helper;

namespace Synchrolog.SDK.Logger
{
    class SynchrologLoggerProvider : ILoggerProvider
    {
        private bool _isDisposed = false; // To detect redundant calls
        private ISynchrologClient _synchrologClient;
        private LogLevel _logLevel;
        private IHttpContextWrapper _httpContextWrapper;

        public SynchrologLoggerProvider(ISynchrologClient synchrologClient, LogLevel logLevel, IHttpContextWrapper httpContextWrapper)
        {
            _synchrologClient = synchrologClient;
            _logLevel = logLevel;
            _httpContextWrapper = httpContextWrapper;
        }

        public ILogger CreateLogger(string categoryName)
        {
            return new SynchrologLogger(_logLevel, categoryName, _synchrologClient, _httpContextWrapper);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_isDisposed)
            {
                if (disposing)
                {
                    _synchrologClient = null;
                    _httpContextWrapper = null;
                }

                _isDisposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
    }
}
