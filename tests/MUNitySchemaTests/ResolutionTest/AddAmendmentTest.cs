using MUNity.Models.Resolution;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using MUNity.Extensions.ResolutionExtensions;
using System.Linq;

namespace MunityNUnitTest.ResolutionTest
{
    /// <summary>
    /// Test cases for the Amendment Type to add a new operative paragraph.
    /// </summary>
    public class AddAmendmentTest
    {
        /// <summary>
        /// Test creating an instance of this type of amendment.
        /// </summary>
        [Test]
        public void TestCreateInstance()
        {
            var resolution = new Resolution();
            var paragraphOne = resolution.OperativeSection.CreateOperativeParagraph();
            var amendment = resolution.OperativeSection.CreateAddAmendment(1, "New Paragraph");
            Assert.NotNull(amendment);
            Assert.AreEqual(1, resolution.OperativeSection.AddAmendments.Count);
            Assert.AreEqual(2, resolution.OperativeSection.Paragraphs.Count);
            Assert.AreEqual(paragraphOne, resolution.OperativeSection.Paragraphs[0]);
            Assert.IsTrue(resolution.OperativeSection.Paragraphs[1].IsVirtual);
            Assert.Contains(amendment, resolution.OperativeSection.AddAmendments);
        }

        /// <summary>
        /// Test that a new created instance of this amendment type has an id.
        /// </summary>
        [Test]
        public void TestInstanceHasId()
        {
            var amendment = new AddAmendment();
            Assert.IsFalse(string.IsNullOrEmpty(amendment.Id));
        }

        /// <summary>
        /// Test that two created amendments dont have the same id.
        /// </summary>
        [Test]
        public void TestIdsAreDifferent()
        {
            var amendmentOne = new AddAmendment();
            var amendmentTwo = new AddAmendment();
            Assert.IsFalse(amendmentOne.Id == amendmentTwo.Id);
        }

        /// <summary>
        /// Test that this amendment can be applied and will create a new paragraph.
        /// </summary>
        [Test]
        public void TestApplyAmendment()
        {
            var resolution = new Resolution();
            var paragraphOne = resolution.OperativeSection.CreateOperativeParagraph();
            var amendment = resolution.OperativeSection.CreateAddAmendment(1, "New Paragraph");
            amendment.Apply(resolution.OperativeSection);
            Assert.AreEqual(2, resolution.OperativeSection.Paragraphs.Count);
            Assert.AreEqual(paragraphOne, resolution.OperativeSection.Paragraphs[0]);
            Assert.AreEqual("New Paragraph", resolution.OperativeSection.Paragraphs[1].Text);
        }

        /// <summary>
        /// Test that when this amendment is applied the target paragraph is not virtual anymore.
        /// </summary>
        [Test]
        public void TestApplyAmendmentVirtualFalse()
        {
            var resolution = new Resolution();
            var paragraphOne = resolution.OperativeSection.CreateOperativeParagraph();
            var amendment = resolution.OperativeSection.CreateAddAmendment(1, "New Paragraph");
            amendment.Apply(resolution.OperativeSection);
            Assert.IsFalse(resolution.OperativeSection.Paragraphs[1].IsVirtual);
        }

        /// <summary>
        /// Test that when this amendment is applied the amendment is removed from the list.
        /// </summary>
        [Test]
        public void TestApplyAmendmentRemovesAmendment()
        {
            var resolution = new Resolution();
            var paragraphOne = resolution.OperativeSection.CreateOperativeParagraph();
            var amendment = resolution.OperativeSection.CreateAddAmendment(1, "New Paragraph");
            amendment.Apply(resolution.OperativeSection);
            Assert.IsFalse(resolution.OperativeSection.AddAmendments.Contains(amendment));
        }

        /// <summary>
        /// Test that when this amendment is deleted the paragraph is also deleted.
        /// </summary>
        [Test]
        public void TestDeleteAddAmendment()
        {
            var resolution = new Resolution();
            var paragraphOne = resolution.OperativeSection.CreateOperativeParagraph();
            var amendment = resolution.OperativeSection.CreateAddAmendment(1, "New Paragraph");
            resolution.OperativeSection.RemoveAmendment(amendment);
            Assert.AreEqual(1, resolution.OperativeSection.Paragraphs.Count);
        }

        /// <summary>
        /// Test that deleting the amendment will remove the amendment from the Add Amendment List of the oeprative section.
        /// </summary>
        [Test]
        public void TestDeleteAddAmendmentRemovesAmendment()
        {
            var resolution = new Resolution();
            var amendment = resolution.OperativeSection.CreateAddAmendment(0, "New Paragraph");
            resolution.OperativeSection.RemoveAmendment(amendment);
            Assert.IsFalse(resolution.OperativeSection.AddAmendments.Contains(amendment));
        }

        /// <summary>
        /// Test that deny the amendment is removing the amendment from the list.
        /// </summary>
        [Test]
        public void TestDenyAmendmentRemovesAmendment()
        {
            var resolution = new Resolution();
            var amendment = resolution.OperativeSection.CreateAddAmendment(0, "New Paragraph");
            amendment.Deny(resolution.OperativeSection);
            Assert.IsFalse(resolution.OperativeSection.AddAmendments.Contains(amendment));
        }

        /// <summary>
        /// Test that denying the amendment will remove the created virtual operative paragraph.
        /// </summary>
        [Test]
        public void TestDenyAmendmentRemovesVirtualParagraph()
        {
            var resolution = new Resolution();
            var amendment = resolution.OperativeSection.CreateAddAmendment(0, "New Paragraph");
            amendment.Deny(resolution.OperativeSection);
            Assert.IsFalse(resolution.OperativeSection.Paragraphs.Any());
        }
    }
}
