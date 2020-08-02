namespace Compentio.Ankara.Models.Hierarchy
{
    using Compentio.Ankara.Models.Companies;
    using Compentio.Ankara.Models.Dictionaries;
    using System;
    using System.Collections.Generic;

    public class Hierarchy
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<Department> Departments { get; set; }
        public IEnumerable<Position> Levels { get; set; }
    }
}
