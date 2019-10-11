using System;

namespace Synchrolog.SDK.Model
{
    class PayloadModelBase
    {
        public string event_type { get; set; }
        public DateTime timestamp { get; set; }
        public string anonymous_id { get; set; }
        public string user_id { get; set; }
        public string source { get; set; }
    }
}
