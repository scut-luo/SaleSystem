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

        //ִ�в�ѯ
        public DataTable openQueryDT(string query,string table = "Table")
        {
            DataTable dtable = null;
            OleDbDataAdapter odda = new OleDbDataAdapter();
            DataSet dset = new DataSet();       //�������ݼ�����

            try
            {
                //�����������
                odda.SelectCommand = new OleDbCommand(query, conn);
                openConnection();       //������

                //������ݼ�����
                odda.Fill(dset,table);

                //ȡ�ñ�
                dtable = dset.Tables[0];               
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                closeConnection();      //�ر�����
            }

            return dtable;
        }

        //ִ�в�ѯ
        public DataSet openQueryDS(string query,string table = "Table")
        {     
            OleDbDataAdapter odda = new OleDbDataAdapter();
            DataSet dset = new DataSet();       //�������ݼ�����

            try
            {
                //�����������
                odda.SelectCommand = new OleDbCommand(query, conn);
                openConnection();       //������

                //������ݼ�����
                odda.Fill(dset,table);               
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                closeConnection();      //�ر�����
            }

            return dset;
        }
      
        //ִ��һ��SQL���
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
