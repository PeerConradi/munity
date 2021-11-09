using MUNity.Extensions;
using NUnit.Framework;
using System.Collections.Generic;

namespace Munity.Schema.Test.Extenstions;

public class ListExtensionTests
{
    [Test]
    public void TestSwap()
    {
        var list = new List<string>();
        list.Add("Element 1");
        list.Add("Element 2");
        var swappedElement = list.Swap(0, 1);
        Assert.AreEqual("Element 2", swappedElement[0]);
        Assert.AreEqual("Element 1", swappedElement[1]);
    }
}
