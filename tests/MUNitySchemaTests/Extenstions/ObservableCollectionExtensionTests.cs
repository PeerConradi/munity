using MUNity.Extensions.ObservableCollectionExtensions;
using NUnit.Framework;
using System.Collections.ObjectModel;

namespace Munity.Schema.Test.Extenstions;

public class ObservableCollectionExtensionTests
{
    [Test]
    public void TestRemoveAll()
    {
        var collection = new ObservableCollection<string>();
        collection.Add("ToRemove 1");
        collection.Add("ToRemove 2");
        collection.Add("DontRemove");
        collection.RemoveAll(n => n.Contains("ToRemove"));
        Assert.AreEqual(1, collection.Count);
    }
}
