using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Collections;
using System.Data.OleDb;
using System.Data;

namespace DatabaseControlLib
{
    public class StudentControl : DBTable
    {
        private ArrayList StudentList;

        //构造函数
        public StudentControl(DBase db)
            : base(db, "Student")
        {
            StudentList = new ArrayList();
        }
       
        //获取单个用户的信息
        public Student GetStudentInfo(string StudentNum)
        {
            Student student = null;

            if (StudentNum != null)
            {
                DataTable dt;                
                string query = "SELECT * FROM " + tableName
                    + " WHERE Snum='" + StudentNum + "';";

                dt = db.openQueryDT(query, tableName);
                if (dt != null && dt.Rows.Count != 0)
                {
                    DataRow dr = dt.Rows[0];
                    student = new Student((int)dr["id"],
                                          (string)dr["Snum"],
                                          (string)dr["Sname"],
                                          (int)dr["Ssex"],
                                          (byte[])dr["Sphoto"],
                                          (string)dr["Sacademy"]);
                }
            }
            
            return student;
        }
    }
}
