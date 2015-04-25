using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Collections;
using System.Data.OleDb;
using System.Data;

namespace DatabaseControlLib
{
    public class StudentUserControl : DBTable
    {
        private ArrayList StudentUserList;  

        //构造函数
        public StudentUserControl(DBase db)
            : base(db, "StudentUser")
        {
            StudentUserList = new ArrayList();
        }

        public DataTable GetDataByStudentNum(string StudentNum)
        {
            string query = "Select * from StudentUser where Snum = '" + StudentNum + "';";
            return db.openQueryDT(query);
        }
    }
}
