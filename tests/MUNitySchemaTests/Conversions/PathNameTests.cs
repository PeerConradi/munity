using MUNity.Extensions.Conversion;
using NUnit.Framework;

namespace Munity.Schema.Test.Conversions;

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
