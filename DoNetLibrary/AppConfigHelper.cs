﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Common
{
    public class AppConfigHelper
    {
        public static string rootPath
        {
            get
            {
                string appPath = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
                return appPath;
            }
        }
        public static string WebDomain
        {
            get
            {
                string AppPath = "";
                HttpContext HttpCurrent = HttpContext.Current;
                HttpRequest Req;
                if (HttpCurrent != null)
                {
                    Req = HttpCurrent.Request;

                    string UrlAuthority = Req.Url.GetLeftPart(UriPartial.Authority);
                    if (Req.ApplicationPath == null || Req.ApplicationPath == "/")
                        //直接安装在   Web   站点 
                        AppPath = UrlAuthority;
                    else
                        //安装在虚拟子目录下 
                        AppPath = UrlAuthority + Req.ApplicationPath;
                }
                return AppPath;
            }
        }
    }
}
