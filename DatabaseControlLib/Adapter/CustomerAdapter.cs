using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data;
using System.Data.OleDb;

namespace DatabaseControlLib
{
    public class CustomerAdapter : CommonAdapter
    {
        public CustomerAdapter(DBase db)
            : base(db, "Customer")
        {

        }

        public override DataSet CreateEmptyDataset()
        {
            DataSet dataSet = new DataSet();
            DataTable dataTable = new DataTable(tableName);

            dataTable.Columns.Add("Cno", Type.GetType("System.String"));
            dataTable.Columns.Add("Cname", Type.GetType("System.String"));
            dataTable.Columns.Add("Caddr", Type.GetType("System.String"));
            dataTable.Columns.Add("Cphone", Type.GetType("System.String"));

            dataSet.Tables.Add(dataTable);
            return dataSet;
        }

        public override void InitAdapter()
        {
            base.InitAdapter();

            OleDbConnection conn = db.getConnection();
            
            //创建InsertCommand
            OleDbCommand InsertCommand;
            InsertCommand = new OleDbCommand(
                "INSERT INTO Customer(Cno,Cname,Caddr,Cphone) " +
                "VALUES(?,?,?,?);", conn);
            InsertCommand.Parameters.Add(
                "@Cno", OleDbType.VarChar, 50, "Cno");
            InsertCommand.Parameters.Add(
                "@Cname", OleDbType.VarWChar, 256, "Cname");
            InsertCommand.Parameters.Add(
                "@Caddr", OleDbType.VarWChar, 500, "Caddr");
            InsertCommand.Parameters.Add(
                "@Cphone", OleDbType.VarChar, 50, "Cphone");
            odda.InsertCommand = InsertCommand;
        }

        //获取所有顾客信息
        public DataSet GetAllCustomer()
        {
            return SelectData();
        }

        //添加一个新的顾客信息
        public void AddOneCustomer(Customer customer)
        {
            DataSet dataSet = CreateEmptyDataset();

            try
            {
                db.openConnection();
                dataSet.Tables[tableName].Rows.Add(
                    new object[] { customer.Cno, customer.Cname, customer.Caddr, customer.Cphone });
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
