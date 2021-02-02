using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using MUNity.Extensions.LoSExtensions;

namespace MunityNUnitTest.ListOfSpeakerTest
{
    public class ComparerTest
    {
        [Test]
        public void TestInitWithSameId()
        {
            var listOne = new MUNity.Models.ListOfSpeakers.ListOfSpeakers();
            var listTwo = new MUNity.Models.ListOfSpeakers.ListOfSpeakers();
            listTwo.ListOfSpeakersId = listOne.ListOfSpeakersId;
            var result = listOne.CompareTo(listTwo);
            Assert.AreEqual(0, result);
        }

        [Test]
        public void TestMoreSpeakersReturnsFalse()
        {
            var instanceOne = new MUNity.Models.ListOfSpeakers.ListOfSpeakers();
            var instanceTwo = new MUNity.Models.ListOfSpeakers.ListOfSpeakers();
            instanceTwo.ListOfSpeakersId = instanceOne.ListOfSpeakersId;
            instanceOne.AddSpeaker("Speaker 1");
            var result = instanceOne.CompareTo(instanceTwo);
            Assert.AreNotEqual(0, result);
        }
    }
}
