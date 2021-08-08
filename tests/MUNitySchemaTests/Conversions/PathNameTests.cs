using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MUNity.Extensions.Conversion;

namespace MunitySchemaTest.Conversions
{
    public class PathNameTests
    {
        [Test]
        public void TestToPathNameEmptyArray()
        {
            int[] array = new int[0];
            var pathname = array.ToPathname();
            Assert.AreEqual("", pathname);
        }
    }
}
