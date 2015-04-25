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
    /// SupplierInfoManage.xaml 的交互逻辑
    /// </summary>
    public partial class SupplierInfoManage : Window
    {
        private Supplier supplierInfo = null;
        private bool bAddNewSupplier = false;
       
        //添加新的供应商信息
        public SupplierInfoManage()
        {
            InitializeComponent();

            bAddNewSupplier = true;
            supplierInfo = new Supplier();
            Grid_Supplier.DataContext = supplierInfo;
        }

        //重载构造函数
        //修改选择的供应商的信息
        public SupplierInfoManage(Supplier supplier)
        {
            InitializeComponent();

            bAddNewSupplier = false;
            supplierInfo = supplier;
            Tb_Sno.IsEnabled = false;   //不可修改供应商编号
            Grid_Supplier.DataContext = supplierInfo;
        }

        private void SaveInfo_Click(object sender, RoutedEventArgs e)
        {
            //保存供应商信息
            SupplierAdapter adapter = new SupplierAdapter(SQLConnection.GetDataBase());
            try
            {
                if (bAddNewSupplier)    //添加新的供应商信息
                {
                    adapter.AddOneSupplier(supplierInfo);
                    MessageBox.Show("添加供应商成功", "提醒",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else   //修改供应商信息
                {
                    adapter.UpdateOneSupplier(supplierInfo);
                    MessageBox.Show("更新供应商成功", "提醒",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("异常发生,请检查数据库连接或数据是否符合标准", "提醒", MessageBoxButton.OK,
                    MessageBoxImage.Information);
                //写入日志
            }
        }
    }
}
