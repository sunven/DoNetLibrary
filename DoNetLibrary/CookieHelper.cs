using System;
using System.Collections.Generic;
using System.Web;
using System.Linq;
using System.Text;


namespace Common
{

    public class CookieHelper
    {
        /// <summary>
        /// 创建cookie
        /// </summary>
        /// <param name="CookieName">cookie名称</param>
        /// <param name="ValueObj">字符串或类对象</param>
        /// <param name="Domain">huanrong2010.com</param>
        /// <param name="Days">过期时间</param>
        /// <returns></returns>
        public static HttpCookie setCookie(string CookieName, object ValueObj, string Domain = null)
        {
            HttpCookie cookie = new HttpCookie(CookieName, Common.Serialize.SerializeObject(ValueObj));
            if (!string.IsNullOrEmpty(Domain))
            {
                cookie.Domain = Domain;
            }
            HttpContext.Current.Response.Cookies.Add(cookie);
            return cookie;
        }


        public static HttpCookie setCookie(string CookieName, object ValueObj, DateTime Expires, string Domain = null)
        {
            HttpCookie cookie = new HttpCookie(CookieName, Common.Serialize.SerializeObject(ValueObj));

            cookie.Expires = Expires;

            if (!string.IsNullOrEmpty(Domain))
            {
                cookie.Domain = Domain;
            }
            HttpContext.Current.Response.Cookies.Add(cookie);
            return cookie;
        }












        /// <summary>
        /// 获取cookie
        /// </summary>
        /// <param name="CookieName">cookie名称</param>
        /// <returns></returns>
        public static object getCookie(string CookieName)
        {
            object re = null;
            if (HttpContext.Current.Request.Cookies[CookieName] != null)
            {
                string value = HttpContext.Current.Request.Cookies[CookieName].Value;
                re = Common.Serialize.DeserializeObject(value);
            }
            return re;
        }

        public static void removeCookie(string CookieName)
        {
            HttpCookie myCookie = new HttpCookie(CookieName);
            DateTime now = DateTime.Now;
            myCookie.Expires = now.AddYears(-2);
            HttpContext.Current.Response.Cookies.Add(myCookie);

        }

    }
}
