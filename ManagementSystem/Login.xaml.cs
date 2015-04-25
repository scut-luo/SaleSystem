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
    /// Login.xaml 的交互逻辑
    /// </summary>
    public partial class Login : Window
    {
        public Login()
        {
            InitializeComponent();
        }

        //测试能否连接到数据库
        private bool TestConnection()
        {
            SQLServerDatabase ssdb = new SQLServerDatabase(ConnectionInfo.DBName, ConnectionInfo.ServerName);
            bool reValue = true;

            try
            {
                ssdb.openConnection();  //打开数据库
            }
            catch
            {
                reValue = false;
            }
            finally
            {
                ssdb.closeConnection();
            }
            return reValue;
        }

        //登录系统
        private void LoginSystem_Click(object sender, RoutedEventArgs e)
        {
            if(tb_username.Text == "" ||
                pb_password.Password == "")
            {
                MessageBox.Show("所有项都必须填写！", "错误",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            string username = tb_username.Text;
            string password = pb_password.Password;
            SysUserAdapter adapter = new SysUserAdapter(SQLConnection.GetDataBase());   
         
            //登录验证
            try
            {
                //用户存在
                if (adapter.IFHasUser(username))
                {
                    //判断用户是否能登录系统
                    if (adapter.IFCanlogin(username))
                    {
                        if (adapter.IFPasswordCorrect(username,password))
                        {
                            LoginInfo.LoginName = username;
                            LoginInfo.LoginID = adapter.GetUID(username);

                            this.Visibility = System.Windows.Visibility.Hidden;  //隐藏登录窗口

                            Main main = new Main();
                            main.ShowDialog();

                            this.Visibility = System.Windows.Visibility.Visible;    //显示登录窗口
                            tb_username.Text = "";
                            pb_password.Password = "";
                        }
                        else
                        {
                            MessageBox.Show("密码错误！", "错误",
                                MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                    else
                    {
                        MessageBox.Show("账号已过期！\n激活请联系管理员", "错误",
                                MessageBoxButton.OK, MessageBoxImage.Error);
                    }

                }
                else
                {
                    MessageBox.Show("用户名不存在！", "错误",
                            MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("异常发生,请检查数据库连接", "提醒", MessageBoxButton.OK,
                    MessageBoxImage.Information);
                //写入日志
            }           
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void WindowLoaded(object sender, RoutedEventArgs e)
        {
            //测试是否能够连接数据库
            if (!TestConnection())
            {
                MessageBox.Show("数据库不能连接！", "错误",
                                MessageBoxButton.OK, MessageBoxImage.Error);
                this.Close();
            }
        }
        
    }

    //登录信息
    public class LoginInfo
    {
        public static int LoginID;
        public static string LoginName;
    }
}
