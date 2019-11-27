using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using MUNityAngular.Models;

namespace MUNityTest.Resolution
{
    class MoveAmendmentTest
    {
        [SetUp]
        public void Setup()
        {

        }

        [Test]
        public void TestCreateMoveAmendment()
        {
            var amendment = new MoveAmendment();
            Assert.IsNotNull(amendment);
        }

        [Test]
        public void TestAppendMoveAmendment()
        {
            var resolution = new ResolutionModel();
            var sectionOne = resolution.AddOperativeParagraph("sectionOne");
            var sectionTwo = resolution.AddOperativeParagraph("sectionTwo");
            var amendment = new MoveAmendment();
            amendment.TargetSection = sectionTwo;
            amendment.NewPosition = 0;
            Assert.IsTrue(resolution.Amendments.Contains(amendment));
            Assert.AreEqual(1, sectionTwo.MoveAmendmentCount);
        }

        //[Test]
        public void TestActivateMoveAmendment()
        {
            var resolution = new ResolutionModel();
            var sectionOne = resolution.AddOperativeParagraph("sectionOne");
            var sectionTwo = resolution.AddOperativeParagraph("sectionTwo");
            var amendment = new MoveAmendment();
            amendment.TargetSection = sectionTwo;
            amendment.NewPosition = 0;
            Assert.IsTrue(resolution.Amendments.Contains(amendment));
            Assert.AreEqual(1, sectionTwo.MoveAmendmentCount);
            amendment.Activate();
            Assert.AreEqual(3, resolution.OperativeSections.Count);
            Assert.AreEqual(amendment.NewSection, resolution.OperativeSections[0]);
            Assert.AreEqual(sectionOne, resolution.OperativeSections[1]);
            Assert.AreEqual(sectionTwo, resolution.OperativeSections[2]);
        }
    }
}
