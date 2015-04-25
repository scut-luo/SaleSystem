using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DatabaseControlLib
{
    public class Goods
    {
        private string m_Gno;
        private string m_Gname;
        private string m_Gmanufacturer;

        public string Gno
        {
            get
            {
                return m_Gno;
            }
            set
            {
                m_Gno = value;
            }
        }
        public string Gname
        {
            get
            {
                return m_Gname;
            }
            set
            {
                m_Gname = value;
            }
        }
        public string Gmanufacturer
        {
            get
            {
                return m_Gmanufacturer;
            }
            set
            {
                m_Gmanufacturer = value;
            }
        }

        //构造函数
        public Goods(string gno, string gname, string gmanufacturer)
        {
            m_Gno = gno;
            m_Gname = gname;
            m_Gmanufacturer = gmanufacturer;
        }
    }
}
