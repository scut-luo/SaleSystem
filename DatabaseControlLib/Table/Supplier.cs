using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.ComponentModel;

namespace DatabaseControlLib
{
    //实现属性更改通知
    public class Supplier : INotifyPropertyChanged
    {
        private string m_Sno;
        private string m_Sname;
        private string m_Saddr;
        private string m_Sphone;

        public string Sno   //供应商编号 
        {
            get { return m_Sno; }
            set
            {
                m_Sno = value;
                OnPropertyChanged("Sno");
            }
        }
        public string Sname //供应商名字
        {
            get { return m_Sname; }
            set
            {
                m_Sname = value;
                OnPropertyChanged("Sname");
            }
        }
        public string Saddr //供应商地址
        {
            get { return m_Saddr; }
            set
            {
                m_Saddr = value;
                OnPropertyChanged("Saddr");
            }
        }
        public string Sphone  //供应商电话
        {
            get { return m_Sphone; }
            set
            {
                m_Sphone = value;
                OnPropertyChanged("Sphone");
            }
        }        

        //构造函数
        public Supplier(string sno, string sname, string saddr, string sphone)
        {
            m_Sno = sno;
            m_Sname = sname;
            m_Saddr = saddr;
            m_Sphone = sphone;
        }

        public Supplier()
        {

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

    }
}
