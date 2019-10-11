using Microsoft.Extensions.Logging;
using Synchrolog.SDK.Client;
using Synchrolog.SDK.Helper;

namespace Synchrolog.SDK.Logger
{
    class SynchrologLoggerProvider : ILoggerProvider
    {
        private bool _isDisposed = false; // To detect redundant calls
        private ISynchrologClient _synchrologClient;
        private SynchrologLogger _synchlogLogger;
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
            if (_synchlogLogger == null)
            {
                _synchlogLogger = new SynchrologLogger(_logLevel, _synchrologClient, _httpContextWrapper);
            }

            return _synchlogLogger;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_isDisposed)
            {
                if (disposing)
                {
                    _synchlogLogger = null;
                }

                _isDisposed = true;
            }
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            Dispose(true);
        }
    }
}
