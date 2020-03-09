using Microsoft.VisualStudio.TestTools.UnitTesting;
using Utility;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Linq;

namespace Utility.Tests
{
    [TestClass()]
    public class GuidExTests
    {
        [TestMethod()]
        public void NewGuidTest()
        {
            List<long> ls = new List<long>();
            //循环调用10次
            for (int i = 0; i < 20; i++)
            {
                var id = GuidEx.NewGuid();
                ls.Add(id);
                Console.WriteLine(id);
                //Thread.Sleep(100);
            }

            Assert.IsTrue(ls.Distinct().Count() == 20);
        }
    }
}



namespace UtilityTests
{
    class GuidExTests
    {
    }
}
