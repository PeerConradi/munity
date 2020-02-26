using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using MUNityAngular.Services;
using MUNityAngular.Models.Conference;

namespace MUNityTest.ServiceTests
{
    class SpeakerlistServiceTest
    {
        [Test]
        public void TimerIsDecrisingSpeakerTimeTest()
        {
            var service = new SpeakerlistService();
            var speakerlist = service.CreateSpeakerlist();
            speakerlist.Speakertime = new TimeSpan(0, 1, 0);
            var delegation = new DelegationModel();
            delegation.Name = "Test";
            speakerlist.AddSpeaker(delegation);
            speakerlist.NextSpeaker();
            speakerlist.StartSpeaker();
            System.Threading.Thread.Sleep(5000);
            Assert.IsTrue(speakerlist.RemainingSpeakerTime < new TimeSpan(0, 0, 57));
            Assert.IsTrue(speakerlist.RemainingSpeakerTime > new TimeSpan(0, 0, 50));
        }

        [Test]
        public void StartStopShouldContinueTest()
        {
            var service = new SpeakerlistService();
            var speakerlist = service.CreateSpeakerlist();
            speakerlist.Speakertime = new TimeSpan(0, 1, 0);
            var delegation = new DelegationModel();
            delegation.Name = "Test";
            speakerlist.AddSpeaker(delegation);
            speakerlist.NextSpeaker();
            speakerlist.StartSpeaker();
            System.Threading.Thread.Sleep(5000);
            speakerlist.PauseSpeaker();
            speakerlist.StartSpeaker();
            Assert.IsTrue(speakerlist.RemainingSpeakerTime < new TimeSpan(0, 0, 57));
            Assert.IsTrue(speakerlist.RemainingSpeakerTime > new TimeSpan(0, 0, 50));
        }
    }
}
