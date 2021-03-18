using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SizzlingHot
{
    class ConnectionInfo
    {
        private static string _SERVER = "server=localhost; ";
        private static string _DATABASE = "database=netdb; ";
        private static string _USER = "user=root; ";
        private static string _PWD = "pwd=";

        protected ConnectionInfo()
        {

        }

        public static string GetConnectionString()
        {
            return _SERVER + _DATABASE + _USER + _PWD;
        }
    }
}
