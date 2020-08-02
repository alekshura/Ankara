namespace Compentio.Ankara.Models.Maintenance
{
    using System;

    public class LogEntry
    {
        public string Severity { get; set; }
        public string Message { get; set; }
        public string ExceptionType { get; set; }
        public DateTime Timestamp { get; set; }
        public string ExceptionMessage { get; set; }
        public string Logger { get; set; }
    }
}
