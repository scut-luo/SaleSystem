using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.ComponentModel;

namespace DatabaseControlLib
{
    //数据表Orders
    //实现属性更改通知
    public class Orders :  INotifyPropertyChanged
    {
        private string m_Ono;
        private string m_Cno;
        private DateTime m_Odate;
        private double m_Osum;
        private double m_Omoney;

        //订单号
        public string Ono
        {
            get { return m_Ono; }
            set
            {
                m_Ono = value;
                OnPropertyChanged("Ono");  
            }
        }

        //顾客号
        public string Cno
        {
            get { return m_Cno; }
            set
            {
                m_Cno = value;
                OnPropertyChanged("Cno");
            }
        }

        //订单日期
        public DateTime Odate
        {
            get { return m_Odate; }
            set
            {
                m_Odate = value;
                OnPropertyChanged("Odate");  
            }
        }

        //订单数量
        public double Osum
        {
            get { return m_Osum; }
            set
            {
                m_Osum = value;
                OnPropertyChanged("Osum");
            }
        }

        //订单总金额
        public double Omoney
        {
            get { return m_Omoney; }
            set
            {
                m_Omoney = value;
                OnPropertyChanged("Omoney");  
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }
        
        //构造函数
        public Orders()
        {

        }
        public Orders(string ono, string cno, DateTime odate, double osum, double omoney)
        {
            m_Ono = ono;
            m_Cno = cno;
            m_Odate = odate;
            m_Osum = osum;
            m_Omoney = omoney;
        }
    }
}
