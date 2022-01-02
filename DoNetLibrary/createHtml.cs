using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;

namespace Common
{
    public class createHtml
    {
        public delegate void Target(object obj);
        public static bool Create(string oldPage, string newPage, Target errorTarget = null, string encode = null)
        {
            if (string.IsNullOrEmpty(encode))
            {
                encode = "utf-8";
            }
            DirectoryInfo oDir = new DirectoryInfo(Path.GetDirectoryName(newPage));
            if (!oDir.Exists)
            {
                oDir.Create();
            }
            bool res = false;
            Encoding code = Encoding.GetEncoding(encode);
            StreamReader sr = null;
            StreamWriter sw = null;
            string str = null;
            //读取远程路径
            WebRequest temp = WebRequest.Create(oldPage);
            WebResponse myTemp = temp.GetResponse();
            sr = new StreamReader(myTemp.GetResponseStream(), code);
            try
            {
                sr = new StreamReader(myTemp.GetResponseStream(), code);
                str = sr.ReadToEnd();
            }
            catch (Exception ex)
            {
                if (errorTarget != null)
                {
                    errorTarget(ex);
                }
                //  addLog(ex, "AppError");
            }
            finally
            {
                sr.Close();
            }
            //写入
            try
            {
                sw = new StreamWriter(newPage, false, code);
                sw.Write(str);
                sw.Flush();
                res = true;
            }
            catch (Exception ex)
            {
                if (errorTarget != null)
                {
                    errorTarget(ex);
                }
            }
            finally
            {
                if (sw != null)
                {
                    sw.Close();
                }
            }
            return res;
        }
    }
}
