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
using DatabaseControlLib;

namespace ManagementSystem
{
    /// <summary>
    /// Interaction logic for OrderInfoUpdate.xaml
    /// </summary>
    public partial class OrderInfoUpdate : Window
    {
        //数据库接口
        private OrderDetailAdapter orderDetailAdapter = new OrderDetailAdapter(
            SQLConnection.GetDataBase());
        private OrdersAdapter ordersAdapter = new OrdersAdapter(
            SQLConnection.GetDataBase());
        //数据表
        private Orders OrderInfo = new Orders();
        private DataSet OrderDetailList = new DataSet();    //订单细则信息
        //其他
        private bool bAddOrder;     //是否添加新的订单信息

        public OrderInfoUpdate(bool bAddNew = false,Orders orderinfo = null)
        {
            InitializeComponent();

            bAddOrder = bAddNew;
            OrderInfo = orderinfo;

            //窗口初始化
            Init();
        }

        private void Init()
        {
            try
            {
                if (bAddOrder)      //要添加新的订单信息
                {
                    OrderInfo = new Orders();
                    OrderDetailList = orderDetailAdapter.CreateEmptyDataset();          //创建空表
                    OrderDetailList.Tables["OrderDetail"].Columns.Add("Gname", Type.GetType("System.String"));  //添加商品名称列

                    //设置当前时间
                    dp_Odate.SelectedDate = DateTime.Now;
                }
                else                //修改已有订单
                {
                    if (OrderInfo == null)   //没有订单信息
                    {
                        MessageBox.Show("未指定修改的订单信息", "错误", MessageBoxButton.OK,
                            MessageBoxImage.Error);
                        //写入日志
                        this.Close();
                    }

                    //偷懒写法
                    DBase db = SQLConnection.GetDataBase();
                    string Query = string.Format("SELECT Ono,First.Gno,Gnum,Gprice,Gname FROM OrderDetail First,Goods Second " +
                                                    "WHERE First.Gno=Second.Gno AND Ono='{0}'", OrderInfo.Ono);
                    OrderDetailList = db.openQueryDS(Query, "OrderDetail");
                    DataSet dataSet;                    
                    Query = string.Format("SELECT * FROM Customer WHERE Cno='{0}'", OrderInfo.Cno);
                    dataSet = db.openQueryDS(Query, "Customer");
                    tb_Cname.Text = dataSet.Tables["Customer"].Rows[0]["Cname"].ToString();

                    dp_Odate.SelectedDate = OrderInfo.Odate;
                }

                //数据绑定
                Grid_Order.DataContext = OrderInfo;
                LV_OrderDetail.ItemsSource = OrderDetailList.Tables["OrderDetail"].DefaultView;
            }
            catch(Exception ex)
            {
                MessageBox.Show("异常发生,请检查数据库连接", "错误", MessageBoxButton.OK,
                    MessageBoxImage.Error);                
                //写入日志
                this.Close();
            }            
        }

        //保存订单信息
        private void Save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (bAddOrder)       //添加新订单
                {
                    ordersAdapter.AddOneOrder(OrderInfo);
                }
                else                 //修改订单
                {

                }

                //订单明细
                if(OrderDetailList.HasChanges())
                    orderDetailAdapter.UpdateData(OrderDetailList);

                MessageBox.Show("保存订单信息成功", "提醒", MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("异常发生,请检查数据库连接或数据是否符合标准", "错误", MessageBoxButton.OK,
                    MessageBoxImage.Error);
                //写入日志
            }            
        }

        //添加订单明细
        private void AddOneOrderDetail_Click(object sender, RoutedEventArgs e)
        {
            if(tb_Gname.Text == "" ||
                tb_Gnum.Text == "" ||
                tb_Gprice.Text == "")
            {
                MessageBox.Show("订单细则信息不完整", "错误", MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return;
            }

            if(tb_Ono.Text == "")
            {
                MessageBox.Show("请输入订单号","错误",MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return;
            }

            OrderDetailList.Tables["OrderDetail"].Rows.Add(
                new object[] {tb_Ono.Text,
                              tb_Gname.Tag,
                              Convert.ToDouble(tb_Gnum.Text),
                              Convert.ToDouble(tb_Gprice.Text),
                              tb_Gname.Text});

        }

        //商品列表
        private void GoodsList_Click(object sender, RoutedEventArgs e)
        {
            OrderGoodsList ogl = new OrderGoodsList();
            ogl.Owner = this;
            ogl.ShowDialog();
        }

        //顾客列表
        private void CustomerList_Click(object sender, RoutedEventArgs e)
        {
            CustomerList cl = new CustomerList();
            cl.Owner = this;
            cl.ShowDialog();
        }

        private void DeleteOneOrderDetail_Click(object sender, RoutedEventArgs e)
        {
            object item = LV_OrderDetail.SelectedItem;
            if (item == null)
            {
                MessageBox.Show("请选择一个订单", "错误", MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return;
            }

            int n = LV_OrderDetail.SelectedIndex;
            OrderDetailList.Tables["OrderDetail"].Rows[n].Delete();
        }
    }
}
