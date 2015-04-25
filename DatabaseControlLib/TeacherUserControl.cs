using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Collections;
using System.Data.OleDb;
using System.Data;

namespace DatabaseControlLib
{
    public class TeacherUserControl : DBTable
    {
        private ArrayList TeacherUserList;

        //构造函数
        public TeacherUserControl(DBase db)
            : base(db, "TeacherUser")
        {
            TeacherUserList = new ArrayList();
        }

        public DataTable GetDataByTeacherNum(string TeacherNum)
        {
            string query = "Select * from TeacherUser where Tnum = '" + TeacherNum + "';";
            return db.openQueryDT(query);
        }
    }
}
