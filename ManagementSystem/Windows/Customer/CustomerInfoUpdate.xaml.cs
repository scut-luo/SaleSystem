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

namespace ManagementSystem
{
    /// <summary>
    /// CustomerInfoUpdate.xaml 的交互逻辑
    /// </summary>
    public partial class CustomerInfoUpdate : Window
    {
        private CustomerAdapter adapter = new CustomerAdapter(
            SQLConnection.GetDataBase());
        public CustomerInfoUpdate()
        {
            InitializeComponent();
        }

        //取消
        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        //保存信息
        private void Save_Click(object sender, RoutedEventArgs e)
        {
            //验证信息
            if(tb_Cno.Text == "" ||
                tb_Cname.Text == "" ||
                tb_Cphone.Text == "" ||
                tb_Caddr.Text == "")
            {
                MessageBox.Show("请输入完整顾客信息", "错误", MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return;
            }

            //顾客信息
            Customer customer = new Customer(tb_Cno.Text, tb_Cname.Text,
                tb_Caddr.Text, tb_Cphone.Text);
            try
            {
                adapter.AddOneCustomer(customer);
                MessageBox.Show("添加商品成功", "提醒", MessageBoxButton.OK,
                    MessageBoxImage.Information);

            }
            catch(Exception ex)
            {
                MessageBox.Show("异常发生,请检查数据库连接或数据是否符合标准", "错误", MessageBoxButton.OK,
                    MessageBoxImage.Error);
                //写入日志
            }
        }
    }
}
