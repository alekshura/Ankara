namespace Compentio.Ankara.Models.Maintenance
{
    using System;

    public class VersionInfo
    {
        public DatabaseVersionInfo DatabaseVersion { get; set; }
        public string AssemblyVersion { get; set; }
    }
    public class DatabaseVersionInfo
    {
        public long Version { get; set; }
        public DateTime AppliedOn { get; set; }
        public string Description { get; set; }
    }
}
