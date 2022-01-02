using System;
using System.IO;

namespace Tools
{
    /// <summary>
    /// 日志对象
    /// </summary>
    public class LogTool
    {
        public string LogName { get; set; }
        public delegate void LogChangedHandler(string addLineString);
        public event LogChangedHandler OnLogChanged;
        readonly string _basePath = AppDomain.CurrentDomain.BaseDirectory + "\\Log";
        public string LogPath { get { return _basePath + "\\" + LogName + "\\" + DateTime.Now.ToString("yyyy-MM-dd") + ".txt"; } }

        public LogTool(string logName)
        {
            LogName = logName;
        }

        readonly object _lockObj = new object();
        public void WriteLine(string text)
        {
            try
            {
                lock (_lockObj)
                {
                    var dir = Path.GetDirectoryName(LogPath);
                    if (!Directory.Exists(dir))
                    {
                        Directory.CreateDirectory(dir);
                    }
                    if (!File.Exists(LogPath))
                    {
                        File.Create(LogPath).Dispose();
                    }
                    using (var sw = File.AppendText(LogPath))
                    {
                        sw.WriteLine(DateTime.Now + "\r\n" + text + "\r\n--------------------------------------------------------\r\n");
                    }
                    if (OnLogChanged != null) OnLogChanged(DateTime.Now + " : " + text + "\r\n");
                }
            }
            catch { }
        }
    }
}
