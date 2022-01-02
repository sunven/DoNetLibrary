using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Tools
{
    public class XmlTool
    {
        #region 1

        public static T XmlStrToObject<T>(string xmlStr)
        {
            using (StringReader rdr = new StringReader(xmlStr))
            {
                XmlSerializer ser = new XmlSerializer(typeof(T));
                return (T)ser.Deserialize(rdr);
            }
        }

        public static T XmlToObject<T>(XmlDocument xdoc)
        {
            XmlSerializer ser = new XmlSerializer(typeof(T));
            return (T)ser.Deserialize(new XmlNodeReader(xdoc.DocumentElement));
        }

        public static string ToXmlString<T>(T t)
        {
            using (var stream = new MemoryStream())
            {
                var settings = new XmlWriterSettings();
                //去除XML声明：顶部的 <?xml version="1.0" encoding="utf-8"?>
                settings.OmitXmlDeclaration = true;
                settings.Encoding = Encoding.Default;
                //MemoryStream mem = new MemoryStream();
                using (XmlWriter writer = XmlWriter.Create(stream, settings))
                {
                    //去除默认命名空间xmlns:xsd和xmlns:xsi
                    XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
                    ns.Add(string.Empty, string.Empty);
                    XmlSerializer formatter = new XmlSerializer(typeof(T));
                    formatter.Serialize(writer, t, ns);
                    return Encoding.Default.GetString(stream.ToArray());
                }
            }
        }

        #endregion

        #region 2

        private static ConcurrentDictionary<Type, XmlSerializer> _cache;
        private static XmlSerializerNamespaces _defaultNamespace;

        static XmlTool()
        {
            _defaultNamespace = new XmlSerializerNamespaces();
            _defaultNamespace.Add(string.Empty, string.Empty);
            _cache = new ConcurrentDictionary<Type, XmlSerializer>();
        }

        private static XmlSerializer GetSerializer<T>()
        {
            var type = typeof(T);
            return _cache.GetOrAdd(type, XmlSerializer.FromTypes(new[] { type }).FirstOrDefault());
        }

        public static string XmlSerialize<T>(T obj)
        {
            using (var memoryStream = new MemoryStream())
            {
                GetSerializer<T>().Serialize(memoryStream, obj, _defaultNamespace);
                return Encoding.UTF8.GetString(memoryStream.GetBuffer());
            }
        }

        public static T XmlDeserialize<T>(string xml)
        {
            using (var memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(xml)))
            {
                var obj = GetSerializer<T>().Deserialize(memoryStream);
                return obj == null ? default(T) : (T)obj;
            }
        }

        #endregion
    }
}
