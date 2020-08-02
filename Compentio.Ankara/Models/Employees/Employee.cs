namespace Compentio.Ankara.Models.Employees
{
    using Compentio.Ankara.Models.Companies;
    using System;
    using System.Collections.Generic;
    using System.Data;

    public class Employee
    {
        public long Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Login { get; set; }
        public string PersonalNumber { get; set; }
        public string Email { get; set; }
        public Department Department { get; set; } = new Department();
        public string Position { get; set; }
        public DateTime DateTo { get; set; }
        public DateTime? DateManual { get; set; }
        public string Comment { get; set; }
        public DateTime DateToFull { get; set; }
        public bool IsSuccessor { get; set; } = false;
        public EmployeeAction? Action { get; set; }
        public IEnumerable<Successor> Successors { get; set; }
    }
}
