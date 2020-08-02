namespace Compentio.Ankara.Models.Employees
{
    using Compentio.Ankara.Models.Companies;
    using Compentio.Ankara.Models.Dictionaries;
    using System;

    public class InternalRecruitment
    {
        public long Id { get; set; }
        public long EmployeeId { get; set; }
        public string Comment { get; set; }
        public Position Position { get; set; }
        public Department[] Departments { get; set; }
    }
}
