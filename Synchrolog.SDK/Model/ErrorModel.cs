namespace Synchrolog.SDK.Model
{
    class ErrorModel
    {
        public string status { get; set; }
        public string description { get; set; }
        public string backtrace { get; set; }
        public string ip_address { get; set; }
        public string user_agent { get; set; }
        public string file_name { get; set; }
        public int? line_number { get; set; }
        public string file { get; set; }
    }
}
