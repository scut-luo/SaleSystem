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
    /// SysUser.xaml 的交互逻辑
    /// </summary>
    public partial class SysUserManage : Window
    {
        private DataSet SysUserList = new DataSet();
        private SysUserAdapter adapter = new SysUserAdapter(
            SQLConnection.GetDataBase());
        public SysUserManage()
        {
            InitializeComponent();

            LoadData();
        }

        //加载数据
        private void LoadData()
        {
            try
            {
                SysUserList = adapter.GetAllUser();
                LV_SysUser.ItemsSource = SysUserList.Tables["SysUser"].DefaultView;
            }
            catch (Exception ex)
            {
                MessageBox.Show("异常发生,请检查数据库连接", "错误", MessageBoxButton.OK,
                    MessageBoxImage.Error);
                //写入日志
            }
        }

        //添加新的用户
        private void AddSysUser_Click(object sender, RoutedEventArgs e)
        {
            SystemUserAdd sua = new SystemUserAdd();
            sua.ShowDialog();
            LoadData();
        }
    }
}
