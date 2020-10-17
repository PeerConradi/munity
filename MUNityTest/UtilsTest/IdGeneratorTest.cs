using NUnit.Framework;

namespace MUNityTest.UtilsTest
{
    [TestFixture]
    public class IdGeneratorTest
    {
        [Test]
        public void TestToPrimaryKey()
        {
            string key = "dmun e.V.";
            var result = MUNityAngular.Util.Tools.IdGenerator.AsPrimaryKey(key);
            Assert.AreEqual("dmunev", result);
        }
    }
}