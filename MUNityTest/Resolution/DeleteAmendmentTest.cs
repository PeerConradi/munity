using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using MUNityAngular.Models.Resolution;
using System.Linq;

namespace MUNityTest.Resolution
{
    class DeleteAmendmentTest
    {
        [SetUp]
        public void Setup() { }

        [Test]
        public void TestCreateDeleteAmendment()
        {
            var amendment = new DeleteAmendmentModel();
            Assert.IsNotNull(amendment);
        }

        [Test]
        public void TestAppendDeleteAmendment()
        {
            var resolution = new ResolutionModel();
            var section = resolution.AddOperativeParagraph("section");
            var amendment = new DeleteAmendmentModel();
            amendment.TargetSection = section;
            Assert.AreEqual(amendment, resolution.Amendments[0]);
            Assert.AreEqual(1, section.DeleteAmendmentCount);
            Assert.IsTrue(resolution.DeleteAmendments.Contains(amendment));
            Assert.AreEqual(section.ID, amendment.TargetSectionID);
        }

        [Test]
        public void TestDenyDeleteAmendment()
        {
            //Expectation: Should remove all other Move Amendments that are targeting the same Section
            var resolution = new ResolutionModel();
            var sectionOne = resolution.AddOperativeParagraph("section One");
            var sectionTwo = resolution.AddOperativeParagraph("section Two");
            var amendmentOne = new DeleteAmendmentModel();
            var amendmentTwo = new DeleteAmendmentModel();
            var amendmentThree = new DeleteAmendmentModel();
            amendmentOne.TargetSection = sectionOne;
            amendmentTwo.TargetSection = sectionOne;
            amendmentThree.TargetSection = sectionTwo;
            Assert.AreEqual(3, resolution.Amendments.Count);
            Assert.AreEqual(2, sectionOne.DeleteAmendmentCount);
            Assert.AreEqual(1, sectionTwo.DeleteAmendmentCount);
            amendmentOne.Deny();
            Assert.AreEqual(1, resolution.Amendments.Count);
            Assert.AreEqual(0, sectionOne.DeleteAmendmentCount);
            Assert.IsFalse(sectionOne.Amendments.Any());
        }

        [Test]
        public void TestActivateDeleteAmendment()
        {
            var resolution = new ResolutionModel();
            var section = resolution.AddOperativeParagraph("section");
            var amendment = new DeleteAmendmentModel();
            amendment.TargetSection = section;
            amendment.Activate();
            Assert.AreEqual(OperativeParagraphModel.EViewModus.Remove, section.ViewModus);
            Assert.AreEqual(section.ActiveAmendment, amendment);
            Assert.IsTrue(amendment.Activated);
        }

        [Test]
        public void TestDeactivateDeleteAmendment()
        {
            var resolution = new ResolutionModel();
            var section = resolution.AddOperativeParagraph("section");
            var amendment = new DeleteAmendmentModel();
            amendment.TargetSection = section;
            amendment.Activate();
            amendment.Deactivate();
            Assert.IsFalse(amendment.Activated);
            Assert.AreEqual(section.ActiveAmendment, null);
            Assert.AreEqual(OperativeParagraphModel.EViewModus.Normal, section.ViewModus);
        }

        [Test]
        public void TestRemoveDeleteAmendment()
        {
            var resolution = new ResolutionModel();
            var section = resolution.AddOperativeParagraph("section");
            var amendment = new DeleteAmendmentModel();
            amendment.TargetSection = section;
            amendment.Remove();
            Assert.IsFalse(resolution.Amendments.Contains(amendment));
        }

        [Test]
        public void TestSubmitDeleteAmendment()
        {
            //OAs aufbauen
            var resolution = new ResolutionModel();
            var sectionOne = resolution.AddOperativeParagraph("Section 1");
            var sectionOneOne = sectionOne.AddSubSection("Section 1.1");
            var sectionOneTwo = sectionOne.AddSubSection("Section 1.2");
            var sectionOneTwoOne = sectionOneTwo.AddSubSection("Section 1.2.1");
            var sectionTwo = resolution.AddOperativeParagraph("Section 2");

            //Änderungsanträge bauen
            //Der Ziel Änderungsantrag
            var amendmentOne = new DeleteAmendmentModel();
            amendmentOne.TargetSection = sectionOne;

            //Änderungsanträge welche durch ein Annehmen mit entfernt werden müssen
            var noiseOne = new DeleteAmendmentModel() { TargetSection = sectionOne };
            var noiseTwo = new ChangeAmendmentModel() { TargetSection = sectionOne };
            var noiseThree = new DeleteAmendmentModel() { TargetSection = sectionOneOne };
            var noiseFour = new MoveAmendment() { TargetSection = sectionOneTwo };

            //Änderungsanträge die erhalten bleiben sollen
            var keepOne = new DeleteAmendmentModel() { TargetSection = sectionTwo };

            //Zwischentest
            Assert.AreEqual(5, resolution.OperativeSections.Count);
            Assert.AreEqual(6, resolution.Amendments.Count);

            amendmentOne.Submit();
            Assert.AreEqual(1, resolution.OperativeSections.Count);
            Assert.AreEqual(1, resolution.Amendments.Count);
        }
    }
}
