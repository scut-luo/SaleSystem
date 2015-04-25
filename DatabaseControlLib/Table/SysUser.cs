using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DatabaseControlLib
{
    public class SysUser
    {
        private int m_UID;
        private string m_Username;
        private string m_Password;
        private bool m_Canlogin;

        public int UID              //用户ID(UID)
        {
            get { return m_UID; }
            set
            {
                m_UID = value;
            }
        }
        public string Username      //用户名
        {
            get { return m_Username; }
            set
            {
                m_Username = value;
            }
        }
        public string Password      //用户密码
        {
            get { return m_Password; }
            set
            {
                m_Password = value;
            }
        }
        public bool Canlogin        //是否允许登录
        {
            get { return m_Canlogin; }
            set
            {
                m_Canlogin = value;
            }
        }

        //构造函数
        public SysUser()
        {

        }
        public SysUser(int uid, string username, string password, bool canlogin)
        {
            m_UID = uid;
            m_Username = username;
            m_Password = password;
            m_Canlogin = canlogin;
        }
    }
}
