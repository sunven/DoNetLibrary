using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Caching;

namespace Common
{
    public class CacheHelper
    {
        public static List<string> AllCacheKey = new List<string>();

        /// <summary> 
        /// 添加缓存 
        /// </summary> 
        /// <param name="key"></param> 
        /// <param name="value"></param> 
        /// <param name="absoluteExpiration">DateTime.Now.AddMinutes(20)</param> 
        public static void addCache(string key, object value, DateTime absoluteExpiration)
        {
            key = key.ToLower();
            if (AllCacheKey.Contains(key))
            {
                removeCache(key);
            }
            AllCacheKey.Add(key);
            Cache objC = HttpRuntime.Cache;
            objC.Add(key, value, null, absoluteExpiration, TimeSpan.Zero, System.Web.Caching.CacheItemPriority.Normal, null);
        }

        /// <summary>
        /// 获取当前应用程序指定CacheKey的Cache值
        /// </summary>
        /// <param name="CacheKey"></param>
        /// <returns></returns>
        public static object getCache(string CacheKey)
        {
            CacheKey = CacheKey.ToLower();
            Cache objCache = HttpRuntime.Cache;
            object cache = objCache[CacheKey];
            if (cache != null)
            {
                debug(CacheKey);
            }
            return cache;
        }
        /// <summary> 
        /// 移除缓存 
        /// </summary> 
        /// <param name="key"></param> 
        public static void removeCache(string key)
        {
            key = key.ToLower();
            Cache objCache = HttpRuntime.Cache;
            if (AllCacheKey.Contains(key))
            {
                AllCacheKey.Remove(key);
                objCache.Remove(key);
            }
        }
        /// <summary> 
        /// 清空使用的缓存 
        /// </summary> 
        public static void clearCache()
        {
            Cache objCache = HttpRuntime.Cache;
            foreach (string value in AllCacheKey)
            {
                objCache.Remove(value);
            }
            AllCacheKey.Clear();
        }
        /// <summary>
        /// 清除以BeginStr打头的缓存
        /// </summary>
        /// <param name="BeginStr"></param>
        public static void clearCache(string BeginStr)
        {
            BeginStr = BeginStr.ToLower();
            Cache objCache = HttpRuntime.Cache;
            for (int i = 0; i < AllCacheKey.Count; i++)
            {
                string value = AllCacheKey[i];
                if (value.Contains(BeginStr) && value.Length >= BeginStr.Length)
                {
                    string subStr = value.Substring(0, BeginStr.Length);
                    if (subStr == BeginStr)
                    {
                        objCache.Remove(value);
                        AllCacheKey.Remove(value);
                        i--;
                    }
                }
            }
        }
        private static void debug(string str)
        {
           // HttpContext.Current.Response.Write(str);
           // Console.WriteLine(str);
            //HttpContext.Current.Response.End();
        }
    }
}
