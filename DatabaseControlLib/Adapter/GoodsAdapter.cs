using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data;
using System.Data.OleDb;

namespace DatabaseControlLib
{
    public class GoodsAdapter : CommonAdapter
    {
        //构造函数 
        public GoodsAdapter(DBase db)
            : base(db, "Goods")
        {

        }

        public override void InitAdapter()
        {
            OleDbConnection conn = db.getConnection();

            //创建InsertCommand
            OleDbCommand InsertCommand;
            InsertCommand = new OleDbCommand(
                "INSERT INTO Goods(Gno,Gname,Gmanufacturer) " +
                "VALUES(?,?,?);", conn);
            InsertCommand.Parameters.Add(
                "@Gno", OleDbType.VarChar, 50, "Gno");
            InsertCommand.Parameters.Add(
                "@Gname", OleDbType.VarWChar, 256, "Gname");
            InsertCommand.Parameters.Add(
                "@Gmanufacturer", OleDbType.VarWChar, 500, "Gmanufacturer");
            odda.InsertCommand = InsertCommand;
        }
        
        public override DataSet CreateEmptyDataset()
        {
            DataSet dataSet = new DataSet();
            DataTable dataTable = new DataTable(tableName);

            dataTable.Columns.Add("Gno", Type.GetType("System.String"));
            dataTable.Columns.Add("Gname", Type.GetType("System.String"));
            dataTable.Columns.Add("Gmanufacturer", Type.GetType("System.String"));
            dataSet.Tables.Add(dataTable);

            return dataSet;
        }

        public DataSet GetAllGoods()
        {
            DataSet dataSet = null;

            dataSet = SelectData();         //获取全部商品信息

            return dataSet;
        }

        public void AddOneGoods(Goods goods)
        {
            DataSet dataSet = CreateEmptyDataset();

            try
            {
                db.openConnection();
                dataSet.Tables[tableName].Rows.Add(
                    new object[] { goods.Gno, goods.Gname, goods.Gmanufacturer });
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
    }
}
