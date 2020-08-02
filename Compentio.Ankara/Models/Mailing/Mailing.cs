namespace Compentio.Ankara.Models.Mailing
{
    using Compentio.Ankara.Models.Companies;
    using System;
    using System.Data;
    using System.Runtime.Serialization;

    public class MailingTemplate
    {
        public long Id { get; set; }
        public string Subject { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public string From { get; set; }
        public string Reply { get; set; }
        public int Sending { get; set; }
    }

    public enum SendingOption
    {
        EveryWeek = 1,
        EveryMonth = 2,
        EveryDays = 3,
        EveryYear = 4
    }
}
