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

using System.Data;
using System.Data.OleDb;
using DatabaseControlLib;

namespace ManagementSystem
{
    /// <summary>
    /// PurchaseGoodsManage.xaml 的交互逻辑
    /// </summary>
    public partial class PurchaseGoodsManage : Window
    {       
        private DataSet PurchaseList = new DataSet();
        private DataSet PurchaseDetailList = new DataSet();

        public PurchaseGoodsManage()
        {
            InitializeComponent();
            LoadData();
        }

        //加载数据
        private void LoadData()
        {
            try
            {
                DataSet SupplierList;
                SupplierAdapter sadapter = new SupplierAdapter(
                    SQLConnection.GetDataBase());
                PurchaseAdapter padapter = new PurchaseAdapter(
                    SQLConnection.GetDataBase());
                 
                SupplierList = sadapter.GetAllSupplier();           //获取所有供应商信息
                PurchaseList = padapter.GetAllPurchase();            //获取所有进货信息
                 
                DataTable table = SupplierList.Tables["Supplier"];
                DataSetHelper phelper = new DataSetHelper(ref PurchaseList);                

                SupplierList.Tables.Remove("Supplier");
                PurchaseList.Tables.Add(table);    //添加子表
                PurchaseList.Relations.Add("PAndS", PurchaseList.Tables["Supplier"].Columns["Sno"],
                                            PurchaseList.Tables["Purchase"].Columns["Sno"]);
                
                phelper.SelectJoinInto("PurchaseAndSupplier", PurchaseList.Tables["Purchase"],
                                        "Pno,Sno,PAndS.Sname,Pdate", "1=1", "Pno");

                //加载进货信息
                LV_PurchaseAndSupplier.ItemsSource = PurchaseList.Tables["PurchaseAndSupplier"].DefaultView;

                //加载进货细节
                if (LV_PurchaseAndSupplier.Items.Count != 0)
                {
                    //选择第一项
                    LV_PurchaseAndSupplier.SelectedIndex = 0;
                    LoadPurchaseDetailData(0);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("异常发生,请检查数据库连接", "错误", MessageBoxButton.OK,
                    MessageBoxImage.Error);
                //写入日志
            }            
        }

        //加载进货细节
        private void LoadPurchaseDetailData(int selectedIndex)
        {
            if (selectedIndex == -1)
                return;

            string purcharnum = (LV_PurchaseAndSupplier.Items[selectedIndex]
                                            as DataRowView)["Pno"].ToString();      //获取选择的进货单号

            DataSet GoodsList;
            DataTable GoodsTable;
            GoodsAdapter gadapter = new GoodsAdapter(
                SQLConnection.GetDataBase());
            PurchaseDetailAdapter pdadapter = new PurchaseDetailAdapter(
                SQLConnection.GetDataBase());

            PurchaseDetailList = pdadapter.GetAllPurchaseDetail();
            DataSetHelper pdhelper = new DataSetHelper(ref PurchaseDetailList);

            //获取所有商品信息
            GoodsList = gadapter.GetAllGoods();
            GoodsTable = GoodsList.Tables["Goods"];

            GoodsList.Tables.Remove("Goods");
            PurchaseDetailList.Tables.Add(GoodsTable);  //添加子表
            PurchaseDetailList.Relations.Add("PDAndG", PurchaseDetailList.Tables["Goods"].Columns["Gno"],
                                                PurchaseDetailList.Tables["PurchaseDetail"].Columns["Gno"]);

            pdhelper.SelectJoinInto("PurchaseDeatailAndGoods", PurchaseDetailList.Tables["PurchaseDetail"],
                                    "Gno,PDAndG.Gname,PDnum,PDprice",
                                    string.Format("Pno = '{0}'", purcharnum), "Gno");
            LV_PurchaseDetail.ItemsSource = PurchaseDetailList.Tables["PurchaseDeatailAndGoods"].DefaultView;
        }

        //添加新的进货信息
        private void AddNewPurchase(object sender, RoutedEventArgs e)
        {
            PurchaseGoodsAddAndUpdate pgau = new PurchaseGoodsAddAndUpdate();
            pgau.ShowDialog();
            LoadData();
        }

        //编辑选中的进货单
        private void EditPurchase_Click(object sender, RoutedEventArgs e)
        {
            object item = LV_PurchaseAndSupplier.SelectedItem;
            if(item == null)
            {
                MessageBox.Show("请选择进货批次","错误",MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return;
            }
            DataRowView oneRow = item as DataRowView;
            PurchaseGoodsAddAndUpdate pgau = 
                new PurchaseGoodsAddAndUpdate(false,new Purchase(oneRow["Pno"].ToString(),
                                                           oneRow["Sno"].ToString(),
                                                           Convert.ToDateTime(oneRow["Pdate"].ToString())));
            pgau.ShowDialog();
            LoadData();
        } 

        //选择不同的进货单
        private void PurchaseSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            object item = LV_PurchaseAndSupplier.SelectedItem;
            if (item == null)
                return;
            try
            {
                LoadPurchaseDetailData(LV_PurchaseAndSupplier.SelectedIndex);
            }
            catch (Exception ex)
            {
                MessageBox.Show("异常发生,请检查数据库连接", "错误", MessageBoxButton.OK,
                    MessageBoxImage.Error);
                //写入日志
            }
        }         
    }
}
