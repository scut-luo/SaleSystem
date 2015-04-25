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
    /// OrderManage.xaml 的交互逻辑
    /// </summary>
    public partial class OrderManage : Window
    {
        //数据库接口定义
        DBase db = SQLConnection.GetDataBase();
        private OrdersAdapter orderAdapter = new OrdersAdapter(
            SQLConnection.GetDataBase());
        private OrderDetailAdapter orderDetailAdapter = new OrderDetailAdapter(
            SQLConnection.GetDataBase());
        /*
        private CustomerAdapter customerAdapter = new CustomerAdapter(
            SQLConnection.GetDataBase());
        private GoodsAdapter goodsAdapter = new GoodsAdapter(
            SQLConnection.GetDataBase());       
        
        */
        //数据表存储
        private DataSet OrderList = new DataSet();      //订单列表  
        private DataSet OrderDetailList = new DataSet();    //订单细节列表
        public OrderManage()
        {
            InitializeComponent();

            //初始化窗口
            LoadData();
        }

        //加载数据
        private void LoadData()
        {
            try
            {
                LV_Order.Items.Clear();
                LV_OrderDetail.Items.Clear();

                LoadOrderData();
                
                //加载订单细节
                if (LV_Order.Items.Count != 0)
                {
                    //选择第一项
                    LV_Order.SelectedIndex = 0;
                    //LoadOrderDetailData(0);
                }
            }
            catch (Exception ex)
            {
                //清除列表
                LV_Order.Items.Clear();
                LV_OrderDetail.Items.Clear();

                MessageBox.Show("异常发生,请检查数据库连接", "错误", MessageBoxButton.OK,
                    MessageBoxImage.Error);                
                //写入日志                
            }
        }

        //加载订单数据
        private void LoadOrderData()
        {
            /*
            string OrderName = "Order_T";
            DataSet CustomerList = new DataSet();

            CustomerList = customerAdapter.GetAllCustomer();        //获取所有供应商信息
            OrderList = orderAdapter.GetAllOrder();                 //获取所有订单信息

            OrderList.Tables["Orders"].TableName = OrderName;        //修改原始表名
            
            DataTable CustomerTable = CustomerList.Tables["Customer"];
            CustomerList.Tables.Remove("Customer");     //删除表
            OrderList.Tables.Add(CustomerTable);        //添加表

            DataSetHelper ocHelper = new DataSetHelper(ref OrderList);

            OrderList.Relations.Add("OAndC", OrderList.Tables["Customer"].Columns["Cno"],
                                    OrderList.Tables[OrderName].Columns["Cno"]);
            ocHelper.SelectJoinInto("Orders", OrderList.Tables[OrderName],
                                    "Ono,Cno,Odate,OSum,Omoney,OAndC.Cname", "1=1", "Ono");
            */
            string Query = "SELECT Ono,First.Cno,Odate,Osum,Omoney,Cname FROM Orders First,Customer Second " + 
                                   "WHERE First.Cno=Second.Cno";

            OrderList = db.openQueryDS(Query, "Orders");
            LV_Order.ItemsSource = OrderList.Tables["Orders"].DefaultView;
        }

        //加载订单细节数据
        private void LoadOrderDetailData(int selectedIndex)
        {
            /*
            if (selectedIndex == -1)
                return;
            
            string OrderDetailName = "OrderDetail_T";
            DataSet GoodsList = goodsAdapter.GetAllGoods();

            OrderDetailList = orderDetailAdapter.GetAllOrderDetail();
            OrderDetailList.Tables["OrderDetail"].TableName = OrderDetailName;  //修改原始表名

            DataTable GoodsTable = GoodsList.Tables["Goods"];
            GoodsList.Tables.Remove("Goods");       //删除表
            OrderDetailList.Tables.Add(GoodsTable); //添加表

            DataSetHelper odgHelper = new DataSetHelper(ref OrderDetailList);

            OrderDetailList.Relations.Add("ODAndG", OrderDetailList.Tables["Goods"].Columns["Gno"],
                                           OrderDetailList.Tables[OrderDetailName].Columns["Gno"]);
            odgHelper.SelectJoinInto("OrderDetail", OrderDetailList.Tables[OrderDetailName],
                                     "Ono,Gno,Gnum,Gprice,ODAndG.Gname", "1=1", "Gno");
            
            LV_OrderDetail.ItemsSource = OrderDetailList.Tables["OrderDetail"].DefaultView;
            */
        }

        private void AddOrder_Click(object sender, RoutedEventArgs e)
        {
            OrderInfoUpdate oiu = new OrderInfoUpdate(true);
            oiu.ShowDialog();
            LoadData();
        }

        private void EditOrder_Click(object sender, RoutedEventArgs e)
        {
            object item = LV_Order.SelectedItem;
            if(item == null)
            {
                MessageBox.Show("请选择一个订单","错误",MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }

            DataRowView oneRow = item as DataRowView;
            OrderInfoUpdate oiu = new OrderInfoUpdate(false,
                new Orders(oneRow["Ono"].ToString(),
                           oneRow["Cno"].ToString(),
                           Convert.ToDateTime(oneRow["Odate"].ToString()),
                           Convert.ToDouble(oneRow["Osum"].ToString()),
                           Convert.ToDouble(oneRow["Omoney"].ToString())));
            oiu.ShowDialog();
            LoadData();
        }

        private void OrderSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            object item = LV_Order.SelectedItem;
            if (item == null)
                return;
            try
            {
                string Ono = (item as DataRowView)["Ono"].ToString();
                string Query = string.Format("SELECT * FROM OrderDetail First,Goods Second " +
                                                "WHERE First.Gno=Second.Gno AND Ono='{0}'", Ono);
                OrderDetailList =  db.openQueryDS(Query, "OrderDetail");
                LV_OrderDetail.ItemsSource = OrderDetailList.Tables["OrderDetail"].DefaultView;
            }
            catch (Exception ex)
            {
                LV_OrderDetail.Items.Clear();

                MessageBox.Show("异常发生,请检查数据库连接", "错误", MessageBoxButton.OK,
                    MessageBoxImage.Error);
                //写入日志 
            }            
        }

        //删除订单
        private void DeleteOrder_Click(object sender, RoutedEventArgs e)
        {
            object item = LV_Order.SelectedItem;
            if (item == null)
            {
                MessageBox.Show("请选择一个订单", "错误", MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return;
            }

            int n = LV_Order.SelectedIndex;

            try
            {
                foreach (DataRow oneRow in OrderDetailList.Tables["OrderDetail"].Rows)
                    oneRow.Delete();
                orderDetailAdapter.UpdateData(OrderDetailList);

                OrderList.Tables["Orders"].Rows[n].Delete();
                orderAdapter.UpdateData(OrderList);

                MessageBox.Show("删除订单信息成功", "提醒", MessageBoxButton.OK,
                    MessageBoxImage.Information);
                LoadData();
            }
            catch (Exception ex)
            {
                LV_OrderDetail.Items.Clear();

                MessageBox.Show("异常发生,请检查数据库连接", "错误", MessageBoxButton.OK,
                    MessageBoxImage.Error);
                //写入日志 
            }            
        }
    }
}
