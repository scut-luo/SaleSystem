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
    /// CustomerManage.xaml 的交互逻辑
    /// </summary>
    public partial class CustomerManage : Window
    {
        private DataSet CustomerList = null;
        public CustomerManage()
        {
            InitializeComponent();

            LoadData();
        }

        private void LoadData()
        {
            try
            {
                CustomerAdapter adapter = new CustomerAdapter(
                    SQLConnection.GetDataBase());
                CustomerList = adapter.GetAllCustomer();
                LV_Customer.ItemsSource = CustomerList.Tables[0].DefaultView;
                
            }
            catch (Exception ex)
            {
                MessageBox.Show("异常发生,请检查数据库连接", "错误", MessageBoxButton.OK,
                    MessageBoxImage.Error);
                //写入日志
            }
        }

        //添加顾客信息
        private void AddCustomer_Click(object sender, RoutedEventArgs e)
        {
            CustomerInfoUpdate ciu = new CustomerInfoUpdate();
            ciu.ShowDialog();
        }
    }
}
