using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DatabaseControlLib
{
    public class Customer
    {
        private string m_Cno;
        private string m_Cname;
        private string m_Caddr;
        private string m_Cphone;

        //顾客编号
        public string Cno
        {
            get { return m_Cno; }
            set
            {
                m_Cno = value;
            }
        }

        //顾客名字
        public string Cname
        {
            get { return m_Cname; }
            set
            {
                m_Cname = value;
            }
        }

        //顾客地址
        public string Caddr
        {
            get { return m_Caddr; }
            set
            {
                m_Caddr = value;
            }
        }

        //顾客电话
        public string Cphone
        {
            get { return m_Cphone; }
            set
            {
                m_Cphone = value;
            }
        }

        //构造函数
        public Customer()
        {

        }
        public Customer(string cno, string cname, string caddr, string cphone)
        {
            m_Cno = cno;
            m_Cname = cname;
            m_Caddr = caddr;
            m_Cphone = cphone;
        }
    }
}
