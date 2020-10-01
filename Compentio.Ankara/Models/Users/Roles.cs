namespace Compentio.Ankara.Models.Users
{
    using Compentio.Ankara.Models.Companies;
    using System.Collections.Generic;

    public class UserRole
    { 
        public Roles Role { get; set; }
        public IEnumerable<Department> Departments { get; set; }
    }

    public enum Roles
    {
        Manager = 1,
        TechAdmin,
        BusinessAdmin
    }
}
