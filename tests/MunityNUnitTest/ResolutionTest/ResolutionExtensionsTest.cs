using MUNity.Models.Resolution;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using MUNity.Extensions.ResolutionExtensions;

namespace MunityNUnitTest.ResolutionTest
{
    public class ResolutionExtensionsTest
    {
        [Test]
        public void TestFirstLevelWhere()
        {
            var resolution = new Resolution();
            var paragraphOne = resolution.OperativeSection.CreateOperativeParagraph("Test 1");
            var paragraphTwo = resolution.OperativeSection.CreateOperativeParagraph("Rolf");
            var paragraphThree = resolution.OperativeSection.CreateOperativeParagraph("Test 2");
            var result = resolution.OperativeSection.WhereParagraph(n => n.Text.Contains("Test"));
            Assert.AreEqual(2, result.Count);
            Assert.Contains(paragraphOne, result);
            Assert.Contains(paragraphThree, result);
        }

        [Test]
        public void TestWhereSubLevel()
        {
            var resolution = new Resolution();
            var paragraphOne = resolution.OperativeSection.CreateOperativeParagraph("Paragraph 1");
            var subOne = resolution.OperativeSection.CreateChildParagraph(paragraphOne, "Child 1");
            var paragraphTwo = resolution.OperativeSection.CreateOperativeParagraph("Paragraph 2");
            var subTwo = resolution.OperativeSection.CreateChildParagraph(paragraphTwo, "Child 2");
            var subThree = resolution.OperativeSection.CreateChildParagraph(paragraphTwo, "Child 3");
            var result = resolution.OperativeSection.WhereParagraph(n => n.Text.Contains("Child"));
            Assert.AreEqual(3, result.Count);
            Assert.Contains(subOne, result);
            Assert.Contains(subTwo, result);
            Assert.Contains(subThree, result);
        }

        [Test]
        public void TestFirstOrDefaultLevelOne()
        {
            var resolution = new Resolution();
            var paragraphOne = resolution.OperativeSection.CreateOperativeParagraph("Paragraph 1");
            var paragraphTwo = resolution.OperativeSection.CreateOperativeParagraph("Paragraph 2");
            var paragraphThree = resolution.OperativeSection.CreateOperativeParagraph("Paragraph 3");
            var result = resolution.OperativeSection.FirstOrDefault(n => n.Text == "Paragraph 2");
            Assert.NotNull(result);
            Assert.AreEqual(paragraphTwo, result);
        }

        [Test]
        public void TestFirstOrDefaultInSub()
        {
            var resolution = new Resolution();
            var paragraphOne = resolution.OperativeSection.CreateOperativeParagraph("Paragraph 1");
            var subOne = resolution.OperativeSection.CreateChildParagraph(paragraphOne, "Child 1");
            var paragraphTwo = resolution.OperativeSection.CreateOperativeParagraph("Paragraph 2");
            var subTwo = resolution.OperativeSection.CreateChildParagraph(paragraphTwo, "Child 2");
            var subThree = resolution.OperativeSection.CreateChildParagraph(paragraphTwo, "Child 3");
            var result = resolution.OperativeSection.FirstOrDefault(n => n.Text == "Child 2");
            Assert.NotNull(result);
            Assert.AreEqual(subTwo, result);
        }
    }
}
