using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Tools
{
    /// <summary>
    /// 类型转换工具
    /// </summary>
    public static class ConvertTool
    {
        /// <summary>
        /// DataTable转List
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static List<Dictionary<string, object>> ToList(this DataTable dt)
        {
            var list = new List<Dictionary<string, object>>();
            foreach (DataRow dr in dt.Rows)
            {
                var result = new Dictionary<string, object>();
                foreach (DataColumn dc in dt.Columns)
                {
                    result.Add(dc.ColumnName, dr[dc]);
                }
                list.Add(result);
            }
            return list;
        }

        /// <summary>
        /// DataRow转Dictionary
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        public static Dictionary<string, object> ToList(this DataRow dr)
        {
            var result = new Dictionary<string, object>();
            foreach (DataColumn dc in dr.Table.Columns)
            {
                result.Add(dc.ColumnName, dr[dc]);
            }
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dic"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static object GetValue(Dictionary<object, object> dic, object key)
        {
            var first = dic.FirstOrDefault(c => c.Key == key);
            return first.Value;
        }
    }
}
