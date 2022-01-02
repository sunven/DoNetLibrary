using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Web.Configuration;
using System.Web;

namespace Common
{
    public class ConfigHelper1
    {

        private Configuration config;
        private string Path;
        public ConfigHelper1()
        {
            Path = System.Web.HttpContext.Current.Request.ApplicationPath;
            config = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration(Path);
        }
        public ConfigHelper1(string path)
        {
            config = ConfigurationManager.OpenExeConfiguration(path);
            Path = path;
        }
        public string GetConfigString(string key)
        {
            object objModel = ConfigurationManager.AppSettings[key];
            return objModel.ToString();
        }
        public int GetConfigInt(string key)
        {
            object objModel = ConfigurationManager.AppSettings[key];
            return Int32.Parse(objModel.ToString());
        }
        public string GetSqlConnectionString(string key)
        {
            string connectionString = ConfigurationManager.ConnectionStrings[key].ConnectionString.ToString();
            return connectionString;
        }

        public bool SetAppSetting(string key, string value)
        {
            string name = "appSettings";
            try
            {
                AppSettingsSection appSection = (AppSettingsSection)config.GetSection(name);
                if (appSection.Settings[key] == null)
                {
                    appSection.Settings.Add(key, value);
                    config.Save();
                    return true;
                }
                else
                {
                    appSection.Settings.Remove(key);
                    appSection.Settings.Add(key, value);
                    config.Save();
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
