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
    /// GoodsManagement.xaml 的交互逻辑
    /// </summary>
    public partial class GoodsManagement : Window
    {
        private DataSet GoodsList = null;
        public GoodsManagement()
        {
            InitializeComponent();

            LoadData();
        }

        private void LoadData()
        {
            try
            {
                GoodsAdapter adapter = new GoodsAdapter(
                    SQLConnection.GetDataBase());
                GoodsList = adapter.GetAllGoods();
                LV_Goods.ItemsSource = GoodsList.Tables[0].DefaultView;
            }
            catch (Exception ex)
            {
                MessageBox.Show("异常发生,请检查数据库连接", "错误", MessageBoxButton.OK,
                    MessageBoxImage.Error);
                //写入日志
            }
        }

        //添加商品信息
        private void AddGoods_Click(object sender, RoutedEventArgs e)
        {
            GoodsInfoAddAndUpdate giau = new GoodsInfoAddAndUpdate();
            giau.ShowDialog();
            LoadData();
        }

        //更改商品信息
        private void UpdateGoodsInfo_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
