using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using MUNityAngular.Models;

namespace MUNityTest.Resolution
{
    class WorkflowCaseTest
    {
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
