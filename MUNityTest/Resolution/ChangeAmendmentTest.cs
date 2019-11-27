using System;
using System.Collections.Generic;
using System.Text;
using MUNityAngular.Models;
using NUnit.Framework;

namespace MUNityTest.Resolution
{
    class ChangeAmendmentTest
    {
        [SetUp]
        public void Setup() { }

        [Test]
        public void TestCreateChangeAmendment()
        {
            var amendment = new ChangeAmendmentModel();
            Assert.IsNotNull(amendment);
        }

        [Test]
        public void TestAppendChangeAmendment()
        {
            var resolution = new ResolutionModel();
            var section = resolution.AddOperativeParagraph();
            var amendment = new ChangeAmendmentModel();
            amendment.TargetSection = section;
            Assert.IsTrue(section.Amendments.Contains(amendment));
            Assert.IsTrue(section.Resolution.Amendments.Contains(amendment));
            Assert.AreEqual(1, section.ChangeAmendmentCount);
        }

        [Test]
        public void TestSubmitChangeAmendment()
        {
            var resolution = new ResolutionModel();
            var expection = "new Text";
            var section = resolution.AddOperativeParagraph("sectionOne");
            var amendment = new ChangeAmendmentModel();
            amendment.NewText = expection;
            amendment.TargetSection = section;
            amendment.Submit();
            Assert.AreEqual(expection, section.Text);
            Assert.IsFalse(resolution.Amendments.Contains(amendment));
        }

        [Test]
        public void TestRemoveChangeAmendment()
        {
            var resolution = new ResolutionModel();
            var section = resolution.AddOperativeParagraph();
            var amendment = new ChangeAmendmentModel();
            amendment.TargetSection = section;
            Assert.IsTrue(section.Amendments.Contains(amendment));
            Assert.IsTrue(section.Resolution.Amendments.Contains(amendment));
            amendment.Remove();
            Assert.IsFalse(section.Amendments.Contains(amendment));
            Assert.IsFalse(section.Resolution.Amendments.Contains(amendment));
        }

        [Test]
        public void TestActivateChangeAmendment()
        {
            var resolution = new ResolutionModel();
            var section = resolution.AddOperativeParagraph();
            var amendment = new ChangeAmendmentModel();
            amendment.TargetSection = section;
            amendment.Activate();
            Assert.IsTrue(amendment.Activated);
            Assert.AreEqual(amendment, section.ActiveAmendment);
        }

        [Test]
        public void TestDeactivateChangeAmendment()
        {
            var resolution = new ResolutionModel();
            var section = resolution.AddOperativeParagraph();
            var amendment = new ChangeAmendmentModel();
            amendment.TargetSection = section;
            amendment.Activate();
            Assert.IsTrue(amendment.Activated);
            Assert.AreEqual(amendment, section.ActiveAmendment);
            amendment.Deactivate();
            Assert.IsNull(section.ActiveAmendment);
            Assert.IsFalse(amendment.Activated);
        }
    }
}
