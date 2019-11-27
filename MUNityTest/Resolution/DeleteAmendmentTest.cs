using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using MUNityAngular.Models;

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
    }
}
