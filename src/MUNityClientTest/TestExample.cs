using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Bunit;

namespace MunityClientTest
{
    public class TestExample
    {
        [Fact]
        public void BaseTest()
        {
            using var ctx = new TestContext();

            var comp = ctx.RenderComponent<MUNityClient.Shared.Navbar>();
            Assert.NotNull(comp);
        }
    }
}
