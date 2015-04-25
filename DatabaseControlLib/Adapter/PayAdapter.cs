using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data;
using System.Data.OleDb;

namespace DatabaseControlLib
{
    public class PayAdapter : CommonAdapter
    {
        public PayAdapter(DBase db)
            : base(db, "Pay")
        {
            
        }

        public override DataSet CreateEmptyDataset()
        {
            DataSet dataSet = new DataSet();
            DataTable dataTable = new DataTable(tableName);

            dataTable.Columns.Add("Cno", Type.GetType("System.String"));
            dataTable.Columns.Add("Ono", Type.GetType("System.String"));
            dataTable.Columns.Add("Pdate", Type.GetType("System.DateTime"));
            dataTable.Columns.Add("Pmoney", Type.GetType("System.DateTime"));
            dataSet.Tables.Add(dataTable);

            return dataSet;
        }

        public override void InitAdapter()
        {
            base.InitAdapter();

            //初始化接口
            OleDbConnection conn = db.getConnection();

            //创建UpdateCommand
            OleDbCommand UpdateCommand;
            UpdateCommand = new OleDbCommand(
                "UPDATE " + tableName + " SET Cno=?,Pdate=?,Pmoney=? WHERE Ono=?", conn);
            UpdateCommand.Parameters.Add(
                "@Cno", OleDbType.VarChar, 50, "Cno");
            UpdateCommand.Parameters.Add(
                "@Pdate", OleDbType.Date, 0, "Pdate");
            UpdateCommand.Parameters.Add(
                "@Pmoney", OleDbType.Double, 0, "Pmoney");
            UpdateCommand.Parameters.Add(
                "@Ono", OleDbType.VarChar, 50, "Ono");
            odda.UpdateCommand = UpdateCommand;
        }

        //获取所有支付信息
        public DataSet GetAllPay()
        {
            return SelectData();
        }
    }
}
