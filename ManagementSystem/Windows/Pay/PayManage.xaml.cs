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
    /// Interaction logic for PayManage.xaml
    /// </summary>
    public partial class PayManage : Window
    {
        private DBase db = SQLConnection.GetDataBase();
        private PayAdapter payAdapter = new PayAdapter(
            SQLConnection.GetDataBase());
        private DataSet PayList = new DataSet();
        public PayManage()
        {
            InitializeComponent();
            LoadData();
        }

        private void LoadData()
        {
            try
            {
                string Query = "SELECT First.Cno,Ono,Pdate,Pmoney,Cname FROM Pay First,Customer Second WHERE First.Cno=Second.Cno";
                PayList = db.openQueryDS(Query, "Pay");
                LV_Pay.ItemsSource = PayList.Tables["Pay"].DefaultView;
            }
            catch (Exception ex)
            {
                LV_Pay.Items.Clear();

                MessageBox.Show("异常发生,请检查数据库连接", "错误", MessageBoxButton.OK,
                    MessageBoxImage.Error);
                //写入日志
            }
        }

        //更新支付单到支付列表中
        private void UpdatePayInfo_Click(object sender, RoutedEventArgs e)
        {
            object item = LV_Pay.SelectedItem;
            if (item == null)
                return;
            DataRowView oneRow = item as DataRowView;

            //获取支付数据
            oneRow["Pdate"] = dp_Pdate.SelectedDate;
            oneRow["Pmoney"] = Convert.ToDouble(tb_Pmoney.Text);

            try
            {
                if (PayList.HasChanges())
                    payAdapter.UpdateData(PayList);
                MessageBox.Show("更新支付信息成功", "提醒", MessageBoxButton.OK,
                    MessageBoxImage.Information);                
            }
            catch (Exception ex)
            {                
                MessageBox.Show("异常发生,请检查数据库连接或数据是否符合标准", "错误", MessageBoxButton.OK,
                    MessageBoxImage.Error);
                //写入日志
            }
            LoadData();
        }

        //选择不同支付单时显示支付数据
        private void PaySelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            object item = LV_Pay.SelectedItem;
            if (item == null)
                return;
            DataRowView oneRow = item as DataRowView;

            tb_Cname.Text = oneRow["Cname"].ToString();
            tb_Cname.Tag = oneRow["Cno"].ToString();
            tb_Ono.Text = oneRow["Ono"].ToString();

            if (!DBNull.Value.Equals(oneRow["Pdate"]))
                dp_Pdate.SelectedDate = Convert.ToDateTime(oneRow["Pdate"].ToString());
            tb_Pmoney.Text = oneRow["Pmoney"].ToString();
        }
    }
}
