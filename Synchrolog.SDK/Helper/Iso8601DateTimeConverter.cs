using Newtonsoft.Json.Converters;

namespace Synchrolog.SDK.Helper
{
    class Iso8601DateTimeConverter : IsoDateTimeConverter
    {
        public Iso8601DateTimeConverter()
        {
            base.DateTimeFormat = "yyyy-MM-ddTHH:mm:ss.fffZ";
        }
    }
}
