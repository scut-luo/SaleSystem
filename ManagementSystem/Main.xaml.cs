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

using System.Windows.Threading;

namespace ManagementSystem
{
    /// <summary>
    /// Main.xaml 的交互逻辑
    /// </summary>
    public partial class Main : Window
    {
        private DispatcherTimer dispatcherTimer;

        public Main()
        {
            InitializeComponent();
            Init();
        }

        private void Init()
        {
            dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
            dispatcherTimer.Tick +=new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Start();

            Time.Text = DateTime.Now.ToString();
        }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            Time.Text = DateTime.Now.ToString();
        }

        //供应商信息
        private void MenuSupplierInfo_Click(object sender, RoutedEventArgs e)
        {
            SupplierManagement supplier = new SupplierManagement();
            supplier.ShowDialog();
        }

        //顾客信息管理
        private void MenuCustomerInfo_Click(object sender, RoutedEventArgs e)
        {
            CustomerManage cm = new CustomerManage();
            cm.Owner = this;
            cm.ShowDialog();
        }

        //进货管理
        private void MenuPurchaseGoods_Click(object sender, RoutedEventArgs e)
        {
            PurchaseGoodsManage pgm = new PurchaseGoodsManage();
            pgm.ShowDialog();
        }

        //商品信息管理
        private void MenuGoodsManage_Click(object sender, RoutedEventArgs e)
        {
            GoodsManagement gm = new GoodsManagement();
            gm.ShowDialog();
        }

        //系统用户管理
        private void MenuSysUserManage_Click(object sender, RoutedEventArgs e)
        {
            SysUserManage user = new SysUserManage();
            user.ShowDialog();
        }

        //订单管理
        private void MenuOrder_Click(object sender, RoutedEventArgs e)
        {
            OrderManage om = new OrderManage();
            om.ShowDialog();
        }

        private void MenuStorageManage_Click(object sender, RoutedEventArgs e)
        {
            StorageManage storage = new StorageManage();
            storage.ShowDialog();
        }

        private void MenuPay_Click(object sender, RoutedEventArgs e)
        {
            PayManage pm = new PayManage();
            pm.ShowDialog();
        }              
    }
}
