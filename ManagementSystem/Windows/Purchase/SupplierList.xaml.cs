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
    /// SupplierList.xaml 的交互逻辑
    /// </summary>
    public partial class SupplierList : Window
    {
        private DataSet Suppliers = null;
        public SupplierList()
        {
            InitializeComponent();
            LoadData();
        }

        //加载数据
        private void LoadData()
        {
            try
            {
                SupplierAdapter adapter = new SupplierAdapter(SQLConnection.GetDataBase());
                Suppliers = adapter.GetAllSupplier();        //获取所有供应商信息
                LV_Supplier.ItemsSource = Suppliers.Tables[0].DefaultView;
            }
            catch (Exception ex)
            {
                MessageBox.Show("异常发生,请检查数据库连接", "错误", MessageBoxButton.OK,
                    MessageBoxImage.Error);
                //写入日志
            }
        }

        private void ManageSupplier_Click(object sender, RoutedEventArgs e)
        {
            SupplierManagement sm = new SupplierManagement();
            sm.ShowDialog();
            LoadData();
        }

        private void AddSupplier_Click(object sender, RoutedEventArgs e)
        {
            Object item = LV_Supplier.SelectedItem;
            if (item == null)
            {
                MessageBox.Show("选择要添加的供应商", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            DataRowView selectedItem = item as DataRowView;
            PurchaseGoodsAddAndUpdate pgau = this.Owner as PurchaseGoodsAddAndUpdate;

            pgau.tb_Supplier.Text = selectedItem["Sname"].ToString();
            pgau.tb_Supplier.Tag = selectedItem["Sno"].ToString();

            Close();
        }
    }
}
