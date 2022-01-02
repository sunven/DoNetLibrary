using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
namespace Common
{
    public class Serialize
    {
        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string SerializeObject(object o)
        {
            System.Runtime.Serialization.IFormatter obj = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
            MemoryStream ms = new MemoryStream();
            obj.Serialize(ms, o);
            BinaryReader br = new BinaryReader(ms);
            ms.Position = 0;
            byte[] bs = br.ReadBytes((int)ms.Length);
            ms.Close();
            return Convert.ToBase64String(bs);
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static object DeserializeObject(string str)
        {
            byte[] bs = Convert.FromBase64String(str);
            System.Runtime.Serialization.IFormatter obj = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
            MemoryStream ms = new MemoryStream();
            ms.Write(bs, 0, bs.Length);
            ms.Position = 0;
            object o = obj.Deserialize(ms);
            ms.Close();
            return o;
        }
    }
}
