using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data;
using System.Data.OleDb;

namespace DatabaseControlLib
{
    public class PurchaseAdapter : CommonAdapter
    {
        public PurchaseAdapter(DBase db)
            : base(db, "Purchase")
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
                "INSERT INTO Purchase(Pno,Sno,Pdate) " +
                "VALUES(?,?,?);", conn);
            InsertCommand.Parameters.Add(
                "@Pno", OleDbType.VarChar, 50, "Pno");
            InsertCommand.Parameters.Add(
                "@Sno", OleDbType.VarChar, 50, "Sno");
            InsertCommand.Parameters.Add(
                "@Pdate", OleDbType.Date, 0, "Pdate");
            odda.InsertCommand = InsertCommand;

            //创建UpdateCommand
            OleDbCommand UpdateCommand;
            UpdateCommand = new OleDbCommand(
                "UPDATE " + tableName + " SET Sno=?,Pdate=? WHERE Pno=?", conn);
            UpdateCommand.Parameters.Add(
                "@Sno", OleDbType.VarChar, 50, "Sno");
            UpdateCommand.Parameters.Add(
                "@Pno", OleDbType.VarChar, 50, "Pno");
            UpdateCommand.Parameters.Add(
                "@Pdate", OleDbType.Date, 0, "Pdate");
            odda.UpdateCommand = UpdateCommand;
        }

        public override DataSet CreateEmptyDataset()
        {
            DataSet dataSet = new DataSet();
            DataTable dataTable = new DataTable(tableName);
            
            dataTable.Columns.Add("Pno", Type.GetType("System.String"));
            dataTable.Columns.Add("Sno", Type.GetType("System.String"));
            dataTable.Columns.Add("Pdate", Type.GetType("System.DateTime"));
            dataSet.Tables.Add(dataTable);

            return dataSet;
        }

        public DataSet GetAllPurchase()
        {
            return GetAllData();
        }

        //添加一个新的进货批次信息
        public void AddOnePurchase(Purchase purchase)
        {
            DataSet dataSet = CreateEmptyDataset();

            try
            {
                db.openConnection();
                dataSet.Tables[tableName].Rows.Add(
                    new object[] { purchase.Pno, purchase.Sno, purchase.PDate });
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
    }
}
