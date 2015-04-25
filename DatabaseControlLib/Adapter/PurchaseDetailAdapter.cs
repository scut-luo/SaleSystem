using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data;
using System.Data.OleDb;

namespace DatabaseControlLib
{
    public class PurchaseDetailAdapter : CommonAdapter
    {
        public PurchaseDetailAdapter(DBase db)
            : base(db, "PurchaseDetail")
        {

        }

        public override void InitAdapter()
        {
            base.InitAdapter();

            //初始化接口
            OleDbConnection conn = db.getConnection();

            //创建InsertCommand
            OleDbCommand InsertCommand;
            InsertCommand = new OleDbCommand(
                "INSERT INTO " +  tableName + "(Pno,Gno,PDnum,PDprice) " +
                "VALUES(?,?,?,?);", conn);
            InsertCommand.Parameters.Add(
                "@Pno", OleDbType.VarChar, 50, "Pno");
            InsertCommand.Parameters.Add(
                "@Gno", OleDbType.VarChar, 50, "Gno");
            InsertCommand.Parameters.Add(
                "@PDnum", OleDbType.Double, 0, "PDnum");
            InsertCommand.Parameters.Add(
                "@PDprice", OleDbType.Double, 0, "PDprice");
            odda.InsertCommand = InsertCommand;

            //创建UpdateCommand
            OleDbCommand UpdateCommand;
            UpdateCommand = new OleDbCommand(
                "UPDATE " + tableName + " SET PDnum=?,PDprice=? " +
                "WHERE (Pno=?,Gno=?)", conn);
            UpdateCommand.Parameters.Add(
                "@PDnum", OleDbType.Double, 0, "PDnum");
            UpdateCommand.Parameters.Add(
                "@PDprice", OleDbType.Double, 0, "PDprice");
            UpdateCommand.Parameters.Add(
                "@Pno", OleDbType.VarChar, 50, "Pno");
            UpdateCommand.Parameters.Add(
                "@Gno", OleDbType.VarChar, 50, "Gno");
            odda.UpdateCommand = UpdateCommand;
            
            //创建DeleteCommand
            OleDbCommand DeleteCommand;
            DeleteCommand = new OleDbCommand(
                "DELETE FROM " + tableName + " WHERE (Pno=? AND Gno=?);", conn);
            DeleteCommand.Parameters.Add(
                "@Pno", OleDbType.VarChar, 50, "Pno");
            DeleteCommand.Parameters.Add(
                "@Gno", OleDbType.VarChar, 50, "Gno");
            odda.DeleteCommand = DeleteCommand;
        }

        public override DataSet CreateEmptyDataset()
        {
            DataSet dataSet = new DataSet();
            DataTable dataTable = new DataTable(tableName);
            
            dataTable.Columns.Add("Pno", Type.GetType("System.String"));
            dataTable.Columns.Add("Gno", Type.GetType("System.String"));
            dataTable.Columns.Add("PDnum", Type.GetType("System.Double"));
            dataTable.Columns.Add("PDprice", Type.GetType("System.Double"));
            dataSet.Tables.Add(dataTable);

            return dataSet;
        }

        public DataSet GetAllPurchaseDetail()
        {
            return GetAllData();
        }

        public DataSet GetPurchaseDetailByPurchaseNum(string PurchaseNum)
        {
            return SelectData("*", string.Format("Pno = '{0}'", PurchaseNum));
        }
    }
}
