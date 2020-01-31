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
            Assert.AreEqual(sectionTwo.ID, amendment.TargetSectionID);
        }

        [Test]
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
            Assert.IsTrue(amendment.TargetSection.IsLocked);
            Assert.AreEqual(sectionTwo.ActiveAmendment, amendment);
        }

        [Test]
        public void TestSubmitMoveAmendment()
        {
            var resolution = new ResolutionModel();
            var sectionOne = resolution.AddOperativeParagraph("sectionOne");
            var sectionTwo = resolution.AddOperativeParagraph("sectionTwo");
            var amendment = new MoveAmendment();
            amendment.TargetSection = sectionTwo;
            amendment.NewPosition = 0;
            amendment.Submit();
            Assert.AreEqual(2, resolution.OperativeSections.Count);
            Assert.AreEqual(sectionTwo, resolution.OperativeSections[0]);
            Assert.AreEqual(sectionOne, resolution.OperativeSections[1]);
            Assert.IsFalse(sectionTwo.IsLocked);
            Assert.IsFalse(sectionTwo.IsVirtual);
            Assert.IsFalse(resolution.Amendments.Contains(amendment));
        }

        [Test]
        public void TestSubmitAfterActivate()
        {
            var resolution = new ResolutionModel();
            var sectionOne = resolution.AddOperativeParagraph("sectionOne");
            var sectionTwo = resolution.AddOperativeParagraph("sectionTwo");
            var amendment = new MoveAmendment();
            amendment.TargetSection = sectionTwo;
            amendment.NewPosition = 0;
            amendment.Activate();
            amendment.Submit();
            Assert.AreEqual(2, resolution.OperativeSections.Count);
            Assert.AreEqual(sectionTwo, resolution.OperativeSections[0]);
            Assert.AreEqual(sectionOne, resolution.OperativeSections[1]);
            Assert.IsFalse(sectionTwo.IsLocked);
            Assert.IsFalse(sectionTwo.IsVirtual);
            Assert.IsFalse(resolution.Amendments.Contains(amendment));
        }

        [Test]
        public void TestRemoveAddAmendment()
        {
            var resolution = new ResolutionModel();
            var sectionOne = resolution.AddOperativeParagraph("sectionOne");
            var sectionTwo = resolution.AddOperativeParagraph("sectionTwo");
            var amendment = new MoveAmendment();
            amendment.TargetSection = sectionTwo;
            amendment.NewPosition = 0;
            amendment.Remove();
            Assert.IsFalse(resolution.Amendments.Contains(amendment));
        }

        //In this case everything should stay te same!
        [Test]
        public void TestUpdateMoveAmendmentPositionOnOperativeSectionAdded()
        {
            var resolution = new ResolutionModel();
            var sectionOne = resolution.AddOperativeParagraph("section One");
            var sectionTwo = resolution.AddOperativeParagraph("sectionTwo");
            var amendment = new MoveAmendment();
            amendment.TargetSection = sectionOne;
            amendment.NewPosition = 1;
            var sectionThree = resolution.AddOperativeParagraph("section Three");
            Assert.AreEqual(1, amendment.NewPosition);
        }

        [Test]
        public void TestUpdateMoveAmendmentPositionOnOperativeSectionInsert()
        {
            var resolution = new ResolutionModel();
            var sectionOne = resolution.AddOperativeParagraph("section One");
            var sectionTwo = resolution.AddOperativeParagraph("sectionTwo");
            var amendment = new MoveAmendment();
            amendment.TargetSection = sectionOne;
            amendment.NewPosition = 1;
            var sectionThree = resolution.AddOperativeParagraph(0);
            Assert.AreEqual(1, amendment.NewPosition);
        }
    }
}
