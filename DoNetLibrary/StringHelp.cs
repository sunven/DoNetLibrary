using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;

namespace Common
{
    /// <summary>
    ///StringHelp 的摘要说明
    /// </summary>
    public static class StringHelp
    {
        static StringHelp()
        {
            //
            //TODO: 在此处添加构造函数逻辑
            //
        }

        /// <summary>
        /// 截取字符串
        /// 截取n个字节（一个中文2个字节，英文1个字节）
        /// </summary>
        /// <param name="str">原字符窜</param>
        /// <param name="length">要截取的长度</param>
        /// <returns></returns>
        public static string Cut1(this string str, int length)
        {

            if (System.Text.Encoding.Default.GetByteCount(str) <= length)
            {
                return str;
            }
            else
            {
                //超出长度的索引处
                int index = 0;
                //循环判断字符串中的每一个字符    i：字节长度
                for (int i = 0; i < length; index++)
                {
                    try
                    {
                        //如果当前字符是中文+2否则+1
                        i += Regex.IsMatch(str[index].ToString(), @"[\u4e00-\u9fa5]") ? 2 : 1;  //使用正则表达式判断是否全角
                    }
                    catch (Exception)
                    {
                        break;
                    }
                }
                return str.Substring(0, index - 1);
                ///注意，该算法可能有一个字节的误差:
                ///当最后一个文字是正文时会多出1个字节，当最后一个是英文时则刚好
                ///如果宁可少，不能多的情况，可以调整Substring 中 --index
                ///或者另外判断最后一个字符
            }
        }
        /// <summary>
        /// 截取指定字节长度的字符串
        /// </summary>
        /// <param name="str">原字符串</param>
        /// <param name="len">截取字节长度</param>
        /// <returns></returns>
        public static string Cut(this string str, int len)
        {
            string result = string.Empty;//最终返回结果
            if (string.IsNullOrEmpty(str)) { return result; }
            int byteLen = System.Text.Encoding.Default.GetByteCount(str);//单字节字符长度
            int charLen = str.Length;//把字符平等对待时的字符串长度
            int byteCount = 0;//记录读取进度
            int pos = 0;//记录截取位置
            if (byteLen > len)
            {
                for (int i = 0; i < charLen; i++)
                {
                    if (Convert.ToInt32(str.ToCharArray()[i]) > 255)//按中文字符计算加2
                    {
                        byteCount += 2;
                    }
                    else//按英文字符计算加1
                    {
                        byteCount += 1;
                    }
                    if (byteCount > len)//超出时只记下上一有效位置
                    {
                        pos = i;
                        break;
                    }
                    else if (byteCount == len)//记下当前位置
                    {
                        pos = i + 1;
                        break;
                    }
                }
            }
            if (pos > 0)
            {
                result = str.Substring(0, pos) + "...";
            }
            else
            {
                result = str;

            }
            return result;
        }
        /// <summary>
        /// 截取指定字节长度的字符串
        /// </summary>
        /// <param name="str">原字符串</param>
        /// <param name="startindex">起始位置</param>
        /// <param name="len">截取字节长度</param>
        /// <returns></returns>
        public static string Cut2(this string str, int startindex, int len)
        {
            string result = string.Empty;//最终返回的结果
            if (string.IsNullOrEmpty(str))
            {
                return result;
            }
            int byteLen = System.Text.Encoding.Default.GetByteCount(str);//单字节字符长度
            int charLen = str.Length;//把字符平等对待时的字符串长度
            if (startindex == 0)
            {
                return Cut1(str, len);
            }
            else if (startindex >= byteLen)
            {
                return result;
            }
            else
            {
                int AllLen = startindex + len;
                int byteCountStart = 0;//记录读取进度
                int byteCountEnd = 0;//记录读取进度
                int startpos = 0;//记录截取位置
                int endpos = 0;//记录截取位置
                for (int i = 0; i < charLen; i++)
                {
                    if (Convert.ToInt32(str.ToCharArray()[i]) > 255)//按照中文字符计算加2
                    {
                        byteCountStart = +2;
                    }
                    else
                    {
                        byteCountStart = +1;
                    }
                    if (byteCountStart > startindex)//超出时只记下上一有效位置
                    {
                        startpos = i;
                        AllLen = startindex + len - 1;
                        break;
                    }
                    else if (byteCountStart == startindex)//记下当前位置
                    {
                        startpos = i + 1;
                        break;
                    }
                }
                if (startindex + len <= byteLen)//截取字符在总长以内
                {
                    for (int i = 0; i < charLen; i++)
                    {
                        if (Convert.ToInt32(str.ToCharArray()[i]) > 255)
                        {
                            byteCountEnd += 2;
                        }
                        else
                        {
                            byteCountEnd += 1;
                        }
                        if (byteCountEnd > AllLen)//超出时只记下上一有效位置
                        {
                            endpos = i;
                            break;
                        }
                        else if (byteCountEnd == AllLen)//记下当前位置
                        {
                            endpos = i + 1;
                            break;
                        }
                    }
                    endpos = endpos - startpos;
                }
                else if (startindex + len > byteLen)//截取字符超出总长
                {
                    endpos = charLen - startpos;
                }
                if (endpos >= 0)
                {
                    result = str.Substring(startpos, endpos);
                }
                return result;
            }
        }
        /// <summary>
        /// string 扩展方法
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static int CnLen(this string str)
        {
            if (string.IsNullOrEmpty(str)) return 0;
            int byteLen = System.Text.Encoding.Default.GetByteCount(str);
            if (byteLen % 2 == 0) return byteLen / 2;
            return Convert.ToInt32(Math.Floor(Convert.ToDouble(byteLen / 2)) + 1);
        }
    }
}