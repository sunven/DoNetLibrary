using System;
using DoNetLibrary;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Test.DoNetLibrary
{
    [TestClass]
    public class RetryTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            bool GetA(int i)
            {
                Console.WriteLine($"Fun {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
                return new Random().Next(0, 100) > 90;
            }
            var r = new Retry(c => GetA((int)c), 0, () => { Console.WriteLine("成功了"); }, () => { Console.WriteLine("失败了"); }, () => { Console.WriteLine("完成了"); }, 1000, 1000, 0);
        }
    }
}
