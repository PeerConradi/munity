using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MUNityBaseTest.Util
{
    public class IdGeneratorTest
    {
        [Test]
        public void TestCreateRandomString()
        {
            var randomString = MUNity.Util.IdGenerator.RandomString(10);
            Assert.AreEqual(10, randomString.Length);
        }

        [Test]
        public void TestAsPrimaryKeyReplacesUmlaute()
        {
            string text = "äöü";
            var result = MUNity.Util.IdGenerator.AsPrimaryKey(text);
            Assert.AreEqual("aeoeue", result);
        }

        [Test]
        public void TestAsPrimaryKeyRemovesSpecialChars()
        {
            string text = "Hello$World";
            var result = MUNity.Util.IdGenerator.AsPrimaryKey(text);
            Assert.AreEqual("helloworld", result);
        }
    }
}
