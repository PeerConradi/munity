using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using MUNityAngular.Models;
using MUNityAngular.DataHandlers.Database;

namespace MUNityTest.Speakerlist
{
    class SpeakerlistTests
    {
        //[Test]
        public void TestCreateSpeakerlist()
        {
            var instance = new SpeakerlistModel();
            Assert.IsNotNull(instance);
        }

    }
}
