using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Common
{
    public class Reflection
    {
        string ClassName = "";
        Type clstype;
        public Reflection(string ClassName)
        {
            this.ClassName = ClassName;
        }
        public Reflection()
        {

        }
        /// <summary>  
        /// 实例对象时需要指定类名  
        /// </summary>  
        public object GetClassInstance(string assembly, string NameSpace)
        {
            //assembly为程序集名称，NameSpace为命名空间  

            clstype = Assembly.Load(assembly).GetType(string.Concat(NameSpace, ".", this.ClassName));
            if (clstype == null)
                return null;
            object obj = (object)Activator.CreateInstance(clstype);
            return obj;
        }
        /// <summary>  
        /// 实例对象时不用指定类名  
        /// </summary>  
        public object GetClassInstance(string assembly, string NameSpace, string classname)
        {
            ClassName = classname;
            clstype = Assembly.Load(assembly).GetType(string.Concat(NameSpace, ".", classname));
            if (clstype == null)
                return null;
            object obj = (object)Activator.CreateInstance(clstype);
            return obj;
        }
        /// <summary>  
        /// 执行类的静态方法  
        /// </summary>  
        /// <param name="methodname">  
        /// 类的方法名  
        /// </param>  
        /// <param name="parameters">  
        /// 方法的参数类型  
        /// </param>  
        /// <param name="methodtype">  
        /// 方法的参数  
        /// </param>  

        public object GetMethod(string methodname, Type[] methodtype, object[] parameters)
        {
            // methodtype.SetValue(typeof(string),1);  
            System.Reflection.MethodInfo pMethod = clstype.GetMethod(methodname, BindingFlags.IgnoreCase | BindingFlags.Static | BindingFlags.Public, null, methodtype, null);
            //调用方法的一些标志位，这里的含义是Public并且是实例方法，这也是默认的值  
            //System.Reflection.BindingFlags flag = BindingFlags.IgnoreCase | BindingFlags.Static | BindingFlags.Public;  
            object returnValue = pMethod.Invoke(null, parameters);
            //string returnValue = pMethod.Invoke(clsObj, flag, Type.DefaultBinder, parameters,null).ToString();  
            return returnValue;
        }
    }
}
