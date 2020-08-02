namespace Compentio.Ankara.Database
{
    public class Schema
    {
        public static string Name => "app";

        public static class Tables
        {
            public static string Users => "Users";
            public static string Dictionaries => "Dictionaries";
            public static string Params => "Params";
            public static string Logs => "Logs";            
            // Here you can add tables for youe App
        }
    }
}
