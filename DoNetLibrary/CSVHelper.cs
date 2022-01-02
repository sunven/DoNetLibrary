using System.Data;
using System.IO;
using System.Data.Odbc;
using System;
using System.Text;
using System.Collections.Generic;

namespace Common
{

    public static class CsvHelper
    {
        public static DataTable GetDataFromCSV(string file, bool HasHead = true, char[] split = null)
        {
            DataTable dt = new DataTable();
            if (split == null)
            {
                split = new char[] { '\t' };
            }
            using (StreamReader sw = new StreamReader(file, Encoding.Default))
            {
                string str = sw.ReadLine();
                string[] columns = str.Split(split);//默认第一行是column的name
                List<string> names = new List<string>();
                int i = 0;
                foreach (string name in columns)
                {
                    if (!string.IsNullOrEmpty(name))
                    {
                        names.Add(name);
                        dt.Columns.Add(name + "_" + i);
                        i++;
                    }
                }
                if (HasHead)
                {
                    dt.Rows.Add(names.ToArray());
                }
                string line = sw.ReadLine();
                while (line != null)
                {
                    string[] data = line.Split(split);
                    dt.Rows.Add(data);
                    line = sw.ReadLine();
                }
                sw.Close();
            }
            return dt;
        }
    }
}
