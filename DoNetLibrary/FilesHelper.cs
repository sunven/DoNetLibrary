using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Common
{
    public class FilesHelper
    {
        public static List<FileInfo> getFiles(string fileName, DirectoryInfo dir,  List<FileInfo> resFiles)
        {
            FileInfo[] info = dir.GetFiles(fileName);
            if (info.Length > 0)
            {
                foreach (var o in info)
                {
                    resFiles.Add(o);
                }
            }
            foreach (DirectoryInfo d in dir.GetDirectories())
            {
                getFiles(fileName, d, resFiles);
            }
            return resFiles;

        }
        public static void MoveDirectoryTo(string directorySource, string directoryTarget)
        {
            //检查是否存在目的目录  
            if (!Directory.Exists(directoryTarget))
            {
                Directory.CreateDirectory(directoryTarget);
            }
            //先来移动文件  
            DirectoryInfo directoryInfo = new DirectoryInfo(directorySource);
            FileInfo[] files = directoryInfo.GetFiles();
            //移动所有文件  
            foreach (FileInfo file in files)
            {
                //如果自身文件在运行，不能直接覆盖，需要重命名之后再移动  
                if (File.Exists(Path.Combine(directoryTarget, file.Name)))
                {
                    if (File.Exists(Path.Combine(directoryTarget, file.Name + ".bak")))
                    {
                        File.Delete(Path.Combine(directoryTarget, file.Name + ".bak"));
                    }
                    File.Move(Path.Combine(directoryTarget, file.Name), Path.Combine(directoryTarget, file.Name + ".bak"));

                }
                file.MoveTo(Path.Combine(directoryTarget, file.Name));

            }
            //最后移动目录  
            DirectoryInfo[] directoryInfoArray = directoryInfo.GetDirectories();
            foreach (DirectoryInfo dir in directoryInfoArray)
            {
                MoveDirectoryTo(Path.Combine(directorySource, dir.Name), Path.Combine(directoryTarget, dir.Name));
            }
        }

        public static void DeleteEmptyDirectory(string path)
        {
            string[] dirs = Directory.GetDirectories(path);
            for (int i = 0; i < dirs.Length; i++)
            {
                DeleteEmptyDirectory(dirs[i]);
            }
            try
            {
                Directory.Delete(path);
            }
            catch (Exception)
            {
            }
        }
      
    }

}
