using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using DatabaseControlLib;

namespace ManagementSystem
{
    public class ConnectionInfo
    {
        public static string DBName = "SaleSystem";
        public static string ServerName = "(local)";
    }
    public class SQLConnection
    {
        public static DBase GetDataBase()
        {
            return (new SQLServerDatabase(ConnectionInfo.DBName,
                ConnectionInfo.ServerName));
        }
    }
}
