using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Collections;
using System.Data.OleDb;
using System.Data;

namespace DatabaseControlLib
{
    //数据表 Course 操作
    public class CourseControl : DBTable
    {
        private ArrayList CourseList;      //保存课程列表

        //构造函数
        public CourseControl(DBase db)
            : base(db, "Course")
        {
            CourseList = new ArrayList();
        }
      
        //更新数据（单个数据）
        public bool CourseUpdate_SingleData(Course course)
        {
            return true;
        }

        //插入数据（单个数据）
        public bool CourseInsert_SingleDate(Course course)
        {
            bool reValue = false;
            try
            {
                OleDbConnection odc = new OleDbConnection();

                odc = db.getConnection();

                if (odc.State == ConnectionState.Closed)    //判断是否打开了数据库
                    odc.Open();

                //构造插入SQL语句
                string strAdd = "Insert into Course ";
                strAdd += "Values('" + course.Cnum + "','" + course.Cname + "','" +
                    course.Ccredit.ToString() + "','" + course.Chours.ToString() + "');";

                OleDbCommand oledCmd = new OleDbCommand(strAdd, odc);   //OleDbCommand对象

                if (oledCmd.ExecuteNonQuery() != 0)
                    reValue = true;
            }
            catch (Exception ex)
            {
                reValue = false;       //添加发生错误
            }
            return reValue;
        }

        //单元数据添加到数组列表
        public void addRow(string Cnum, string Cname, int Ccredit, int Chours)
        {
            CourseList.Add(new Course(Cnum, Cname, Ccredit, Chours));
        }
       
    }
}
