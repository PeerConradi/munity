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
        public void TestApplyChangeAmendment()
        {
            var resolution = new ResolutionModel();
            var section = resolution.AddOperativeParagraph();
            var amendment = new ChangeAmendmentModel();
            amendment.TargetSection = section;
            Assert.IsTrue(section.Amendments.Contains(amendment));
        }
    }
}
