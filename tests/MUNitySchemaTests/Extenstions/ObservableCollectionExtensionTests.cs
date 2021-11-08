using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MUNity.Extensions.ObservableCollectionExtensions;
using System.Collections.ObjectModel;

namespace MunitySchemaTest.Extenstions
{
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
}
