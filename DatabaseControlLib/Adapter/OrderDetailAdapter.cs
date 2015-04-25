using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data;
using System.Data.OleDb;

namespace DatabaseControlLib
{
    public class OrderDetailAdapter : CommonAdapter
    {
        //构造函数
        public OrderDetailAdapter(DBase db)
            : base(db, "OrderDetail")
        {

        }

        public override DataSet CreateEmptyDataset()
        {
            DataSet dataSet = new DataSet();
            DataTable dataTable = new DataTable(tableName);

            dataTable.Columns.Add("Ono", Type.GetType("System.String"));
            dataTable.Columns.Add("Gno", Type.GetType("System.String"));
            dataTable.Columns.Add("Gnum", Type.GetType("System.Double"));
            dataTable.Columns.Add("Gprice", Type.GetType("System.Double"));

            dataSet.Tables.Add(dataTable);
            return dataSet;
        }

        public override void InitAdapter()
        {
            base.InitAdapter();

            //初始化接口
            OleDbConnection conn = db.getConnection();

            //创建InsertCommand
            OleDbCommand InsertCommand;
            InsertCommand = new OleDbCommand(
                "INSERT INTO " + tableName + "(Ono,Gno,Gnum,Gprice) " +
                "VALUES(?,?,?,?);", conn);
            InsertCommand.Parameters.Add(
                "@Ono", OleDbType.VarChar, 50, "Ono");
            InsertCommand.Parameters.Add(
                "@Gno", OleDbType.VarChar, 50, "Gno");
            InsertCommand.Parameters.Add(
                "@Gnum", OleDbType.Double, 0, "Gnum");
            InsertCommand.Parameters.Add(
                "@Gprice", OleDbType.Double, 0, "Gprice");
            odda.InsertCommand = InsertCommand;

            //创建UpdateCommand
            OleDbCommand UpdateCommand;
            UpdateCommand = new OleDbCommand(
                "UPDATE " + tableName + " SET Gnum=?,Gprice=? WHERE Ono=? AND Gno=?", conn);
            UpdateCommand.Parameters.Add(
                "@Gnum", OleDbType.Double, 0, "Gnum");
            UpdateCommand.Parameters.Add(
                "@Gprice", OleDbType.Double, 0, "Gprice");
            UpdateCommand.Parameters.Add(
                "@Ono", OleDbType.VarChar, 50, "Ono");
            UpdateCommand.Parameters.Add(
                "@Gno", OleDbType.VarChar, 50, "Gno");
            odda.UpdateCommand = UpdateCommand;

            //创建DeleteCommand
            OleDbCommand DeleteCommand;
            DeleteCommand = new OleDbCommand(
                "DELETE FROM " + tableName + " WHERE Ono=? AND Gno=?", conn);
            DeleteCommand.Parameters.Add(
                "@Ono", OleDbType.VarChar, 50, "Ono");
            DeleteCommand.Parameters.Add(
                "@Gno", OleDbType.VarChar, 50, "Gno");
            odda.DeleteCommand = DeleteCommand;
        }

        //获取所有订单细节
        public DataSet GetAllOrderDetail()
        {
            return SelectData();
        }
    }
}
