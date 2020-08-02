namespace Compentio.Ankara.Models.Users
{
    using Compentio.Ankara.Models.Companies;
    using System;
    using System.Collections.Generic;

    public class User
    {
        public long Id { get; set; }
        public Department Department { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Login { get; set; }
        public string PersonalNumber { get; set; }
        public string Email { get; set; }
        public string Position { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }
        public IEnumerable<UserRole> Roles { get; set; }
        public override string ToString()
        {
            return $"Id: '{Id}', PersonalNumber: '{PersonalNumber}', Login: '{Login}', FirstName: '{FirstName}', LastName: '{LastName}', Email: '{Email}'";
        }
    }
}
