using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.ComponentModel;

namespace DatabaseControlLib
{
    //实现属性更改通知
    public class Purchase : INotifyPropertyChanged
    {
        private string m_Pno;       //进货单号
        private string m_Sno;       //供应商编号
        private DateTime m_PDate;   //进货时间

        //进货单号
        public string Pno
        {
            get { return m_Pno; }
            set
            {
                m_Pno = value;
                OnPropertyChanged("Pno");                
            }
        }

        //供应商编号
        public string Sno
        {
            get { return m_Sno; }
            set
            {
                m_Sno = value;
                OnPropertyChanged("Sno");
            }
        }

        //进货时间
        public DateTime PDate
        {
            get { return m_PDate; }
            set
            {
                m_PDate = value;
                OnPropertyChanged("PDate");
            }
        }

        //构造函数
        public Purchase(string pno, string sno, DateTime pdate)
        {
            m_Pno = pno;
            m_Sno = sno;
            m_PDate = pdate;
        }

        public Purchase()
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
