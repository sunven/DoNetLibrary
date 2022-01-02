using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Runtime.Serialization;
using System.Linq;

namespace Common
{
    /// <summary> 
    /// 日志管理器
    /// 主要用于通用日志管理
    /// </summary>
    public class LogManager
    {
        static string logBasePath = AppDomain.CurrentDomain.BaseDirectory + "\\Log";
        /// <summary>
        /// 日志目录
        /// </summary>
        static public string LogBasePath
        {
            get
            {
                return logBasePath;
            }
            set
            {
                logBasePath = value;
            }
        }
        static object lockObj = new object();
        static List<Log> list = new List<Log>();
        public LogManager()
        {

        }
        static LogManager lm = new LogManager();
        static public LogManager Logs { get { return lm; } }

        public Log this[string logName]
        {
            get
            {
                lock (lockObj)
                {
                    if (LogManager.list.Any(p => p.LogName == logName))
                    {
                        return LogManager.list.Where(p => p.LogName == logName).Take(1).Single();
                    }
                    else
                    {
                        var log = new Log(logName);
                        LogManager.list.Add(log);
                        return log;
                    }
                }
            }
        }
        public void Add(Log log)
        {
            LogManager.list.Add(log);
        }
    }
    /// <summary>
    /// 日志对象
    /// </summary>
    public class Log
    {

        private string _logName;

        public string LogName
        {
            get { return _logName; }
            set { _logName = value; }
        }

        public delegate void LogChangedHandler(string AddLineString);
        public event LogChangedHandler OnLogChanged;
        string basePath = LogManager.LogBasePath;   
        public string BasePath { get { return basePath; } set { basePath = value; } }
        public string Path { get { return basePath + "\\" + LogName + "\\" + DateTime.Now.ToString("yyyy-MM-dd") + ".txt"; } }

        public Log(string logName)
        {
            this.LogName = logName;

        }

        public  string GetLogText()
        {
            lock (lockObj)
            {
                string text = "";
                try { text = File.ReadAllText(Path, Encoding.UTF8); }
                catch { }
                return text;
            }
        }
        object lockObj = new object();
        public  void WriteLine(string text)
        {
            try {
                lock (lockObj)
                {
                    if (!Directory.Exists(System.IO.Path.GetDirectoryName(Path)))
                    {
                        Directory.CreateDirectory(System.IO.Path.GetDirectoryName(Path));
                    }
                    if (!File.Exists(Path))
                    {
                        File.Create(Path).Dispose();
                    }
                    using (var sw = File.AppendText(Path))
                    {
                        sw.WriteLine(DateTime.Now.ToString() + " : " + text);
                    }
                    if (OnLogChanged != null) OnLogChanged(DateTime.Now.ToString() + " : " + text + "\r\n");
                }
            }
            catch { }         
        }
    }
}
