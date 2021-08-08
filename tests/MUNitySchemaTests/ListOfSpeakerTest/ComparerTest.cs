using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using MUNity.Extensions.LoSExtensions;
using MUNity.ViewModels.ListOfSpeakers;

namespace MunityNUnitTest.ListOfSpeakerTest
{
    public class ComparerTest
    {
        [Test]
        public void TestInitWithSameId()
        {
            var listOne = new ListOfSpeakersViewModel();
            var listTwo = new ListOfSpeakersViewModel();
            listTwo.ListOfSpeakersId = listOne.ListOfSpeakersId;
            var result = listOne.CompareTo(listTwo);
            Assert.AreEqual(0, result);
        }

        [Test]
        public void TestMoreSpeakersReturnsFalse()
        {
            var instanceOne = new ListOfSpeakersViewModel();
            var instanceTwo = new ListOfSpeakersViewModel();
            instanceTwo.ListOfSpeakersId = instanceOne.ListOfSpeakersId;
            instanceOne.AddSpeaker("Speaker 1");
            var result = instanceOne.CompareTo(instanceTwo);
            Assert.AreNotEqual(0, result);
        }
    }
}
