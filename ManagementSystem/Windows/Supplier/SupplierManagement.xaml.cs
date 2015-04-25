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
    /// Supplier.xaml 的交互逻辑
    /// </summary>
    public partial class SupplierManagement : Window
    {
        private DataSet SupplierList = null;
        public SupplierManagement()
        {
            InitializeComponent();

            LoadData();
        }

        private void LoadData()
        {
            try
            {
                SupplierAdapter adapter = new SupplierAdapter(SQLConnection.GetDataBase());
                SupplierList = adapter.GetAllSupplier();        //获取所有供应商信息
                SupplierListView.ItemsSource = SupplierList.Tables[0].DefaultView;
            }
            catch (Exception ex)
            {
                MessageBox.Show("异常发生,请检查数据库连接", "错误", MessageBoxButton.OK,
                    MessageBoxImage.Error);
                //写入日志
            }
        }

        //添加供应商信息
        private void AddSupplier(object sender, RoutedEventArgs e)
        {
            SupplierInfoManage sim = new SupplierInfoManage();
            sim.ShowDialog();
            LoadData();
        }

        //更新供应商信息
        private void UpdateSupplier(object sender, RoutedEventArgs e)
        {
            Object item = SupplierListView.SelectedItem;
            if (item == null)
            {
                MessageBox.Show("选择要更改的供应商", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            DataRowView selectedItem = item as DataRowView;
            Supplier supplierInfo = new Supplier();

            //有待改进
            supplierInfo.Sno = selectedItem["Sno"].ToString();
            supplierInfo.Sname = selectedItem["Sname"].ToString();
            supplierInfo.Sphone = selectedItem["Sphone"].ToString();
            supplierInfo.Saddr = selectedItem["Saddr"].ToString();
            SupplierInfoManage sim = new SupplierInfoManage(supplierInfo);
            sim.ShowDialog();
            LoadData();
        }
    }
}
