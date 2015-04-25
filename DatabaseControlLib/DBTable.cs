using System;
using System.Data ;
using System.Data.SqlClient ;
using System.Data.OleDb ;
using System.Collections ;

namespace DatabaseControlLib
{
	/// <summary>
	/// Summary description for DBTable.
	/// </summary>
	public class DBTable 	
    {
		protected DBase db;             
		protected string tableName;         //数据表的名称
        protected OleDbConnection conn;     //数据库连接

        //构造函数        
		public DBTable(DBase datab, string tb_Name)
        {
			db = datab;
			tableName = tb_Name;
            conn = datab.getConnection();
		}

        public void openConn()
        {
            if (conn.State == ConnectionState.Closed)
                conn.Open();
        }

        public void closeConn()
        {
            if (conn.State == ConnectionState.Open)
                conn.Close();
        }

        //获取全部数据
        public DataTable GetAllDataDT()
        {
            string query = "Select * from " + tableName + ";";
            return db.openQueryDT(query,tableName);
        }

        //获取全部数据
        public DataSet GetAllDataDS()
        {
            string query = "Select * from " + tableName + ";";
            return db.openQueryDS(query,tableName);
        }

        //返回数据表名称
        public string GetTableName()
        {
            return tableName;
        }
	}
}
