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

		//ʹ�� SQL Server �˺ŵ�¼
		public SQLServerDatabase(string dbName, string serverName, string userid, string pwd) 
        {
			 connectionString = "Persist Security Info = False;" +
                "Initial Catalog =" + dbName + ";" +
                "Data Source =" + serverName + ";" +
                "User ID =" + userid + ";" +
                "password=" + pwd;
			openConnection(connectionString);
        }

        //ʹ��ϵͳ��¼�˺ŵ�¼
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
