using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using MUNityAngular.Models;
using Moq;
using MUNityAngular.DataHandlers.Database;

namespace MUNityTest.Resolution
{
    class MainResolutionTests
    {
        
        

        [SetUp]
        public void Setup()
        {

        }

        [Test]
        public void TestCanCreateResolution()
        {
            var resolution = new ResolutionModel();
            Assert.IsNotNull(resolution);
        }

        [Test]
        public void TestConferenceGetter()
        {
            var resolution = new ResolutionModel();
            resolution.ConferenceID = "Test";
            Assert.AreEqual(resolution.Conference, ConferenceHandler.TestConference);
        }

        [Test]
        public void TestAddOperativeSectionBasic()
        {
            var resolution = new ResolutionModel();
            var sectionOne = resolution.AddOperativeParagraph("sectionOne");
            var sectionTwo = resolution.AddOperativeParagraph("sectionTwo");
            Assert.AreEqual(sectionOne, resolution.OperativeSections[0]);
            Assert.AreEqual(sectionTwo, resolution.OperativeSections[1]);
        }

        [Test]
        public void TestRemoveOperativeSection()
        {
            var resolution = new ResolutionModel();
            var sectionOne = resolution.AddOperativeParagraph("sectionOne");
            var sectionTwo = resolution.AddOperativeParagraph("sectionTwo");
            Assert.AreEqual(sectionOne, resolution.OperativeSections[0]);
            Assert.AreEqual(sectionTwo, resolution.OperativeSections[1]);
            sectionOne.Remove();
            Assert.AreEqual(sectionTwo, resolution.OperativeSections[0]);
        }

        [Test]
        public void TestMoveOperativeSectionUp()
        {
            var resolution = new ResolutionModel();
            var sectionOne = resolution.AddOperativeParagraph("sectionOne");
            var sectionTwo = resolution.AddOperativeParagraph("sectionTwo");
            Assert.AreEqual(sectionOne, resolution.OperativeSections[0]);
            Assert.AreEqual(sectionTwo, resolution.OperativeSections[1]);
            sectionTwo.MoveUp();
            Assert.AreEqual(sectionOne, resolution.OperativeSections[1]);
            Assert.AreEqual(sectionTwo, resolution.OperativeSections[0]);
        }

        [Test]
        public void TestMoveOperativeSectionDown()
        {
            var resolution = new ResolutionModel();
            var sectionOne = resolution.AddOperativeParagraph("sectionOne");
            var sectionTwo = resolution.AddOperativeParagraph("sectionTwo");
            Assert.AreEqual(sectionOne, resolution.OperativeSections[0]);
            Assert.AreEqual(sectionTwo, resolution.OperativeSections[1]);
            sectionOne.MoveDown();
            Assert.AreEqual(sectionOne, resolution.OperativeSections[1]);
            Assert.AreEqual(sectionTwo, resolution.OperativeSections[0]);
        }

        [Test]
        public void TestMoveOperativeSectionRightLeft()
        {
            var resolution = new ResolutionModel();
            var sectionOne = resolution.AddOperativeParagraph("sectionOne");
            var sectionTwo = resolution.AddOperativeParagraph("sectionTwo");
            Assert.AreEqual(resolution.OperativeSections[0], sectionOne);
            Assert.AreEqual(resolution.OperativeSections[1], sectionTwo);
            sectionTwo.MoveRight();
            Assert.AreEqual(resolution.OperativeSections[0], sectionOne);
            Assert.AreEqual(sectionOne, sectionTwo.Parent);
            sectionTwo.MoveLeft();
            Assert.IsNull(sectionTwo.Parent);
        }

        [Test]
        public void TestMoveOperativeSectionLeftNotMoveable()
        {
            var resolution = new ResolutionModel();
            var sectionOne = resolution.AddOperativeParagraph("sectionOne");
            Assert.AreEqual(resolution.OperativeSections[0], sectionOne);
            sectionOne.MoveLeft();
            Assert.AreEqual(resolution.OperativeSections[0], sectionOne);
            Assert.IsNull(sectionOne.Parent);
        }

        [Test]
        public void TestMoveOperativeSectionRightNotMoveable()
        {
            var resolution = new ResolutionModel();
            var sectionOne = resolution.AddOperativeParagraph("sectionOne");
            Assert.AreEqual(resolution.OperativeSections[0], sectionOne);
            sectionOne.MoveRight();
            Assert.AreEqual(resolution.OperativeSections[0], sectionOne);
            Assert.IsNull(sectionOne.Parent);
        }

        [Test]
        public void TestMoveUpNotMoveable()
        {
            var resolution = new ResolutionModel();
            var sectionOne = resolution.AddOperativeParagraph("sectionOne");
            var sectionTwo = resolution.AddOperativeParagraph("sectionTwo");
            Assert.AreEqual(resolution.OperativeSections[0], sectionOne);
            Assert.AreEqual(resolution.OperativeSections[1], sectionTwo);
            sectionOne.MoveUp();
            Assert.AreEqual(resolution.OperativeSections[0], sectionOne);
            Assert.AreEqual(resolution.OperativeSections[1], sectionTwo);
        }

        [Test]
        public void TestMoveDownNotMoveable()
        {
            var resolution = new ResolutionModel();
            var sectionOne = resolution.AddOperativeParagraph("sectionOne");
            var sectionTwo = resolution.AddOperativeParagraph("sectionTwo");
            Assert.AreEqual(resolution.OperativeSections[0], sectionOne);
            Assert.AreEqual(resolution.OperativeSections[1], sectionTwo);
            sectionTwo.MoveDown();
            Assert.AreEqual(resolution.OperativeSections[0], sectionOne);
            Assert.AreEqual(resolution.OperativeSections[1], sectionTwo);
        }
    }
}
