using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using MUNityCore.Models;
using MUNityCore.Util.Extensions;
using MUNityCore.Models.Conference;
using MUNityCore.Models.ListOfSpeakers;

namespace MUNityTest.Speakerlist
{
    class SpeakerlistTests
    {
        [Test]
        public void TestCreateSpeakerlist()
        {
            var instance = new ListOfSpeakers();
            Assert.IsNotNull(instance);
        }

       

    }
}
