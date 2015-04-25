using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data;
using System.Data.OleDb;

namespace DatabaseControlLib
{
    public class SysUserAdapter : CommonAdapter
    {
        public SysUserAdapter(DBase db)
            : base(db,"SysUser")
        {

        }
        public override void InitAdapter()
        {
            base.InitAdapter();

            OleDbConnection conn = db.getConnection();

            //创建InsertCommand
            OleDbCommand InsertCommand;
            InsertCommand = new OleDbCommand(
                "INSERT INTO SysUser(UID,Username,Password,Canlogin) " +
                "VALUES(?,?,?,?);", conn);
            InsertCommand.Parameters.Add(
                "@UID", OleDbType.Integer, 0, "UID");
            InsertCommand.Parameters.Add(
                "@Username", OleDbType.VarWChar, 256, "Username");
            InsertCommand.Parameters.Add(
                "@Password", OleDbType.VarWChar, 255, "Password");
            InsertCommand.Parameters.Add(
                "@Canlogin", OleDbType.Boolean, 0, "Canlogin");
            odda.InsertCommand = InsertCommand;

            //创建DeleteCommand
            OleDbCommand DeleteCommand;
            DeleteCommand = new OleDbCommand(
                "DELETE FROM SysUser WHERE UID = ?", conn);
            DeleteCommand.Parameters.Add(
                "@UID", OleDbType.Integer, 0, "UID");
            odda.DeleteCommand = DeleteCommand;
        }

        public override DataSet CreateEmptyDataset()
        {
            DataSet dataSet = new DataSet();
            DataTable dataTable = new DataTable(tableName);

            dataTable.Columns.Add("UID", Type.GetType("System.Int32"));
            dataTable.Columns.Add("Username", Type.GetType("System.String"));
            dataTable.Columns.Add("Password", Type.GetType("System.String"));
            dataTable.Columns.Add("Canlogin", Type.GetType("System.Boolean"));
            dataSet.Tables.Add(dataTable);
            return dataSet;
        }

        //获取所有用户信息
        public DataSet GetAllUser()
        {
            return SelectData();
        }

        //添加一个系统用户
        public void AddOneSysUser(SysUser user)
        {
            DataSet dataSet = CreateEmptyDataset();

            try
            {
                db.openConnection();
                dataSet.Tables[tableName].Rows.Add(
                    new object[] { user.UID, user.Username, user.Password, user.Canlogin });
                odda.Update(dataSet, tableName);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                db.closeConnection();
            }
        }

        //获取指定用户的用户信息
        public DataSet GetUserInfo(string username)
        {
            DataSet dataSet = null;

            dataSet = SelectData("*", string.Format(
                "Username = '{0}'", username));

            return dataSet;
        }

        //获取用户的UID
        public int GetUID(string username)
        {
            int uid = -1;
            DataSet dataSet = null;

            dataSet = GetUserInfo(username);
            
            if (dataSet.Tables[tableName].Rows.Count != 0)
            {
                DataRow dataRow = dataSet.Tables[tableName].Rows[0];
                object value = dataRow["UID"];
                if (!DBNull.Value.Equals(value))
                {
                    uid = (int)value;
                }           
            }

            return uid;
        }

        // 判断该用户是否存在
        public bool IFHasUser(string username)
        {
            bool reValue = false;
            DataSet dataSet = null;

            dataSet = GetUserInfo(username);
           
            if (dataSet.Tables[tableName].Rows.Count != 0)
                reValue = true;

            return reValue;
        }

        // 判断用户密码是否正确
        public bool IFPasswordCorrect(string username, string password)
        {
            bool reValue = false;
            DataSet dataSet = null;

            dataSet = GetUserInfo(username);

            if (dataSet.Tables[tableName].Rows.Count != 0)
            {
                DataRow dataRow = dataSet.Tables[tableName].Rows[0];
                object value = dataRow["Password"];
                if (!DBNull.Value.Equals(value))
                {
                    if (value.ToString() == password)
                        reValue = true;
                }
            }
            return reValue;
        }

        //判断用户是否能够登录系统
        public bool IFCanlogin(string username)
        {
            bool reValue = false;
            DataSet dataSet = null;

            dataSet = GetUserInfo(username);

            if (dataSet != null)
            {
                if (dataSet.Tables[tableName].Rows.Count != 0)
                {
                    DataRow dataRow = dataSet.Tables[tableName].Rows[0];
                    object value = dataRow["Canlogin"];
                    if (!DBNull.Value.Equals(value))
                    {
                        if ((bool)value)
                            reValue = true;
                    }
                }
            }
            return reValue;
        }

        //获取在1-500已经使用的UID的最大值
        public int GetMaxUIDOfAdmin()
        {
            int uid = 0;
            DataSet dataSet = null;

            dataSet = SelectData("MAX(UID) as MaxUID", "UID<=500 AND UID>=1");
            if(dataSet != null &&
                dataSet.Tables[tableName].Rows.Count == 1)
            {
                DataRow dataRow = dataSet.Tables[tableName].Rows[0];
                object value = dataRow["MaxUID"];
                if (!DBNull.Value.Equals(value))
                    uid = (int)value;
            }
            return uid;
        }

        //获取501-无穷已经使用的UID的最大值
        public int GetMaxUIDOfUser()
        {
            int uid = 500;
            DataSet dataSet = null;

            dataSet = SelectData("MAX(UID) as MaxUID", "UID>500");
            int count = dataSet.Tables[tableName].Rows.Count;
            if (dataSet != null &&
                dataSet.Tables[tableName].Rows.Count == 1)
            {
                DataRow dataRow = dataSet.Tables[tableName].Rows[0];
                object value = dataRow["MaxUID"];
                if (!DBNull.Value.Equals(value))
                    uid = (int)value;
            }
            return uid;
        }
    }
}
