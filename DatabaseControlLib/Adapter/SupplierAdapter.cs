using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Collections.ObjectModel;
using System.Data;
using System.Data.OleDb;

namespace DatabaseControlLib
{
    public class SupplierAdapter : CommonAdapter
    {
        public SupplierAdapter(DBase db)
            : base(db,"Supplier")
        {

        }

        public override void InitAdapter()
        {
            OleDbConnection conn = db.getConnection();

            //创建InsertCommand
            OleDbCommand InsertCommand;
            InsertCommand = new OleDbCommand(
                "INSERT INTO Supplier(Sno,Sname,Saddr,Sphone) " +
                "VALUES(?,?,?,?);", conn);
            InsertCommand.Parameters.Add(
                "@Sno", OleDbType.VarChar,50, "Sno");
            InsertCommand.Parameters.Add(
                "@Sname", OleDbType.VarWChar,256, "Sname");
            InsertCommand.Parameters.Add(
                "@Saddr", OleDbType.VarWChar,500,"Saddr");
            InsertCommand.Parameters.Add(
                "@Sphone", OleDbType.VarChar,50, "Sphone");
            odda.InsertCommand = InsertCommand;

            //创建UpdateCommand
            OleDbCommand UpdateCommand;
            UpdateCommand = new OleDbCommand(
                "UPDATE Supplier SET Sname=?,Saddr=?,Sphone=? WHERE (Sno=?);",conn);
            UpdateCommand.Parameters.Add(
                "@Sname", OleDbType.VarWChar, 256, "Sname");
            UpdateCommand.Parameters.Add(
                "@Saddr", OleDbType.VarWChar, 500, "Saddr");
            UpdateCommand.Parameters.Add(
                "@Sphone", OleDbType.VarChar, 50, "Sphone");
            UpdateCommand.Parameters.Add(
                "@Sno", OleDbType.VarChar, 50, "Sno");
            odda.UpdateCommand = UpdateCommand;
        }

        public override DataSet CreateEmptyDataset()
        {
            DataSet dataSet = new DataSet();
            DataTable dataTable = new DataTable(tableName);

            dataTable.Columns.Add("Sno", Type.GetType("System.String"));
            dataTable.Columns.Add("Sname", Type.GetType("System.String"));
            dataTable.Columns.Add("Saddr", Type.GetType("System.String"));
            dataTable.Columns.Add("Sphone", Type.GetType("System.String"));
            dataSet.Tables.Add(dataTable);

            return dataSet;
        }

        public DataSet GetSupplierBySupplierNum(string SupplierNum)
        {
            return SelectData("*",
                string.Format("Sno = '{0}'", SupplierNum));
        }

        public DataSet GetAllSupplier()
        {
            DataSet dataSet = null;

            dataSet = SelectData();     //获取全部供应商信息

            return dataSet;
        }

        public ICollection<Supplier> GetAllSupplierToArray()
        {
            DataSet dataset = null;
            ObservableCollection<Supplier> suppliers = new ObservableCollection<Supplier>();

            dataset = SelectData();
            foreach (DataRow dataRow in dataset.Tables[tableName].Rows)
            {
                suppliers.Add(new Supplier((string)dataRow["Sno"],
                    (string)dataRow["Sname"],(string)dataRow["Saddr"],(string)dataRow["Sphone"]));
            }
            return suppliers;
        }

        //添加一个新的供应商
        public void AddOneSupplier(Supplier supplier)
        {
            DataSet dataSet = CreateEmptyDataset();

            try
            {
                db.openConnection();
                dataSet.Tables[tableName].Rows.Add(
                    new object[] { supplier.Sno, supplier.Sname, supplier.Saddr, supplier.Sphone });
                odda.Update(dataSet, tableName);
            }
            catch (Exception ex)
            {
                //抛出异常，上层处理
                throw new Exception(ex.Message);
            }
            finally
            {
                db.closeConnection();
            }        
        }

        //更改一个供应商信息
        public void UpdateOneSupplier(Supplier supplier)
        {
            DataSet dataSet = CreateEmptyDataset();

            try
            {
                db.openConnection();
                dataSet.Tables[tableName].Rows.Add(
                    new object[] { supplier.Sno, supplier.Sname, supplier.Saddr, supplier.Sphone });
                string Sno = dataSet.Tables[tableName].Rows[0]["Sno"].ToString();
                dataSet.Tables[tableName].AcceptChanges();
                dataSet.Tables[tableName].Rows[0].SetModified();
                odda.Update(dataSet,tableName);
            }
            catch (Exception ex)
            {
                //抛出异常，上层处理
                throw new Exception(ex.Message);
            }
            finally
            {
                db.closeConnection();
            }
        }
    }
}
