using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Security;

namespace Tools
{
    public static class StringTool
    {
        #region 加密

        /// <summary>
        /// MD5加密
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ToMD5(this string str)
        {
            return FormsAuthentication.HashPasswordForStoringInConfigFile(str, "MD5");
        }


        #region DES加密

        ///<summary>
        /// 使用默认密钥加密
        ///</summary>
        ///<param name="strText"></param>
        ///<returns></returns>
        public static string Encrypt(string strText)
        {
            return Encrypt(strText, "TSF");
        }

        ///<summary>
        /// 使用给定密钥加密
        ///</summary>
        ///<param name="strText"></param>
        ///<param name="sKey">密钥</param>
        ///<returns></returns>
        public static string Encrypt(string strText, string sKey)
        {
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            byte[] inputByteArray = Encoding.Default.GetBytes(strText);
            des.Key = ASCIIEncoding.ASCII.GetBytes(FormsAuthentication.HashPasswordForStoringInConfigFile(sKey, "md5").Substring(0, 8));
            des.IV = ASCIIEncoding.ASCII.GetBytes(FormsAuthentication.HashPasswordForStoringInConfigFile(sKey, "md5").Substring(0, 8));
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            StringBuilder ret = new StringBuilder();
            foreach (byte b in ms.ToArray())
            {
                ret.AppendFormat("{0:X2}", b);
            }
            return ret.ToString();
        }

        #endregion

        #region DES解密

        ///<summary>
        /// 使用默认密钥解密
        ///</summary>
        ///<param name="strText"></param>
        ///<returns></returns>
        public static string Decrypt(string strText)
        {
            return Decrypt(strText, "TSF");
        }

        ///<summary>
        /// 使用给定密钥解密
        ///</summary>
        ///<param name="strText"></param>
        ///<param name="sKey"></param>
        ///<returns></returns>
        public static string Decrypt(string strText, string sKey)
        {
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            int len = strText.Length / 2;
            byte[] inputByteArray = new byte[len];
            int x, i;
            for (x = 0; x < len; x++)
            {
                i = Convert.ToInt32(strText.Substring(x * 2, 2), 16);
                inputByteArray[x] = (byte)i;
            }
            des.Key = ASCIIEncoding.ASCII.GetBytes(FormsAuthentication.HashPasswordForStoringInConfigFile(sKey, "md5").Substring(0, 8));
            des.IV = ASCIIEncoding.ASCII.GetBytes(FormsAuthentication.HashPasswordForStoringInConfigFile(sKey, "md5").Substring(0, 8));
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            return Encoding.Default.GetString(ms.ToArray());
        }

        #endregion

        #endregion

        #region convert

        public static int[] ToIntArray(this string str, char separator)
        {
            var arrStr = str.Split(separator);
            return arrStr.Select(int.Parse).ToArray();
        }

        public static int GetArrSeq(this string[] arr, int value)
        {
            for (var i = 0; i < arr.Length; i++)
            {
                if (arr[i] == value.ToString())
                {
                    return i;
                }
            }
            return 0;
        }

        #endregion

        #region 剔除 截取

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

        public static string RemoveHtmlTag(this string htmlstring)
        {
            if (string.IsNullOrEmpty(htmlstring))
            {
                return htmlstring;
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
            htmlstring = Regex.Replace(htmlstring, @"<script[^>]*?>.*?</script>", "",
              RegexOptions.IgnoreCase);

            htmlstring = Regex.Replace(htmlstring, @"([\r\n])[\s]+", "",
              RegexOptions.IgnoreCase);
            htmlstring = Regex.Replace(htmlstring, @"-->", "", RegexOptions.IgnoreCase);
            htmlstring = Regex.Replace(htmlstring, @"<!--.*", "", RegexOptions.IgnoreCase);
            htmlstring = Regex.Replace(htmlstring, @"&(quot|#34);", "\"",
              RegexOptions.IgnoreCase);
            htmlstring = Regex.Replace(htmlstring, @"&(amp|#38);", "&",
              RegexOptions.IgnoreCase);
            htmlstring = Regex.Replace(htmlstring, @"&(lt|#60);", "<",
              RegexOptions.IgnoreCase);
            htmlstring = Regex.Replace(htmlstring, @"&(gt|#62);", ">",
              RegexOptions.IgnoreCase);
            htmlstring = Regex.Replace(htmlstring, @"&(nbsp|#160);", "   ",
              RegexOptions.IgnoreCase);
            htmlstring = Regex.Replace(htmlstring, @"&(iexcl|#161);", "\xa1",
              RegexOptions.IgnoreCase);
            htmlstring = Regex.Replace(htmlstring, @"&(cent|#162);", "\xa2",
              RegexOptions.IgnoreCase);
            htmlstring = Regex.Replace(htmlstring, @"&(pound|#163);", "\xa3",
              RegexOptions.IgnoreCase);
            htmlstring = Regex.Replace(htmlstring, @"&(copy|#169);", "\xa9",
              RegexOptions.IgnoreCase);
            htmlstring = Regex.Replace(htmlstring, @"&#(\d+);", "",
              RegexOptions.IgnoreCase);
            //删除HTML 
            htmlstring = Regex.Replace(htmlstring, @"<(.[^>]*)>", "",
              RegexOptions.IgnoreCase);
            //htmlstring.Replace("<", "");
            //htmlstring.Replace(">", "");
            //htmlstring.Replace("\r\n", "");
            //Htmlstring = HttpContext.Current.Server.HtmlEncode(Htmlstring).Trim();
            return htmlstring;
        }

        #endregion
    }
}
