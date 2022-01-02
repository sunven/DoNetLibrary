using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
namespace Common
{
    public class RegexHelper
    {
        /// <summary>
        ///1、15位或18位，如果是15位，必需全是数字。
        ///2、如果是18位，最后一位可以是数字或字母Xx，其余必需是数字。
        /// </summary>
        const string idCard = @"/^(\d{15}$|^\d{18}$|^\d{17}(\d|X|x))$/";

        public static List<LinkModel> getLinks(string str)
        {
            List<LinkModel> links = new List<LinkModel>();
            Regex regex = new Regex("<a(?:\\s+.+?)*?\\s+href=[\"']([^\"]*?)[\"'].*?>(.*?)</a>");
            MatchCollection ms = regex.Matches(str);
            foreach (Match m in ms)
            {
                string href = m.Groups[1].Value;
                string text = m.Groups[2].Value;
                links.Add(new LinkModel()
                {
                    Href = href,
                    Text = text
                });
            }

            return links;
        }


        public static string ReplaceTag(string str, string tag, string attrName, string attrValue, string replacement)
        {
            string reg = string.Format(@"(?is)<{0}[^>]*?{1}=(['""\s]?){2}\1[^>]*?>(((?!</?{0}).)*</{0}>)?", tag, attrName, attrValue);
            return Regex.Replace(str, reg, replacement);
        }
        public static string RemoveTag(string str, string tag, string attrName, string attrValue)
        {
            return ReplaceTag(str, tag, attrName, attrValue, "");
        }


        public static string getEle(string str, string tag, string attrName)
        {
            string reg = string.Format("<{0}(?:\\s+.+?)*?\\s+{1}=[\"']([^\"]*?)[\"'].*?>(.*?)</{0}>", tag, attrName);
            //HttpContext.Current.Response.Write(str + "");
            //  HttpContext.Current.Response.Write(reg+"</br>");
            Match m = Regex.Match(str, reg);

            return m.Groups[1].Value;
        }

        public static string[] GetImgSrc(string html)
        {
            // 定义正则表达式用来匹配 img 标签 
            var regImg = new Regex(@"<img\b[^<>]*?\bsrc[\s\t\r\n]*=[\s\t\r\n]*[""']?[\s\t\r\n]*(?<imgUrl>[^\s\t\r\n""'<>]*)[^<>]*?/?[\s\t\r\n]*>", RegexOptions.IgnoreCase);
            // 搜索匹配的字符串 
            MatchCollection matches = regImg.Matches(html);
            int i = 0;
            string[] sUrlList = new string[matches.Count];
            // 取得匹配项列表 
            foreach (Match match in matches)
            {
                sUrlList[i++] = match.Groups["imgUrl"].Value;
            }
            return sUrlList;
        }


        public class LinkModel
        {
            public string Href { set; get; }
            public string Text { set; get; }
        }


    }
}
