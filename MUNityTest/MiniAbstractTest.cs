using System;
using NUnit.Framework;

namespace MUNityTest
{
    [TestFixture]
    public class MiniAbstractTest
    {

        [Test]
        public void TestAbstraction()
        {
            var erbe = new Erbe();
            Console.WriteLine(erbe.Type);
        }

        public abstract class Abstrakt
        {
            public string Type => this.GetType().Name;
        }

        public class Erbe : Abstrakt
        {

        }
    }
}