namespace Compentio.Ankara.Models.Companies
{
    using System.Collections.Generic;

    public class Department
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public long CompanyId { get; set; }
        public string ShortName { get; set; }
        public bool IsActive { get; set; }
        public List<Department> Children { get; set; }
    }
}
