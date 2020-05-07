using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Web;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;

namespace WCFPGMSFront
{   
    public class GeneralFunction
    {
        string strDatabaseServer = System.Configuration.ConfigurationManager.AppSettings["strDatabaseServer"];
        string strDatabaseName = System.Configuration.ConfigurationManager.AppSettings["strDatabaseName"];
        string strDBUserId = System.Configuration.ConfigurationManager.AppSettings["strDBUserId"];
        string strDBUserPassword = System.Configuration.ConfigurationManager.AppSettings["strDBUserPassword"];

        public string StrSetConnection()
        {
            string MyString = "Database=" + strDatabaseName + ";Server=" + strDatabaseServer + ";User id=" + strDBUserId + ";password=" + strDBUserPassword + ";Connect Timeout=8000000";
            return MyString;
        }
        
        //public void SetParameter(Database db, DataRow row, System.Data.Common.DbCommand commandName, DataColumn col, SecurityInfo secuinfo)
        //{
        //    string str = col.Caption.ToString().Trim();
        //    if (str.Length <= 2)
        //        str = str + "modified";

        //    if (str.Substring(0, 2).ToUpper() != "ZZ" && col.Caption.ToString().Trim().ToUpper() != "DUMMY" && col.Caption.ToString().Trim().ToUpper() != "ZZDumSeq")
        //    {
        //        if ((col.ColumnName.ToUpper() == "UPDATEDATE") || (col.ColumnName.ToUpper() == "UPDATEID")) //(col.ColumnName.ToUpper() == "DBID") ||
        //        {
        //            switch (col.ColumnName.ToString().ToUpper())
        //            {
        //                case "UPDATEID":
        //                    db.AddInParameter(commandName, "UpdateId", DbType.Int32, secuinfo.UserId);
        //                    break;
        //                case "UPDATEDATE":
        //                    db.AddInParameter(commandName, "UpdateDate", DbType.DateTime, DateTime.Now);
        //                    break;
        //            }
        //        }
        //        else
        //        {

        //            switch (col.DataType.ToString())
        //            {
        //                case "System.Int64":
        //                    db.AddInParameter(commandName, col.ColumnName.ToString(), DbType.Int64, row[col.ColumnName]);
        //                    break;
        //                case "System.Int32":
        //                    db.AddInParameter(commandName, col.ColumnName.ToString(), DbType.Int32, row[col.ColumnName]);
        //                    break;
        //                case "System.Decimal":
        //                    db.AddInParameter(commandName, col.ColumnName.ToString(), DbType.Decimal, row[col.ColumnName]);
        //                    break;
        //                case "System.String":
        //                    db.AddInParameter(commandName, col.ColumnName.ToString(), DbType.String, row[col.ColumnName]);
        //                    break;
        //                case "System.Double":
        //                    db.AddInParameter(commandName, col.ColumnName.ToString(), DbType.Double, row[col.ColumnName]);
        //                    break;
        //                case "System.Byte[]":
        //                    db.AddInParameter(commandName, col.ColumnName.ToString(), DbType.Binary, row[col.ColumnName]);
        //                    break;
        //                case "System.Boolean":
        //                    db.AddInParameter(commandName, col.ColumnName.ToString(), DbType.Byte, row[col.ColumnName]);
        //                    break;
        //                case "System.DateTime":
        //                    db.AddInParameter(commandName, col.ColumnName.ToString(), DbType.DateTime, row[col.ColumnName]);
        //                    break;
        //                case "System.Time":
        //                    db.AddInParameter(commandName, col.ColumnName.ToString(), DbType.Time, row[col.ColumnName]);
        //                    break;
        //            }
        //        }
        //    }
        //}

        //public DataSet RowAdd(int intStatus, string strRemarks, int intVNoMax, int intVNoMin, SecurityInfo secuinfo)
        //{

        //    DataSet dsRow = new DataSet();
        //    Database dbRow = new SqlDatabase(StrSetConnection(secuinfo));
        //    System.Data.Common.DbCommand CmdGetRow = null;
        //    CmdGetRow = dbRow.GetStoredProcCommand("Settings.RecordStatusGetStatus", 0);
        //    dbRow.LoadDataSet(CmdGetRow, dsRow, new string[] { "Status" });
        //    DataRow dr = dsRow.Tables["Status"].NewRow();
        //    dr["Status"] = intStatus;
        //    dr["Remarks"] = strRemarks;
        //    dr["VNoMax"] = intVNoMax;
        //    dr["VNoMin"] = intVNoMin;
        //    dr["Date"] = DateTime.Today;
        //    dsRow.Tables["Status"].Rows.Add(dr);

        //    return dsRow;
        //}

    }
}