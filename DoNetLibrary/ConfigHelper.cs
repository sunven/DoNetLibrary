using System;
using System.Configuration;

namespace Common
{
    public class ConfigHelper
    {

        private Configuration config;
        private string Path;
        public ConfigHelper()
        {
            Path = System.Web.HttpContext.Current.Request.ApplicationPath;
            config = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration(Path);
        }
        public ConfigHelper(string path)
        {
            config = ConfigurationManager.OpenExeConfiguration(path);
            Path = path;
        }
        public static string GetConfigString(string key)
        {
            object objModel = ConfigurationManager.AppSettings[key];
            return objModel.ToString();
        }
        public static int GetConfigInt(string key)
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
