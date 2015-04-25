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
    /// SystemUserAdd.xaml 的交互逻辑
    /// </summary>
    public partial class SystemUserAdd : Window
    {
        private SysUserAdapter adapter = new SysUserAdapter(
            SQLConnection.GetDataBase());
        public SystemUserAdd()
        {
            InitializeComponent();

            //窗口初始化
            Init();
        }

        private void Init()
        {            
            //判断可以添加的用户类型
            if (LoginInfo.LoginID == 0) //登录用户为root用户
                CBI_Admin.IsEnabled = true;
            else
                CBI_Admin.IsEnabled = false;
        }
        //添加系统用户
        private void SysUserAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DataSet dataSet = null;
                DataTable dt = null;

                //判断数据正确性
                if (tb_username.Text == "" ||
                    tb_passwd.Password == "" ||
                    tb_passwdAgain.Password == "")
                {
                    MessageBox.Show("未输入完整数据", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (tb_passwd.Password != tb_passwdAgain.Password)
                {
                    MessageBox.Show("输入两次的密码不一致", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                //判断登录类型,确定UID
                string query;
                int maxUID, uid;
                if (CBI_Admin.IsSelected == true)
                {
                    maxUID = adapter.GetMaxUIDOfAdmin();
                    if (maxUID + 1 > 500)     //超过添加用户上限
                    {
                        MessageBox.Show("超过管理员个数上限", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                }
                else
                {
                    maxUID = adapter.GetMaxUIDOfUser();
                }

                uid = maxUID + 1;

                //判断是否可以登录
                bool bCanlogin = true;
                if (RadionY.IsChecked == true)       //允许登录
                    bCanlogin = true;
                else
                    bCanlogin = false;

                SysUser sysUser = new SysUser(uid, tb_username.Text, tb_passwd.Password, bCanlogin);
                adapter.AddOneSysUser(sysUser);

                MessageBox.Show("添加用户成功", "提醒", MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }
            catch (Exception ex)
            {                
                MessageBox.Show("异常发生,请检查数据库连接或数据是否符合标准", "错误", MessageBoxButton.OK,
                    MessageBoxImage.Error);
                //写入日志
            }            
        }

        private void CloseWindow_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
