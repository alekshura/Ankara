namespace Compentio.Ankara.Models.Dictionaries
{
    using Compentio.Ankara.Models.Companies;
    using System.Collections.Generic;

    public class Dictionary
    {
        public string Id { get; set; }
        public DictionaryCategory Category { get; set; }   
        public string Value { get; set; }
        public IEnumerable<Department> Departments { get; set; }

    }
}
