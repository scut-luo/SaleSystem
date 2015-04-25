using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

using DatabaseControlLib;
using System.Data;

namespace ManagementSystem
{
    /// <summary>
    /// Interaction logic for StorageManage.xaml
    /// </summary>
    public partial class StorageManage : Window
    {
        //数据接口
        private GoodsAdapter goodsAdapter = new GoodsAdapter(
            SQLConnection.GetDataBase());
        //数据表
        private DataSet GoodsList = new DataSet();
        private string TableName = "StorageList";
        public StorageManage()
        {
            InitializeComponent();
            Init();
        }

        //初始化操作
        private void Init()
        {
            try
            {
                GoodsList = goodsAdapter.GetAllGoods();     //获取所有商品信息
                GoodsList.Tables["Goods"].TableName = TableName;    //修改表名
                GoodsList.Tables[TableName].Columns.Add("LmGstore", Type.GetType("System.Double"));
                GoodsList.Tables[TableName].Columns.Add("PGnum", Type.GetType("System.Double"));
                GoodsList.Tables[TableName].Columns.Add("SGnum", Type.GetType("System.Double"));
                GoodsList.Tables[TableName].Columns.Add("Gstore", Type.GetType("System.Double"));
            }
            catch (Exception ex)
            {
                MessageBox.Show("异常发生,请检查数据库连接", "错误", MessageBoxButton.OK,
                    MessageBoxImage.Error);
                //写入日志
            }
        }

        private void Search_Click(object sender, RoutedEventArgs e)
        {
            //获取选择的时间
            DateTime selectedDate = dpDate.DisplayDate;
            DateTime preDate = selectedDate.AddMonths(-1);
            int Year = selectedDate.Year;
            int Month = selectedDate.Month;

            //遍历每个商品信息  
            string GoodsNum;
            string Query;   //查询语句
            DBase db = SQLConnection.GetDataBase();
            DataSet dataSet;
            foreach (DataRow oneRow in GoodsList.Tables[TableName].Rows)
            {
                double PGnum = 0, SGnum = 0, LmGstore = 0,Gstore = 0;
                GoodsNum = oneRow["Gno"].ToString();       //获取每个商品编号

                //获取该商品在选择时间的进货总量
                Query = string.Format("SELECT SUM(PDnum) as Num FROM PurchaseDetail WHERE Gno='{0}' AND Pno IN" +
                                            "(SELECT Pno FROM Purchase WHERE DATENAME(Year,Pdate)={1} AND DATENAME(Month,Pdate)={2})",
                                       GoodsNum, Year, Month);
                dataSet = db.openQueryDS(Query, "PGnum");

                if (dataSet.Tables["PGnum"].Rows.Count != 0)        //查询记录不为零
                {
                    if(!DBNull.Value.Equals(dataSet.Tables["PGnum"].Rows[0]["Num"]))
                        PGnum = Convert.ToDouble(dataSet.Tables["PGnum"].Rows[0]["Num"].ToString());
                }

                //获取该商品在选择时间的订单总量
                Query = string.Format("SELECT SUM(Gnum) as Num FROM OrderDetail WHERE Gno='{0}' AND Ono IN" +
                                            "(SELECT Ono FROM Orders WHERE DATENAME(Year,Odate)={1} AND DATENAME(Month,Odate)={2})",
                                       GoodsNum, Year, Month);
                dataSet = db.openQueryDS(Query, "SGnum");
                if (dataSet.Tables["SGnum"].Rows.Count != 0)        //查询记录不为零
                {
                    if (!DBNull.Value.Equals(dataSet.Tables["SGnum"].Rows[0]["Num"]))
                        PGnum = Convert.ToDouble(dataSet.Tables["SGnum"].Rows[0]["Num"].ToString());
                }

                //获取当月的商品结余数量
                Query = string.Format("SELECT Gstore as Num FROM Storage WHERE Gno='{0}' AND " +
                                            "DATENAME(Year,Gdate)={1} AND DATENAME(Month,Gdate)={2}", GoodsNum, Year, Month);
                dataSet = db.openQueryDS(Query, "Gstore");
                if (dataSet.Tables["Gstore"].Rows.Count != 0)
                {
                    if (!DBNull.Value.Equals(dataSet.Tables["Gstore"].Rows[0]["Num"]))
                        Gstore = Convert.ToDouble(dataSet.Tables["Gstore"].Rows[0]["Num"].ToString());
                }

                //获取前一个月的结余商品数量
                Query = string.Format("SELECT Gstore as Num FROM Storage WHERE Gno='{0}' AND " +
                                            "DATENAME(Year,Gdate)={1} AND DATENAME(Month,Gdate)={2}", GoodsNum, preDate.Year, preDate.Month);
                dataSet = db.openQueryDS(Query, "LmGstore");
                if (dataSet.Tables["LmGstore"].Rows.Count != 0)
                {
                    if (!DBNull.Value.Equals(dataSet.Tables["LmGstore"].Rows[0]["Num"]))
                        LmGstore = Convert.ToDouble(dataSet.Tables["LmGstore"].Rows[0]["Num"].ToString());
                }

                oneRow["PGnum"] = PGnum;
                oneRow["SGnum"] = SGnum;               
                oneRow["LmGstore"] = LmGstore;
                oneRow["Gstore"] = Gstore;
            }
            LV_Storage.ItemsSource = GoodsList.Tables[TableName].DefaultView;
        }
    }
}
