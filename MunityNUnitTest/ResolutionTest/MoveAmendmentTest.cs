using MUNity.Models.Resolution;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using MUNity.Extensions.ResolutionExtensions;
using System.Linq;

namespace MunityNUnitTest.ResolutionTest
{
    public class MoveAmendmentTest
    {
        [Test]
        public void TestCreateInstance()
        {
            var resolution = new Resolution();
            var paragraphOne = resolution.OperativeSection.CreateOperativeParagraph("Paragraph One");
            var paragraphTwo = resolution.OperativeSection.CreateOperativeParagraph("Paragraph Two");
            var moveAmendment = resolution.OperativeSection.CreateMoveAmendment(paragraphOne, 1);
            Assert.NotNull(moveAmendment);
            Assert.Contains(moveAmendment, resolution.OperativeSection.MoveAmendments);
            Assert.AreEqual(1, resolution.OperativeSection.MoveAmendments.Count);
            Assert.AreEqual(3, resolution.OperativeSection.Paragraphs.Count);
            Assert.AreEqual(2, resolution.OperativeSection.Paragraphs.Count(n => !n.IsVirtual));
        }

        [Test]
        public void TestRemoveAmendment()
        {
            var resolution = new Resolution();
            var paragraphOne = resolution.OperativeSection.CreateOperativeParagraph("Paragraph One");
            var paragraphTwo = resolution.OperativeSection.CreateOperativeParagraph("Paragraph Two");
            var moveAmendment = resolution.OperativeSection.CreateMoveAmendment(paragraphOne, 1);
            resolution.OperativeSection.RemoveAmendment(moveAmendment);
            Assert.IsFalse(resolution.OperativeSection.MoveAmendments.Contains(moveAmendment));
            Assert.AreEqual(2, resolution.OperativeSection.Paragraphs.Count);
            Assert.AreEqual(2, resolution.OperativeSection.Paragraphs.Count(n => !n.IsVirtual));
        }

        [Test]
        public void TestApplyAmendment()
        {
            var resolution = new Resolution();
            var paragraphOne = resolution.OperativeSection.CreateOperativeParagraph("Paragraph One");
            var paragraphTwo = resolution.OperativeSection.CreateOperativeParagraph("Paragraph Two");
            var moveAmendment = resolution.OperativeSection.CreateMoveAmendment(paragraphOne, 2);
            var success = moveAmendment.Apply(resolution.OperativeSection);
            Assert.IsTrue(success);
            Assert.AreEqual(2, resolution.OperativeSection.Paragraphs.Count);
            Assert.AreEqual(2, resolution.OperativeSection.Paragraphs.Count(n => !n.IsVirtual));
            var firstParagraph = resolution.OperativeSection.Paragraphs[0];
            var secondParagraph = resolution.OperativeSection.Paragraphs[1];
            // Paragraph should have moved up
            Assert.AreEqual("Paragraph Two", firstParagraph.Text);
            Assert.AreEqual("Paragraph One", secondParagraph.Text);
            // Alle Daten sollten nun mit dem Platzhalter ausgetauscht sein.
        }

        [Test]
        public void TestMoveAmendmentCorrentWhenAmendmentAdded()
        {
            // Paragraph One
            // ParagraphTwo
            // [Move Paragraph One here]
            // Paragraph Three
            var resolution = new Resolution();
            var paragraphOne = resolution.OperativeSection.CreateOperativeParagraph("Paragraph One");
            var paragraphTwo = resolution.OperativeSection.CreateOperativeParagraph("Paragraph Two");
            var moveAmendment = resolution.OperativeSection.CreateMoveAmendment(paragraphOne, 2);
            var paragraphThree = resolution.OperativeSection.CreateOperativeParagraph("Paragraph Three");
            var realParagraphs = resolution.OperativeSection.Paragraphs.Where(n => n.IsVirtual == false);
            Assert.AreEqual("Paragraph One", realParagraphs.ElementAt(0).Text);
            Assert.AreEqual("Paragraph Two", realParagraphs.ElementAt(1).Text);
            Assert.AreEqual("Paragraph Three", realParagraphs.ElementAt(2).Text);

            var allParagraphs = resolution.OperativeSection.Paragraphs;
            Assert.AreEqual("Paragraph One", allParagraphs.ElementAt(0).Text);
            Assert.AreEqual("Paragraph Two", allParagraphs.ElementAt(1).Text);
            Assert.IsTrue(allParagraphs.ElementAt(2).IsVirtual);
            Assert.AreEqual("Paragraph Three", allParagraphs.ElementAt(3).Text);
        }

