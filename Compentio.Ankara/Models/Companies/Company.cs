namespace Compentio.Ankara.Models.Companies
{
    public class Company
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
        public long RootDepartmentId { get; set; }
        public bool IsActive { get; set; } = false;
    }
}
