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
		protected string tableName;         //���ݱ������
        protected OleDbConnection conn;     //���ݿ�����

        //���캯��        
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

        //��ȡȫ������
        public DataTable GetAllDataDT()
        {
            string query = "Select * from " + tableName + ";";
            return db.openQueryDT(query,tableName);
        }

        //��ȡȫ������
        public DataSet GetAllDataDS()
        {
            string query = "Select * from " + tableName + ";";
            return db.openQueryDS(query,tableName);
        }

        //�������ݱ�����
        public string GetTableName()
        {
            return tableName;
        }
	}
}
