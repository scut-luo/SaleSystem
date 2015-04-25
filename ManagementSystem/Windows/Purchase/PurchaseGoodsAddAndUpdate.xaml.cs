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
    /// PurchaseGoodsAddAndUpdate.xaml 的交互逻辑
    /// </summary>
    public partial class PurchaseGoodsAddAndUpdate : Window
    {
        //数据接口
        private SupplierAdapter supplierAdapter = new SupplierAdapter(
            SQLConnection.GetDataBase());
        private GoodsAdapter goodsAdapter = new GoodsAdapter(
            SQLConnection.GetDataBase());
        private PurchaseAdapter padapter = new PurchaseAdapter(
            SQLConnection.GetDataBase());
        private PurchaseDetailAdapter pdadapter = new PurchaseDetailAdapter(
                SQLConnection.GetDataBase());
        //数据表        
        private Purchase PurchaseInfo = new Purchase();     //进货信息
        private DataSet PurchaseDetailList = new DataSet(); //进货明细列表
        //其他
        private bool bAddPurchase = true; //添加进货信息

        public PurchaseGoodsAddAndUpdate()
        {
            InitializeComponent();

            //初始化窗口
            Init();
        }

        public PurchaseGoodsAddAndUpdate(bool baddPurchase, Purchase purchaseInfo)
        {
            InitializeComponent();

            bAddPurchase = baddPurchase;
            PurchaseInfo = purchaseInfo;

            //初始化窗口
            Init();
        }

        private void Init()
        {
            try
            {                
                //设定绑定源(进货信息)
                Grid_Purchase.DataContext = PurchaseInfo;
                //设定绑定源(进货明细列表)
                if (bAddPurchase)        //添加进货
                {
                    //设置进货日期为当前日期
                    dp_PDate.SelectedDate = DateTime.Now;
                    PurchaseDetailList = pdadapter.CreateEmptyDataset();
                    PurchaseDetailList.Tables["PurchaseDetail"].Columns.Add("Gname", Type.GetType("System.String"));      //添加商品名称列                
                }
                else
                {
                    dp_PDate.SelectedDate = PurchaseInfo.PDate;

                    DataSet supplierInfo = supplierAdapter.GetSupplierBySupplierNum(PurchaseInfo.Sno);
                    tb_Pno.IsReadOnly = true;
                    tb_Supplier.Text = supplierInfo.Tables["Supplier"].Rows[0]["Sname"].ToString();

                    string PDname = "PurchaseDeatail_T";
                    //获取所有商品信息
                    DataSet GoodsList = goodsAdapter.GetAllGoods();
                    DataTable GoodsTable = GoodsList.Tables["Goods"];

                    //获取指定进货批次的进货明细
                    PurchaseDetailList = pdadapter.GetPurchaseDetailByPurchaseNum(PurchaseInfo.Pno);
                    PurchaseDetailList.Tables["PurchaseDetail"].TableName = PDname;     //修改表名

                    DataSetHelper pdhelper = new DataSetHelper(ref PurchaseDetailList);                                       

                    GoodsList.Tables.Remove("Goods");
                    PurchaseDetailList.Tables.Add(GoodsTable);  //添加子表
                    PurchaseDetailList.Relations.Add("PDAndG", PurchaseDetailList.Tables["Goods"].Columns["Gno"],
                                                        PurchaseDetailList.Tables[PDname].Columns["Gno"]);

                    pdhelper.SelectJoinInto("PurchaseDetail", PurchaseDetailList.Tables[PDname],
                                            "Pno,Gno,PDnum,PDprice,PDAndG.Gname", "1=1", "Gno");
                }
     
                LV_PurchaseDetail.ItemsSource = PurchaseDetailList.Tables["PurchaseDetail"].DefaultView;
            }
            catch (Exception ex)
            {
                MessageBox.Show("异常发生,请检查数据库连接", "错误", MessageBoxButton.OK,
                    MessageBoxImage.Error);                
                //写入日志

                this.Close();
            }            
        }

        //列出供应商列表
        private void ListSupplier_Click(object sender, RoutedEventArgs e)
        {
            SupplierList suppliers = new SupplierList();
            suppliers.Owner = this;
            suppliers.ShowDialog();
        }

        //列出商品列表
        private void ListGoods_Click(object sender, RoutedEventArgs e)
        {
            object test = tb_Supplier.Tag;
            GoodsList goods = new GoodsList();
            goods.Owner = this;
            goods.ShowDialog();
        }

        //添加新的进货明细
        private void AddOnePurchaseDetail_Click(object sender, RoutedEventArgs e)
        {
            if(tb_Pno.Text == "" || 
                tb_Supplier.Text == "")
            {
                MessageBox.Show("进货批次信息不完整", "错误", MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return;
            }

            if(tb_GoodsName.Text == "" ||
                tb_PDnum.Text == "" ||
                tb_PDprice.Text == "")
            {
                MessageBox.Show("进货明细信息不完整", "错误", MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return;
            }

            PurchaseDetailList.Tables["PurchaseDetail"].Rows.Add(
                new object[] {tb_Pno.Text,
                              tb_GoodsName.Tag,
                              Convert.ToDouble(tb_PDnum.Text),
                              Convert.ToDouble(tb_PDprice.Text),
                              tb_GoodsName.Text});
        }

        //保存所有进货信息
        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (tb_Pno.Text == "" ||
                tb_Supplier.Text == "")
            {
                MessageBox.Show("进货批次信息不完整", "错误", MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return;
            }

            try
            {
                if (bAddPurchase)
                {
                    //添加进货批次信息
                    padapter.AddOnePurchase(PurchaseInfo);
                }           
                //更新进货明细
                if(PurchaseDetailList.HasChanges())
                    pdadapter.UpdateData(PurchaseDetailList);

                MessageBox.Show("更新进货信息成功", "提醒", MessageBoxButton.OK,
                    MessageBoxImage.Information);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("异常发生,请检查数据库连接或数据是否符合标准", "提醒", MessageBoxButton.OK,
                    MessageBoxImage.Information);
                //写入日志
            }
        }

        //删除一个进货细则
        private void DeleteOnePurchaseDetail(object sender, RoutedEventArgs e)
        {
            object item = LV_PurchaseDetail.SelectedItem;
            if (item == null)
            {
                MessageBox.Show("请选择要删除的进货细则", "错误", MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return;
            }

            PurchaseDetailList.Tables["PurchaseDetail"].Rows[LV_PurchaseDetail.SelectedIndex].Delete();
        }
    }
}
