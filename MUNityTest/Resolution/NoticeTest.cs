using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using MUNityAngular.Models.Resolution;

namespace MUNityTest.Resolution
{
    class NoticeTest
    {
       [Test]
       public void ResolutionHasNoticesTest()
        {
            var resolution = new ResolutionModel();
            Assert.NotNull(resolution.Notices);
        }
    }
}
