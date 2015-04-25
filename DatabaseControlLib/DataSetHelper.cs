using System;
using System.Collections.Generic;
using System.Data;

namespace DatabaseControlLib
{
   public class DataSetHelper
   {


       public DataSet ds;

       private System.Collections.ArrayList m_FieldInfo;
       private string m_FieldList;

       public DataSetHelper(ref DataSet DataSet)
       {
           ds = DataSet;
       }
       public DataSetHelper()
       {
           ds = null;
       }
       /**//// <summary>
       /// 该方法根据给定的字段列表(FieldList)和表名（TableName），创建表结构，并返回表对象
       /// 给定的字段可来自创建了关系的两张表，如果是源表（子表）中的字段，直接写字段名即可。
       /// 如果是关系表（父表）中的字段，
       /// 字段前面须加上关系名称，格式如：relationname.fieldname
       /// FieldList语法：[relationname.]fieldname[ alias][,[relationname.]fieldname[ alias]]
       /// </summary>
       /// <param name="TableName">生成新结构表的表名</param>
       /// <param name="SourceTable">源表名（子表）</param>
       /// <param name="FieldList">生成新结构表的目标字段</param>
       /// <returns>具有目标结构的表对象</returns>
       public DataTable CreateJoinTable(string TableName, DataTable SourceTable, string FieldList)
       {
           if (FieldList == null)
           {
               throw new ArgumentException("You must specify at least one field in the field list.");
           }
           else
           {
               DataTable dt = new DataTable(TableName);
               ParseFieldList(FieldList, true);
               foreach (FieldInfo Field in m_FieldInfo)
               {
                   if (Field.RelationName == null)
                   {
                       DataColumn dc = SourceTable.Columns[Field.FieldName];
                       dt.Columns.Add(dc.ColumnName, dc.DataType, dc.Expression);
                   }
                   else
                   {
                       DataColumn dc = SourceTable.ParentRelations[Field.RelationName].ParentTable.Columns[Field.FieldName];
                       dt.Columns.Add(dc.ColumnName, dc.DataType, dc.Expression);
                   }
               }
               if (ds != null)
                   ds.Tables.Add(dt);
               return dt;
           }
       }


       /**//// <summary>
       /// 该方法用于关联查询，可以指定条件（RowFilter），以及排序字段（Sort）；
       /// 直接将查询结果存储到DestTable表对象中\n
       /// 在FieldList中的字段可以是关系表中的字段，但是它的前面必须加上关系名称，格式如：relationname.fieldname
       /// 用于指定查询条件的字段和用于排序的字段只能是源表中的字段，不能是关系表中的字段
       /// FieldList语法：[relationname.]fieldname[ alias][,[relationname.]fieldname[ alias]]
       /// </summary>
       /// <param name="DestTable">用于存储查询结果的表对象</param>
       /// <param name="SourceTable">源表名（子表）</param>
       /// <param name="FieldList">查询结果的目标字段</param>
       /// <param name="RowFilter">查询条件</param>
       /// <param name="Sort">排序字段</param>
       public void InsertJoinInto(DataTable DestTable, DataTable SourceTable, string FieldList, string RowFilter, string Sort)
       {
           if (FieldList == null)
           {
               throw new ArgumentException("You must specify at least one field in the field list.");
           }
           else
           {
               ParseFieldList(FieldList, true);
               DataRow[] Rows = SourceTable.Select(RowFilter, Sort);
               foreach (DataRow SourceRow in Rows)
               {
                   DataRow DestRow = DestTable.NewRow();
                   foreach (FieldInfo Field in m_FieldInfo)
                   {
                       if (Field.RelationName == null)
                       {
                           DestRow[Field.FieldName] = SourceRow[Field.FieldName];
                       }
                       else
                       {
                           DataRow ParentRow = SourceRow.GetParentRow(Field.RelationName);
                           DestRow[Field.FieldName] = ParentRow[Field.FieldName];
                       }
                   }
                   DestTable.Rows.Add(DestRow);
               }
           }
       }
       /**//// <summary>
       /// 1.该方法用于关联查询，可以指定条件（RowFilter），以及排序字段（Sort）；   
       /// 2.将查询结果存储到名称为TableName的表对象中；   
       /// 3.在FieldList中的字段可以是关系表中的字段，但是它的前面必须加上关系名称，格式如：relationname.fieldname；
       /// 4.用于指定查询条件的字段和用于排序的字段只能是源表中的字段，不能是关系表中的字段；   
       /// 5.FieldList语法：[relationname.]fieldname[ alias][,[relationname.]fieldname[ alias]]
       /// </summary>
       /// <param name="TableName">查询结果表名</param>
       /// <param name="SourceTable">源表名（子表）</param>
       /// <param name="FieldList">查询结果的目标字段</param>
       /// <param name="RowFilter">查询条件</param>
       /// <param name="Sort">排序字段</param>
       /// <returns>查询结果对象</returns>
       public DataTable SelectJoinInto(string TableName, DataTable SourceTable, string FieldList, string RowFilter, string Sort)
       {
           DataTable dt = CreateJoinTable(TableName, SourceTable, FieldList);
           InsertJoinInto(dt, SourceTable, FieldList, RowFilter, Sort);
           dt.AcceptChanges();
           return dt;
       }

       private void ParseFieldList(string FieldList, bool AllowRelation)
       {
           /**//*
            * 将FieldList中的字段转换为FieldInfo对象，并添加到集合m_FieldInfo中
            * 
            * FieldList 用例:  [relationname.]fieldname[ alias], 
           */
           if (m_FieldList == FieldList) return;
           m_FieldInfo = new System.Collections.ArrayList();
           m_FieldList = FieldList;
           FieldInfo Field;
           string[] FieldParts;
           string[] Fields = FieldList.Split(',');
           int i;
           for (i = 0; i <= Fields.Length - 1; i++)
           {
               Field = new FieldInfo();
               //转换别名，存储在Field.FieldAlias
               FieldParts = Fields[i].Trim().Split(' ');
               switch (FieldParts.Length)
               {
                   case 1:
                       //没有别名
                       break;
                   case 2:
                       Field.FieldAlias = FieldParts[1];
                       break;
                   default:
                       throw new Exception("Too many spaces in field definition: '" + Fields[i] + "'.");
               }
               //转换字段名称和关系名称，分别存储在Field.FieldName和Field.RelationName中
               FieldParts = FieldParts[0].Split('.');
               switch (FieldParts.Length)
               {
                   case 1:
                       Field.FieldName = FieldParts[0];
                       break;
                   case 2:
                       if (AllowRelation == false)
                           throw new Exception("Relation specifiers not permitted in field list: '" + Fields[i] + "'.");
                       Field.RelationName = FieldParts[0].Trim();
                       Field.FieldName = FieldParts[1].Trim();
                       break;
                   default:
                       throw new Exception("Invalid field definition: " + Fields[i] + "'.");
               }
               if (Field.FieldAlias == null)
                   Field.FieldAlias = Field.FieldName;
               m_FieldInfo.Add(Field);
           }
       }

   }

   class FieldInfo
   {
       public string RelationName;
       public string FieldName;    //源表的字段名；
       public string FieldAlias;    //查询结果表中的字段名，即需要查询字段的别名；
       public string Aggregate;
   }

}