namespace Compentio.Ankara.Models.Employees
{
    using Compentio.Ankara.Models.Companies;
    using Compentio.Ankara.Models.Dictionaries;

    public class Successor
    {
        public long SuccessorEmployeeId { get; set; }
        public long EmployeeId { get; set; }
        public Dictionary TrainingSubject { get; set; }
        public Dictionary TrainingTool { get; set; }

        //public long Id { get; set; }
        //public long DepartmentId { get; set; }
        //public long EmployeeId { get; set; }
        //public long EmployeeDepartmentId { get; set; }
        //public string FirstName { get; set; }
        //public string LastName { get; set; }
        //public string Login { get; set; }
        //public string PersonalNumber { get; set; }
        //public Department Department { get; set; } = new Department();
        //public string Position { get; set; }
        //public bool IsSelected { get; set; } = false;

    }
}
