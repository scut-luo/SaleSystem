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
    /// GoodsInfoAddAndUpdate.xaml 的交互逻辑
    /// </summary>
    public partial class GoodsInfoAddAndUpdate : Window
    {
       
        public GoodsInfoAddAndUpdate()
        {
            InitializeComponent();
        }

        //保存更改信息
        private void SaveGoodsInfo_Click(object sender, RoutedEventArgs e)
        {
            if(tb_Gno.Text == "" ||
                tb_Gname.Text == "" ||
                tb_Gmanufacturer.Text == "")
            {
                MessageBox.Show("未填满空项", "错误", MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return;
            }

            Goods goods = new Goods(tb_Gno.Text,tb_Gname.Text,
                tb_Gmanufacturer.Text);
            GoodsAdapter adapter = new GoodsAdapter(
                SQLConnection.GetDataBase());
            try
            {
                adapter.AddOneGoods(goods);
                MessageBox.Show("添加商品成功", "提醒", MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("异常发生,请检查数据库连接或数据是否符合标准", "错误", MessageBoxButton.OK,
                    MessageBoxImage.Error);
                //写入日志
            }
        }
    }
}