        [Test]
        public void TestMoveToStart()
        {
            var resolution = new Resolution();
            var paragraphOne = resolution.OperativeSection.CreateOperativeParagraph("Paragraph One");
            var paragraphTwo = resolution.OperativeSection.CreateOperativeParagraph("Paragraph Two");
            var moveAmendment = resolution.OperativeSection.CreateMoveAmendment(paragraphTwo, 0);
            var success = moveAmendment.Apply(resolution.OperativeSection);
            var firstParagraph = resolution.OperativeSection.Paragraphs[0];
            var secondParagraph = resolution.OperativeSection.Paragraphs[1];
            // Paragraph should have moved up
            Assert.AreEqual("Paragraph Two", firstParagraph.Text);
            Assert.AreEqual("Paragraph One", secondParagraph.Text);
        }

        [Test]
        public void TestCreateMoveToSub()
        {
            var resolution = new Resolution();
            var paragraphOne = resolution.OperativeSection.CreateOperativeParagraph("Paragraph One");
            var paragraphTwo = resolution.OperativeSection.CreateOperativeParagraph("Paragraph Two");
            var moveAmendment = resolution.OperativeSection.CreateMoveAmendment(paragraphTwo, 0, paragraphOne);
            Assert.AreEqual(2, resolution.OperativeSection.Paragraphs.Count);
            Assert.AreEqual(1, paragraphOne.Children.Count);
            Assert.IsTrue(paragraphOne.Children.Any(n => n.OperativeParagraphId == moveAmendment.NewTargetSectionId));
        }

        [Test]
        public void TestApplyMoveToSub()
        {
            var resolution = new Resolution();
            var paragraphOne = resolution.OperativeSection.CreateOperativeParagraph("Paragraph One");
            var paragraphTwo = resolution.OperativeSection.CreateOperativeParagraph("Paragraph Two");
            var moveAmendment = resolution.OperativeSection.CreateMoveAmendment(paragraphTwo, 0, paragraphOne);
            var success = moveAmendment.Apply(resolution.OperativeSection);
            Assert.IsTrue(success);
            Assert.AreEqual(1, resolution.OperativeSection.Paragraphs.Count);
            Assert.AreEqual(1, paragraphOne.Children.Count);
        }

        [Test]
        public void TestCreateMoveSubToMaster()
        {
            var resolution = new Resolution();
            var paragraphOne = resolution.OperativeSection.CreateOperativeParagraph("Paragraph 1");
            var subParagraph = resolution.OperativeSection.CreateChildParagraph(paragraphOne, "Paragraph 1.a");
            var moveAmendment = resolution.OperativeSection.CreateMoveAmendment(subParagraph, resolution.OperativeSection.Paragraphs.Count);
            Assert.AreEqual(2, resolution.OperativeSection.Paragraphs.Count);
            Assert.IsTrue(resolution.OperativeSection.Paragraphs[1].IsVirtual);
        }

        [Test]
        public void TestApplyMoveSubToMaster()
        {
            var resolution = new Resolution();
            var paragraphOne = resolution.OperativeSection.CreateOperativeParagraph("Paragraph 1");
            var subParagraph = resolution.OperativeSection.CreateChildParagraph(paragraphOne, "Paragraph 1.a");
            var moveAmendment = resolution.OperativeSection.CreateMoveAmendment(subParagraph, resolution.OperativeSection.Paragraphs.Count);
            var result = moveAmendment.Apply(resolution.OperativeSection);
            Assert.IsTrue(result);
            Assert.AreEqual(2, resolution.OperativeSection.Paragraphs.Count);
            Assert.IsTrue(resolution.OperativeSection.Paragraphs.All(n => !n.IsVirtual));
        }

