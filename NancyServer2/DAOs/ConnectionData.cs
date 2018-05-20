using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NancyServer2.DAOs
{
    public static class ConnectionData
    {
        public static string Database { get; set; }
        public static string Server { get; set; }
        public static string AuthenticationString { get; set; }
    }
}
