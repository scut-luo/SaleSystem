using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Collections;
using System.Data.OleDb;
using System.Data;

namespace DatabaseControlLib
{
    public class SystemUserControl : DBTable
    {
        //构造函数
        public SystemUserControl(DBase db)
            : base(db,"SystemUser")
        {

        }

        public DataTable GetDataFromSystemUsername(string username)
        {
            string query = "Select * from SystemUser where SysUsername = '" + username + "';";
            return db.openQueryDT(query);
        }
        
    }
}
