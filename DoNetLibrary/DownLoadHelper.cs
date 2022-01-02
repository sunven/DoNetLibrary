using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
namespace Common
{
    public class DownLoadHelper
    {
        public static bool DownLoadFile(string _FileName)
        {
            try
            {
                System.IO.FileStream fs = System.IO.File.OpenRead(_FileName);
                byte[] FileData = new byte[fs.Length];
                fs.Read(FileData, 0, (int)fs.Length);
                HttpContext.Current.Response.Clear();
                HttpContext.Current.Response.AddHeader("Content-Type", "application/notepad");
                string FileName = System.Web.HttpUtility.UrlEncode(System.Text.Encoding.UTF8.GetBytes(_FileName));
                HttpContext.Current.Response.AddHeader("Content-Disposition", "inline;filename=" + System.Convert.ToChar(34) + FileName + System.Convert.ToChar(34));
                HttpContext.Current.Response.AddHeader("Content-Length", fs.Length.ToString());
                HttpContext.Current.Response.BinaryWrite(FileData);
                fs.Close();
                HttpContext.Current.Response.Flush();
                HttpContext.Current.Response.End();
                return true;
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
                return false;
            }
        }
    }
}
