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
    /// GoodsList.xaml 的交互逻辑
    /// </summary>
    public partial class OrderGoodsList : Window
    {
        private DataSet Goods = null;
        public OrderGoodsList()
        {
            InitializeComponent();

            LoadData();
        }

        //加载数据
        private void LoadData()
        {
            try
            {
                GoodsAdapter adapter = new GoodsAdapter(SQLConnection.GetDataBase());
                Goods = adapter.GetAllGoods();        //获取所有供应商信息
                LV_Goods.ItemsSource = Goods.Tables[0].DefaultView;
            }
            catch (Exception ex)
            {
                MessageBox.Show("异常发生,请检查数据库连接", "错误", MessageBoxButton.OK,
                    MessageBoxImage.Error);
                //写入日志                
            }
        }

        //管理商品信息
        private void ManageGoods_Click(object sender, RoutedEventArgs e)
        {
            GoodsManagement gm = new GoodsManagement();
            gm.ShowDialog();
            LoadData();
        }

        //添加商品信息
        private void AddGoods_Click(object sender, RoutedEventArgs e)
        {
            Object item = LV_Goods.SelectedItem;
            if (item == null)
            {
                MessageBox.Show("选择要添加的商品", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            DataRowView selectedItem = item as DataRowView;
            OrderInfoUpdate oiu = this.Owner as OrderInfoUpdate;

            oiu.tb_Gname.Text = selectedItem["Gname"].ToString();
            oiu.tb_Gname.Tag = selectedItem["Gno"].ToString();

            this.Close();
        }
    }
}
