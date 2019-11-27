using System;
using System.Collections.Generic;
using System.Text;
using MUNityAngular.Models;
using NUnit.Framework;

namespace MUNityTest.Resolution
{
    class AddAmendmentTest
    {
        [SetUp]
        public void Setup() { }

        [Test]
        public void TestCreateAddAmendment()
        {
            var amendment = new AddAmendmentModel();
            Assert.IsNotNull(amendment);
        }

        [Test]
        public void TestAppendAddAmendment()
        {
            var resolution = new ResolutionModel();
            var section = resolution.AddOperativeParagraph();
            var amendment = new AddAmendmentModel();
            amendment.TargetPosition = 1;
            amendment.TargetResolution = resolution;
            Assert.IsTrue(section.Resolution.Amendments.Contains(amendment), "Amendment List does not contain this Amendment");
            Assert.AreEqual(section, resolution.OperativeSections[0]);
            Assert.AreEqual(1, resolution.OperativeSections.Count);
        }

        [Test]
        public void TestActivateAddAmendment()
        {
            var resolution = new ResolutionModel();
            var section = resolution.AddOperativeParagraph();
            var amendment = new AddAmendmentModel();
            amendment.TargetPosition = 1;
            amendment.TargetResolution = resolution;
            amendment.Activate();
            Assert.AreEqual(amendment.TargetSection, resolution.OperativeSections[1]);
            Assert.IsTrue(amendment.TargetSection.IsVirtual);
        }

        [Test]
        public void TestDeactivateAddAmendment()
        {
            var resolution = new ResolutionModel();
            var section = resolution.AddOperativeParagraph();
            var amendment = new AddAmendmentModel();
            amendment.TargetPosition = 1;
            amendment.TargetResolution = resolution;
            amendment.Activate();
            amendment.Deactivate();
            Assert.IsFalse(resolution.OperativeSections.Contains(amendment.TargetSection));
        }

        [Test]
        public void TestRemoveAddAmendment()
        {
            var resolution = new ResolutionModel();
            var section = resolution.AddOperativeParagraph();
            var amendment = new AddAmendmentModel();
            amendment.TargetPosition = 1;
            amendment.TargetResolution = resolution;
            amendment.Remove();
            Assert.IsFalse(resolution.Amendments.Contains(amendment));
        }

        [Test]
        public void TestSubmitAddAmendment()
        {
            var expectedText = "new Section";
            var resolution = new ResolutionModel();
            var section = resolution.AddOperativeParagraph();
            var amendment = new AddAmendmentModel();
            amendment.TargetPosition = 1;
            amendment.TargetResolution = resolution;
            amendment.NewText = expectedText;
            amendment.Submit();
            Assert.IsFalse(resolution.Amendments.Contains(amendment));
            Assert.AreEqual(2, resolution.OperativeSections.Count);
            Assert.AreEqual(amendment.TargetSection, resolution.OperativeSections[1]);
            Assert.IsFalse(resolution.OperativeSections[1].IsVirtual);
            Assert.AreEqual(expectedText, resolution.OperativeSections[1].Text);
        }

        [Test]
        public void TestActivateThenSubmitAddAmendment()
        {
            var expectedText = "new Section";
            var resolution = new ResolutionModel();
            var section = resolution.AddOperativeParagraph();
            var amendment = new AddAmendmentModel();
            amendment.TargetPosition = 1;
            amendment.TargetResolution = resolution;
            amendment.NewText = expectedText;
            amendment.Activate();
            amendment.Submit();
            Assert.IsFalse(resolution.Amendments.Contains(amendment));
            Assert.AreEqual(2, resolution.OperativeSections.Count);
            Assert.AreEqual(amendment.TargetSection, resolution.OperativeSections[1]);
            Assert.IsFalse(resolution.OperativeSections[1].IsVirtual);
            Assert.AreEqual(expectedText, resolution.OperativeSections[1].Text);
        }

    }
}
