using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using MUNityAngular.Models;
using Moq;
using MUNityAngular.Util.Extenstions;

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
        public void TestRemoveOperativeSectionRemovesChildren()
        {
            var resolution = new ResolutionModel();
            var sectionOne = resolution.AddOperativeParagraph("section 1");
            var sectionTwo = sectionOne.AddSubSection("Section1.1");
            var sectionThree = sectionOne.AddSubSection("section1.2");
            var sectionFour = resolution.AddOperativeParagraph("Section 2");
            Assert.AreEqual(4, resolution.OperativeSections.Count);
            sectionOne.Remove();
            Assert.AreEqual(1, resolution.OperativeSections.Count);
        }

        [Test]
        public void TestRemoveSubSubSectionsWhenGrandpaIsDeleted()
        {
            var resolution = new ResolutionModel();
            var sectionOne = resolution.AddOperativeParagraph("section 1");
            var sectionTwo = sectionOne.AddSubSection("Section1.1");
            var sectionThree = sectionTwo.AddSubSection("section1.1.1");
            var sectionFour = resolution.AddOperativeParagraph("Section 2");
            Assert.AreEqual(4, resolution.OperativeSections.Count);
            sectionOne.Remove();
            Assert.AreEqual(1, resolution.OperativeSections.Count);
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

        [Test]
        public void TestSaveAndLoadResolution()
        {
            var resolution = new ResolutionModel();
            //Document Settings
            var resolutionDate = new DateTime(1995, 11, 7, 22, 30, 0);
            resolution.Date = resolutionDate;
            resolution.Name = "Test Resolution";
            resolution.FullName = "Test Resolution";
            resolution.Topic = "Test";
            resolution.Session = "1";
            resolution.DocumentNumber = 1;

            //Preamble
            resolution.Preamble.AddParagraph("paragraphOne");
            resolution.Preamble.AddParagraph("paragraphTwo");

            //Operative Sections
            var sectionOne = resolution.AddOperativeParagraph("sectionOne");
            var sectionTwo = resolution.AddOperativeParagraph("sectionTwo");
            var sectionTwoOne = sectionTwo.AddSubSection("sectionTwoOne");

            Assert.AreEqual(sectionTwo, sectionTwoOne.Parent);

            //Amendments
            var changeAmendment = new ChangeAmendmentModel();
            changeAmendment.NewText = "new Text";
            changeAmendment.TargetSection = sectionOne;

            var deleteAmendment = new DeleteAmendmentModel();
            deleteAmendment.TargetSection = sectionOne;

            var moveAmendment = new MoveAmendment();
            moveAmendment.TargetSection = sectionOne;
            moveAmendment.NewPosition = 1;

            var addAmendment = new AddAmendmentModel();
            addAmendment.NewText = "new Section";
            addAmendment.TargetResolution = resolution;
            addAmendment.TargetPosition = 2;

            //Safe
            var service = new MUNityAngular.Services.ResolutionService();

            string safedText = resolution.ToJson();

            //Load
            var loadedResolution = service.GetResolutionFromJson(safedText);
            //Document Info
            Assert.AreEqual(resolution.ID, loadedResolution.ID);
            Assert.AreEqual(resolution.Name, loadedResolution.Name);
            Assert.AreEqual(resolution.FullName, loadedResolution.FullName);
            Assert.AreEqual(resolution.Topic, loadedResolution.Topic);
            Assert.AreEqual(resolution.Date, loadedResolution.Date);
            Assert.AreEqual(resolution.Session, loadedResolution.Session);
            Assert.AreEqual(resolution.DocumentNumber, loadedResolution.DocumentNumber);
            //Preamble
            Assert.AreEqual(resolution.Preamble.ID, loadedResolution.Preamble.ID);
            Assert.AreEqual(resolution.Preamble.Paragraphs.Count, loadedResolution.Preamble.Paragraphs.Count);
            Assert.AreEqual(resolution.Preamble.Paragraphs[0].ID, loadedResolution.Preamble.Paragraphs[0].ID);
            Assert.AreEqual(resolution.Preamble.Paragraphs[0].Text, loadedResolution.Preamble.Paragraphs[0].Text);
            Assert.AreEqual(resolution.Preamble.Paragraphs[1].ID, loadedResolution.Preamble.Paragraphs[1].ID);
            Assert.AreEqual(resolution.Preamble.Paragraphs[1].Text, loadedResolution.Preamble.Paragraphs[1].Text);
            //Operative Sections
            Assert.AreEqual(resolution.OperativeSections.Count, loadedResolution.OperativeSections.Count);
            Assert.AreEqual(resolution.OperativeSections[0].ID, loadedResolution.OperativeSections[0].ID);
            Assert.AreEqual(resolution.OperativeSections[0].Text, loadedResolution.OperativeSections[0].Text);
            Assert.AreEqual(resolution.OperativeSections[1].ID, loadedResolution.OperativeSections[1].ID);
            Assert.AreEqual(resolution.OperativeSections[1].Text, loadedResolution.OperativeSections[1].Text);
            Assert.AreEqual(resolution.OperativeSections[2].ID, loadedResolution.OperativeSections[2].ID);
            Assert.AreEqual(resolution.OperativeSections[2].Text, loadedResolution.OperativeSections[2].Text);
            Assert.AreEqual(sectionTwoOne.ParentID, loadedResolution.OperativeSections[2].ParentID);
            Assert.IsNotNull(loadedResolution.OperativeSections[2].Parent);
            //Amendments
            Assert.AreEqual(resolution.Amendments.Count, loadedResolution.Amendments.Count);
           
            var loadedChangeAmendment = loadedResolution.Amendments[0] as ChangeAmendmentModel;
            Assert.NotNull(loadedChangeAmendment);
            Assert.AreEqual(changeAmendment.ID, loadedChangeAmendment.ID);
            Assert.AreEqual(changeAmendment.TargetSectionID, loadedChangeAmendment.TargetSectionID);
            Assert.AreEqual(loadedResolution.OperativeSections[0], loadedChangeAmendment.TargetSection);

            var loadedDeleteAmendment = loadedResolution.Amendments[1] as DeleteAmendmentModel;
            Assert.NotNull(loadedDeleteAmendment);
            Assert.AreEqual(loadedDeleteAmendment.ID, deleteAmendment.ID);
            Assert.AreEqual(loadedDeleteAmendment.TargetSectionID, deleteAmendment.TargetSectionID);
            Assert.AreEqual(loadedResolution.OperativeSections[0], loadedDeleteAmendment.TargetSection);

        }

        [Test]
        public void RemoveOperativeSectionRemovesAllAmendments()
        {
            var resolution = new ResolutionModel();
            var sectionOne = resolution.AddOperativeParagraph("Paragraph 1");
            var sectionTwo = resolution.AddOperativeParagraph("Paragraph 2");
            var amendmentOne = new ChangeAmendmentModel();
            amendmentOne.TargetSection = sectionOne;
            var amendmentTwo = new ChangeAmendmentModel();
            amendmentTwo.TargetSection = sectionTwo;

            //Zwischentest
            Assert.AreEqual(2, resolution.Amendments.Count);
            Assert.AreEqual(2, resolution.OperativeSections.Count);
            sectionOne.Remove();

            Assert.AreEqual(1, resolution.Amendments.Count);
            Assert.AreEqual(1, resolution.OperativeSections.Count);
        }
    }
}
