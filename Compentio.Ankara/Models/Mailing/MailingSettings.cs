namespace Compentio.Ankara.Models.Mailing
{
    using Compentio.Ankara.Models.Companies;
    using System;
    using System.Data;
    using System.Runtime.Serialization;
    public class SmtpSettings
    {
        public string Address { get; set; }
        public string Host { get; set; }
        public long Port { get; set; }

        public string User { get; set; }
        public string Password { get; set; }

        public string Security { get; set; }
    }
}
