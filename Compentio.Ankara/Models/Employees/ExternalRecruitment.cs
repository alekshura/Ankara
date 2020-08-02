namespace Compentio.Ankara.Models.Employees
{
    using Compentio.Ankara.Models.Dictionaries;
    using System;

    public class ExternalRecruitment
    {
        public long Id { get; set; }
        public long EmployeeId { get; set; }
        public string Comment { get; set; }
        public Position Position { get; set; }
        public DateTime StartDate { get; set; }
        public bool Tabbed { get; set; }
    }
}
