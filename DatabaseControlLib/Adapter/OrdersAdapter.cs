using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data;
using System.Data.OleDb;

namespace DatabaseControlLib
{
    public class OrdersAdapter : CommonAdapter
    {
        public OrdersAdapter(DBase db)
            : base(db, "Orders")
        {
            
        }

        public override DataSet CreateEmptyDataset()
        {
            DataSet dataSet = new DataSet();
            DataTable dataTable = new DataTable(tableName);

            dataTable.Columns.Add("Ono", Type.GetType("System.String"));
            dataTable.Columns.Add("Cno", Type.GetType("System.String"));
            dataTable.Columns.Add("Odate", Type.GetType("System.DateTime"));
            dataTable.Columns.Add("Osum", Type.GetType("System.Double"));
            dataTable.Columns.Add("Omoney", Type.GetType("System.Double"));
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
                "INSERT INTO " + tableName + "(Ono,Cno,Odate,Osum,Omoney) " +
                "VALUES(?,?,?,?,?);", conn);
            InsertCommand.Parameters.Add(
                "@Ono", OleDbType.VarChar, 50, "Ono");
            InsertCommand.Parameters.Add(
                "@Cno", OleDbType.VarChar, 50, "Cno");
            InsertCommand.Parameters.Add(
                "@Odate", OleDbType.Date, 0, "Odate");
            InsertCommand.Parameters.Add(
                "@Osum", OleDbType.Double, 0, "Osum");
            InsertCommand.Parameters.Add(
                "@Omoney", OleDbType.Double, 0, "Omoney");
            odda.InsertCommand = InsertCommand;

            //创建DeleteCommand
            OleDbCommand DeleteCommand;
            DeleteCommand = new OleDbCommand(
                "DELETE FROM " + tableName + " WHERE Ono=?", conn);
            DeleteCommand.Parameters.Add(
                "@Ono", OleDbType.VarChar, 50, "Ono");
            odda.DeleteCommand = DeleteCommand;
        }

        //获取所有订单
        public DataSet GetAllOrder()
        {
            return SelectData();
        }

        //添加一个订单
        public void AddOneOrder(Orders order)
        {            
            try
            {
                DataSet dataSet = CreateEmptyDataset();

                db.openConnection();
                dataSet.Tables[tableName].Rows.Add(
                    new object[] { order.Ono, order.Cno, order.Odate, order.Osum, order.Omoney });
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