        [Test]
        public void TestCreateMoveToMiddle()
        {
            var resolution = new Resolution();
            var paragraphOne = resolution.OperativeSection.CreateOperativeParagraph("Paragraph 1");
            var paragraphTwo = resolution.OperativeSection.CreateOperativeParagraph("Paragraph 2");
            var paragraphThree = resolution.OperativeSection.CreateOperativeParagraph("Paragraph 3");
            var amendment = resolution.OperativeSection.CreateMoveAmendment(paragraphThree, 1);
            Assert.NotNull(amendment);
            Assert.AreEqual(4, resolution.OperativeSection.Paragraphs.Count);
            Assert.AreEqual(paragraphOne, resolution.OperativeSection.Paragraphs[0]);
            Assert.AreEqual(amendment.NewTargetSectionId, resolution.OperativeSection.Paragraphs[1].OperativeParagraphId);
            Assert.IsTrue(resolution.OperativeSection.Paragraphs[1].IsVirtual);
            Assert.AreEqual(paragraphTwo, resolution.OperativeSection.Paragraphs[2]);
            Assert.AreEqual(paragraphThree, resolution.OperativeSection.Paragraphs[3]);
        }

        [Test]
        public void TestApplyMoveToMiddle()
        {
            var resolution = new Resolution();
            var paragraphOne = resolution.OperativeSection.CreateOperativeParagraph("Paragraph 1");
            var paragraphTwo = resolution.OperativeSection.CreateOperativeParagraph("Paragraph 2");
            var paragraphThree = resolution.OperativeSection.CreateOperativeParagraph("Paragraph 3");
            var amendment = resolution.OperativeSection.CreateMoveAmendment(paragraphThree, 1);
            var success = amendment.Apply(resolution.OperativeSection);
            Assert.IsTrue(success);
            Assert.AreEqual(3, resolution.OperativeSection.Paragraphs.Count);
            Assert.IsTrue(resolution.OperativeSection.Paragraphs.All(n => !n.IsVirtual));
            Assert.AreEqual("Paragraph 1", resolution.OperativeSection.Paragraphs[0].Text);
            Assert.AreEqual("Paragraph 3", resolution.OperativeSection.Paragraphs[1].Text);
            Assert.AreEqual("Paragraph 2", resolution.OperativeSection.Paragraphs[2].Text);
        }

        [Test]
        public void TestCreateMoveInsideOfSub()
        {
            var resolution = new Resolution();
            var paragraphOne = resolution.OperativeSection.CreateOperativeParagraph("Paragraph 1");
            var subOne = resolution.OperativeSection.CreateChildParagraph(paragraphOne, "Paragraph 1.a");
            var subTwo = resolution.OperativeSection.CreateChildParagraph(paragraphOne, "Paragraph 1.b");
            var subThree = resolution.OperativeSection.CreateChildParagraph(paragraphOne, "Paragraph 1.c");
            var amendment = resolution.OperativeSection.CreateMoveAmendment(subThree, 0, paragraphOne);
            Assert.NotNull(amendment);
            Assert.AreEqual(4, paragraphOne.Children.Count);
            Assert.IsTrue(paragraphOne.Children[0].IsVirtual);
            Assert.AreEqual(subOne, paragraphOne.Children[1]);
            Assert.AreEqual(subTwo, paragraphOne.Children[2]);
            Assert.AreEqual(subThree, paragraphOne.Children[3]);
        }

        [Test]
        public void TestApplyMoveInsideOfSub()
        {
            var resolution = new Resolution();
            var paragraphOne = resolution.OperativeSection.CreateOperativeParagraph("Paragraph 1");
            var subOne = resolution.OperativeSection.CreateChildParagraph(paragraphOne, "Paragraph 1.a");
            var subTwo = resolution.OperativeSection.CreateChildParagraph(paragraphOne, "Paragraph 1.b");
            var subThree = resolution.OperativeSection.CreateChildParagraph(paragraphOne, "Paragraph 1.c");
            var amendment = resolution.OperativeSection.CreateMoveAmendment(subThree, 0, paragraphOne);
            var success = amendment.Apply(resolution.OperativeSection);
            Assert.IsTrue(success);
            Assert.AreEqual("Paragraph 1.c", paragraphOne.Children[0].Text);
            Assert.AreEqual("Paragraph 1.a", paragraphOne.Children[1].Text);
            Assert.AreEqual("Paragraph 1.b", paragraphOne.Children[2].Text);
        }
    }
}
