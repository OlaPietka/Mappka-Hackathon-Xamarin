using MappkaMobile.Enum;
using System.Collections.Generic;

namespace MappkaMobile.Db
{
    public static class DatabaseStrings
    {
        public static ServiceType DbName { get; set; }

        public static string ConnetionString { get => connetionString;}
        private const string connetionString = "Server=serverxd.database.windows.net;Database=test;User Id=xd;Password=!@34QWer1234;";
    }
}
