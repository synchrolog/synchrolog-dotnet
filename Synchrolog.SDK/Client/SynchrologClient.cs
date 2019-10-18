using Newtonsoft.Json;
using Synchrolog.SDK.Helper;
using Synchrolog.SDK.Model;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Synchrolog.SDK.Client
{
    class SynchrologClient : ISynchrologClient
    {
        private readonly HttpClient _httpClient;

        public SynchrologClient(HttpClient httpClient)
        {
            httpClient.BaseAddress = new Uri("https://input.synchrolog.com");

            httpClient.DefaultRequestHeaders.Add("Accept", "application/json");

            _httpClient = httpClient;
        }

        public async Task TrackLogAsync(string anonymousId, string userId, DateTime timeStamp, string message)
        {
            var payload = new PayloadLogModel
            {
                anonymous_id = anonymousId,
                event_type = "log",
                source = "backend",
                timestamp = timeStamp,
                user_id = userId,
                log = new LogModel
                {
                    message = message,
                    timestamp = timeStamp
                }
            };

            await PostPayloadAsync(payload, "/v1/track-backend");
        }

        public async Task TrackErrorAsync(string anonymousId, string userId, DateTime timeStamp, Exception exception
            , string requestIpAddress, string requestUserAgent)
        {
            var payload = new PayloadErrorModel
            {
                anonymous_id = anonymousId,
                event_type = "error",
                source = "backend",
                timestamp = timeStamp,
                user_id = userId,
                error = new ErrorModel
                {
                    backtrace = exception.StackTrace,
                    description = exception.Message,
                    line_number = exception.Data.Contains(nameof(ErrorModel.line_number))
                        ? (int)exception.Data[nameof(ErrorModel.line_number)] : default,
                    ip_address = requestIpAddress,
                    status = exception.HResult.ToString(),
                    file = exception.Data.Contains(nameof(ErrorModel.file))
                        ? (string)exception.Data[nameof(ErrorModel.file)] : string.Empty,
                    file_name = exception.Source,
                    user_agent = requestUserAgent
                }
            };

            await PostPayloadAsync(payload, "/v1/track-backend-error");
        }

        private async Task PostPayloadAsync(PayloadModelBase payload, string requestUri)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, requestUri);

            var content = JsonConvert.SerializeObject(payload, new Iso8601DateTimeConverter());

            request.Content = new StringContent(content, Encoding.UTF8, "application/json");

            await _httpClient.SendAsync(request);
        }
    }
}
