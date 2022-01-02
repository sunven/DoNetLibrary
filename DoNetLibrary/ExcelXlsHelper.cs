using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.OleDb;

namespace Tools
{
    public class ExcelXlsHelper
    {
        public static DataTable loadData(string strExcelFileName, string strSheetName, bool ContainsTitle = false)
        {
            string strConn = "Provider=Microsoft.Jet.OLEDB.4.0;" + "Data Source=" + strExcelFileName + ";" + "Extended Properties='Excel 8.0;HDR=NO;IMEX=1';";
            string strExcelSql = string.Format("select * from [{0}$]", strSheetName);
            System.Data.DataSet ds = new DataSet();
            OleDbConnection conn = new OleDbConnection(strConn);
            OleDbDataAdapter adapter = new OleDbDataAdapter(strExcelSql, strConn);
            adapter.Fill(ds, "mytable");
            if (ds != null && ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0 && ContainsTitle)
            {
                ds.Tables[0].Rows.RemoveAt(0);
            }
            return ds.Tables[0];
        }
    }
}
