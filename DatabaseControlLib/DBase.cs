using System;
using System.Data ;
using System.Data.SqlClient ;
using System.Data.OleDb ;

namespace DatabaseControlLib
{
	/// <summary>
	/// Summary description for DBase.
	/// </summary>
	public abstract class DBase 	{
		protected OleDbConnection conn;
		public void openConnection() {
			if (conn.State == ConnectionState.Closed){
				conn.Open ();
			}
		}
		//------
		public void closeConnection() {
			if (conn.State == ConnectionState.Open ){
				conn.Close ();
			}
		}
		//------
		public void openConnection(string connectionString) {
			conn = new OleDbConnection(connectionString);
		}
		//------
		public OleDbConnection getConnection() {
			return conn;
		}

        //执行查询
        public DataTable openQueryDT(string query,string table = "Table")
        {
            DataTable dtable = null;
            OleDbDataAdapter odda = new OleDbDataAdapter();
            DataSet dset = new DataSet();       //创建数据集对象

            try
            {
                //创建命令对象
                odda.SelectCommand = new OleDbCommand(query, conn);
                openConnection();       //打开连接

                //填充数据集对象
                odda.Fill(dset,table);

                //取得表
                dtable = dset.Tables[0];               
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                closeConnection();      //关闭连接
            }

            return dtable;
        }

        //执行查询
        public DataSet openQueryDS(string query,string table = "Table")
        {     
            OleDbDataAdapter odda = new OleDbDataAdapter();
            DataSet dset = new DataSet();       //创建数据集对象

            try
            {
                //创建命令对象
                odda.SelectCommand = new OleDbCommand(query, conn);
                openConnection();       //打开连接

                //填充数据集对象
                odda.Fill(dset,table);               
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                closeConnection();      //关闭连接
            }

            return dset;
        }
      
        //执行一般SQL语句
        public int SQLCommand(string sqlCommand)
        {
            int reValue = -1;
            if (sqlCommand != null)
            {
                try
                {
                    openConnection();
                    OleDbCommand oldeCmd = new OleDbCommand(sqlCommand, conn);
                    reValue = oldeCmd.ExecuteNonQuery();
                }
                catch
                {
                    reValue = -1;
                }
                finally
                {
                    closeConnection();
                }
                
            }
            return reValue;
        }
	}
}
