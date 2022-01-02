using System;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Web;

namespace Common
{
    public static class Util
    {
        public static string EmumText<T>(int value)
        {
            return EmumText<T>(value);
        }

        public static string EmumText<T>(string value)
        {
            var text = string.Empty;
            foreach (var item in Enum.GetValues(typeof(T)).Cast<Enum>().Where(item => value == item.ToString("D")))
            {
                text = item.ToString();
                break;
            }
            return text;
        }

        public static string CutStr(this string strInput, int length, bool isAddPoint = true)
        {
            if (string.IsNullOrEmpty(strInput))
            {
                return strInput;
            }
            if (!Regex.IsMatch(strInput, "[^\x00-\xff]", RegexOptions.IgnoreCase))
            {
                if (strInput.Length < length)
                    return strInput;
                else
                    return strInput.Substring(0, length) + (isAddPoint ? "..." : "");
            }
            else
            {
                string checkstr = Regex.Replace(strInput, "[^\x00-\xff]", "**", RegexOptions.IgnoreCase);
                if (checkstr.Length < length)
                    return strInput;
                else
                {
                    string strOut = "";
                    int strLength = 0;
                    for (int i = 0; i < strInput.Length; i++)
                    {
                        if (strLength >= length)
                            break;
                        strOut += strInput.Substring(i, 1);
                        if (!Regex.IsMatch(strInput.Substring(i, 1), "[^\x00-\xff]", RegexOptions.IgnoreCase))
                            strLength += 1;
                        else
                            strLength += 2;
                    }
                    return strOut + (isAddPoint ? "..." : "");
                }
            }
        }

        public static string GetImgSrc(this string content)
        {
            if (string.IsNullOrEmpty(content))
            {
                return "";
            }
            var rg = new Regex(@"<img\s+[^>]*\s*src\s*=\s*([']?)(?<url>\S+)'?[^>]*>", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            MatchCollection mc = rg.Matches(content);
            if (mc.Count > 0)
            {
                return mc[0].Result("${url}").Replace("\"", "");
            }
            return "";
        }

        public static MatchCollection GetImg(this string content)
        {
            if (string.IsNullOrEmpty(content))
            {
                return null;
            }
            var rg = new Regex(@"<img\s+[^>]*\s*src\s*=\s*([']?)(?<url>\S+)'?[^>]*>", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            MatchCollection mc = rg.Matches(content);
            return mc;
        }

        public static string RemoveHtmlTag(this string Htmlstring)
        {
            if (string.IsNullOrEmpty(Htmlstring))
            {
                return Htmlstring;
            }
            //str = Regex.Replace(str, @"<!--(.|\n)*?-->", "", RegexOptions.IgnoreCase);                  //剔除注释
            //str = Regex.Replace(str, @"<link[^<>]*?>", "", RegexOptions.IgnoreCase);                    //剔除样式链接文件
            //str = Regex.Replace(str, @"<style[^<>]*?>(.|\n)*?</style>", "", RegexOptions.IgnoreCase);   //剔除样式代码块
            //str = Regex.Replace(str, @"<script[^<>]*?>(.|\n)*?</script>", "", RegexOptions.IgnoreCase); //剔除javasript代码
            //str = Regex.Replace(str, @"<iframe[^<>]*?>(.|\n)*?</iframe>", "", RegexOptions.IgnoreCase); //剔除iframe代码块
            //str = Regex.Replace(str, @"<frameset[\s\S]+</frameset *>", "", RegexOptions.IgnoreCase); //剔除frameset代码块

            //str = Regex.Replace(str, @"<p[^>]*>", "<p>", RegexOptions.IgnoreCase);                      //格式化<p>
            //str = Regex.Replace(str, @"<div[^>]*?>", "<p>", RegexOptions.IgnoreCase);                   //格式化<div>为<p>
            //str = Regex.Replace(str, @"</div>", "</p>", RegexOptions.IgnoreCase);


            ////str = Regex.Replace(str, @"<((?!/?\b(p|a|img)\b)[^>])*>", "", RegexOptions.IgnoreCase);//需保留标签
            //str = Regex.Replace(str, @"<((?!/?\b(p|a)\b)[^>])*>", "", RegexOptions.IgnoreCase);//需保留标签
            //str = Regex.Replace(str, @"[\r|\n|\t]", "");  //换行
            //str = Regex.Replace(str, @"<p>(&nbsp;|\s)*?</p>", "", RegexOptions.IgnoreCase);  //剔除空标签
            //str = Regex.Replace(str, @"<a[^>]*>(&nbsp;|\s)*?</a>", "", RegexOptions.IgnoreCase);  //剔除空标签
            //str = Regex.Replace(str, "style=\".*?\"", "");
            //str = Regex.Replace(str, "id=\".*?\"", "");
            //str = Regex.Replace(str, "class=\".*?\"", "");
            //str = Regex.Replace(str, @"</p>", "</p>\n");
            //return str; public static string StripHTML(string strHtml)
            //删除脚本 
            Htmlstring = Regex.Replace(Htmlstring, @"<script[^>]*?>.*?</script>", "",
              RegexOptions.IgnoreCase);
           
            Htmlstring = Regex.Replace(Htmlstring, @"([\r\n])[\s]+", "",
              RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"-->", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"<!--.*", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(quot|#34);", "\"",
              RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(amp|#38);", "&",
              RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(lt|#60);", "<",
              RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(gt|#62);", ">",
              RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(nbsp|#160);", "   ",
              RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(iexcl|#161);", "\xa1",
              RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(cent|#162);", "\xa2",
              RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(pound|#163);", "\xa3",
              RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(copy|#169);", "\xa9",
              RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&#(\d+);", "",
              RegexOptions.IgnoreCase);
            //删除HTML 
            Htmlstring = Regex.Replace(Htmlstring, @"<(.[^>]*)>", "",
              RegexOptions.IgnoreCase);
            Htmlstring.Replace("<", "");
            Htmlstring.Replace(">", "");
            Htmlstring.Replace("\r\n", "");
            //Htmlstring = HttpContext.Current.Server.HtmlEncode(Htmlstring).Trim();

            return Htmlstring;
        }
    }



    public class Parameter
    {
        public int ParmInt1 { get; set; }
        public int ParmInt2 { get; set; }

        public string ParmStr1 { get; set; }
        public string ParmStr2 { get; set; }

        public int[] IntArr2 { get; set; }
    }
}
