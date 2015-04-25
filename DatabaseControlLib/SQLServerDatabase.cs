using System;
using System.Data ;
using System.Data.SqlClient ;
using System.Data.OleDb ;

namespace DatabaseControlLib
{
	/// <summary>
	/// Summary description for SQLServerDatabase.
	/// </summary>
	public class SQLServerDatabase : DBase
    {
		string connectionString;

		//使用 SQL Server 账号登录
		public SQLServerDatabase(string dbName, string serverName, string userid, string pwd) 
        {
			 connectionString = "Persist Security Info = False;" +
                "Initial Catalog =" + dbName + ";" +
                "Data Source =" + serverName + ";" +
                "User ID =" + userid + ";" +
                "password=" + pwd;
			openConnection(connectionString);
        }

        //使用系统登录账号登录
        public SQLServerDatabase(string dbName, string serverName)
        {
            connectionString = "Provider = SQLOLEDB;" + 
                "Persist Security Info = False;" + 
                "Data Source =" + serverName + ";" +
                "Initial Catalog =" + dbName + ";" +
                "Integrated Security = SSPI";
            openConnection(connectionString);
        }
	}
}
