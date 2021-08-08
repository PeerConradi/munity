using MUNity.Models.Resolution;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using MUNity.Extensions.ResolutionExtensions;
using MUNity.Exceptions.Resolution;

namespace MunityNUnitTest.ResolutionTest
{
    public class ChangeAmendmentTest
    {
        [Test]
        public void TestCreateInstance()
        {
            var resolution = new Resolution();
            var operativeParagraph = resolution.OperativeSection.CreateOperativeParagraph();
            var changeAmendmet = resolution.OperativeSection.CreateChangeAmendment(operativeParagraph);
            Assert.NotNull(changeAmendmet);
            Assert.Contains(changeAmendmet, resolution.OperativeSection.ChangeAmendments);
            Assert.AreEqual(1, resolution.OperativeSection.ChangeAmendments.Count);
        }

        [Test]
        public void TestRemoveAmendment()
        {
            var resolution = new Resolution();
            var operativeParagraph = resolution.OperativeSection.CreateOperativeParagraph();
            var changeAmendment = resolution.OperativeSection.CreateChangeAmendment(operativeParagraph);
            resolution.OperativeSection.RemoveAmendment(changeAmendment);
            Assert.IsFalse(resolution.OperativeSection.ChangeAmendments.Contains(changeAmendment));
        }

        [Test]
        public void TestApplyChangeAmendment()
        {
            var resolution = new Resolution();
            var operativeParagraph = resolution.OperativeSection.CreateOperativeParagraph("Original Text");
            var changeAmendment = resolution.OperativeSection.CreateChangeAmendment(operativeParagraph, "New Text");
            Assert.AreEqual("Original Text", operativeParagraph.Text);
            changeAmendment.Apply(resolution.OperativeSection);
            Assert.AreEqual("New Text", operativeParagraph.Text);
            Assert.IsFalse(resolution.OperativeSection.ChangeAmendments.Contains(changeAmendment));
        }

        [Test]
        public void TestDenyChangeAmendment()
        {
            var resolution = new Resolution();
            var operativeParagraph = resolution.OperativeSection.CreateOperativeParagraph("Original Text");
            var changeAmendment = resolution.OperativeSection.CreateChangeAmendment(operativeParagraph, "New Text");
            Assert.AreEqual("Original Text", operativeParagraph.Text);
            changeAmendment.Deny(resolution.OperativeSection);
            Assert.AreEqual("Original Text", operativeParagraph.Text);
            Assert.IsFalse(resolution.OperativeSection.ChangeAmendments.Contains(changeAmendment));
        }

        [Test]
        public void TestAmendmentParagraphNotFound()
        {
            var resolution = new Resolution();
            var exception = Assert.Throws<OperativeParagraphNotFoundException>(() => 
                resolution.OperativeSection.CreateChangeAmendment(""));
            Assert.NotNull(exception);
        }

        [Test]
        public void TestRemoveOperativeParagraphRemovesChangeAmendments()
        {
            var resolution = new Resolution();
            var paragraph = resolution.OperativeSection.CreateOperativeParagraph("Test");
            var amendment = resolution.OperativeSection.CreateChangeAmendment(paragraph, "New Text");
            resolution.OperativeSection.RemoveOperativeParagraph(paragraph);
            Assert.AreEqual(0, resolution.OperativeSection.ChangeAmendments.Count);
        }
    }
}
