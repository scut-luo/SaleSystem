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
    /// Interaction logic for CustomerList.xaml
    /// </summary>
    public partial class CustomerList : Window
    {
        private CustomerAdapter adapter = new CustomerAdapter(
            SQLConnection.GetDataBase());
        private DataSet customerList = new DataSet();       //顾客列表
        public CustomerList()
        {
            InitializeComponent();

            LoadData();
        }

        //加载数据
        private void LoadData()
        {
            try
            {
                customerList = adapter.GetAllCustomer();
                LV_Customer.ItemsSource = customerList.Tables[0].DefaultView;
            }
            catch (Exception ex)
            {
                MessageBox.Show("异常发生,请检查数据库连接", "错误", MessageBoxButton.OK,
                    MessageBoxImage.Error);

                //清除列表
                LV_Customer.Items.Clear();

                //写入日志
            }
        }

        //添加顾客信息
        private void AddCustomer_Click(object sender, RoutedEventArgs e)
        {
            Object item = LV_Customer.SelectedItem;
            if (item == null)
            {
                MessageBox.Show("选择要添加的顾客", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            DataRowView selectedItem = item as DataRowView;
            OrderInfoUpdate oiu = this.Owner as OrderInfoUpdate;

            oiu.tb_Cname.Text = selectedItem["Cname"].ToString();
            oiu.tb_Cname.Tag = selectedItem["Cno"].ToString();

            this.Close();
        }

        private void ManageCustomer_Click(object sender, RoutedEventArgs e)
        {
            CustomerManage cm = new CustomerManage();
            cm.ShowDialog();
            LoadData();
        }
    }
}
