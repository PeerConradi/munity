using MUNity.Models.Resolution;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using MUNity.Extensions.ResolutionExtensions;
using MUNity.Exceptions.Resolution;

namespace MunityNUnitTest.ResolutionTest
{
    public class ResolutionGeneralTest
    {
        [Test]
        public void TestCreateResolutionInstance()
        {
            var instance = new Resolution();
            Assert.NotNull(instance);
            Assert.NotNull(instance.Header);
            Assert.NotNull(instance.Preamble);
            Assert.NotNull(instance.Preamble.Paragraphs);
            Assert.NotNull(instance.OperativeSection);
            Assert.NotNull(instance.OperativeSection.Paragraphs, "Expecting a new created Resolution to have at least an empty list in paragraphs");
            Assert.NotNull(instance.OperativeSection.AddAmendments);
            Assert.NotNull(instance.OperativeSection.ChangeAmendments);
            Assert.NotNull(instance.OperativeSection.DeleteAmendments);
            Assert.NotNull(instance.OperativeSection.MoveAmendments);
        }

        [Test]
        public void TestCanCreatePreambleParagraph()
        {
            var instance = new Resolution();
            var paragraph = instance.CreatePreambleParagraph();
            Assert.NotNull(paragraph);
            Assert.Contains(paragraph, instance.Preamble.Paragraphs);
        }

        [Test]
        public void TestCanCreateOperativeParagraph()
        {
            var instance = new Resolution();
            var paragraph = instance.OperativeSection.CreateOperativeParagraph();
            Assert.NotNull(paragraph);
            Assert.Contains(paragraph, instance.OperativeSection.Paragraphs);
        }

        [Test]
        public void FindTopLevelOperativeParagraph()
        {
            var resoltution = new Resolution();
            var paragraphOne = resoltution.OperativeSection.CreateOperativeParagraph();
            var paragraphTwo = resoltution.OperativeSection.CreateOperativeParagraph();
            var result = resoltution.OperativeSection.FindOperativeParagraph(paragraphOne.OperativeParagraphId);
            Assert.NotNull(result);
            Assert.AreEqual(paragraphOne, result);
        }

        [Test]
        public void FindSecondLevelOperativeParagraph()
        {
            var resolution = new Resolution();
            var paragraphOne = resolution.OperativeSection.CreateOperativeParagraph();
            var paragraphOneSubone = resolution.OperativeSection.CreateChildParagraph(paragraphOne);
            var result = resolution.OperativeSection.FindOperativeParagraph(paragraphOneSubone.OperativeParagraphId);
            Assert.NotNull(result);
            Assert.AreEqual(paragraphOneSubone, result);
        }

        [Test]
        public void TestGetFirstLevelPath()
        {
            var resolution = new Resolution();
            var paragraphOne = resolution.OperativeSection.CreateOperativeParagraph();
            var path = resolution.OperativeSection.GetOperativeParagraphPath(paragraphOne.OperativeParagraphId);
            Assert.AreEqual(1, path.Count);
            Assert.AreEqual(paragraphOne, path[0]);
        }

        [Test]
        public void TestGetSecondLevelPath()
        {
            var resolution = new Resolution();
            var paragraphOne = resolution.OperativeSection.CreateOperativeParagraph("1");
            var paragraphOneSubone = resolution.OperativeSection.CreateChildParagraph(paragraphOne, "1.a");
            var path = resolution.OperativeSection.GetOperativeParagraphPath(paragraphOneSubone.OperativeParagraphId);
            Assert.AreEqual(2, path.Count);
            Assert.AreEqual(paragraphOne, path[0]);
            Assert.AreEqual(paragraphOneSubone, path[1]);
        }

        [Test]
        public void TestGetThirdLevelPath()
        {
            var resolution = new Resolution();
            var paragraphOne = resolution.OperativeSection.CreateOperativeParagraph("1");
            var paragraphOneSubone = resolution.OperativeSection.CreateChildParagraph(paragraphOne, "1.a");
            var paragraphLevelThree = resolution.OperativeSection.CreateChildParagraph(paragraphOneSubone, "1.a.i");
            var path = resolution.OperativeSection.GetOperativeParagraphPath(paragraphLevelThree.OperativeParagraphId);
            Assert.AreEqual(3, path.Count);
            Assert.AreEqual(paragraphOne, path[0]);
            Assert.AreEqual(paragraphOneSubone, path[1]);
            Assert.AreEqual(paragraphLevelThree, path[2]);
        }

        [Test]
        public void TestFirstLevelPathnames()
        {
            var resolution = new Resolution();
            var paragraphOne = resolution.OperativeSection.CreateOperativeParagraph();
            var paragraphTwo = resolution.OperativeSection.CreateOperativeParagraph();
            var pathOne = resolution.OperativeSection.GetIndexNameOfOperativeParagraph(paragraphOne);
            var pathTwo = resolution.OperativeSection.GetIndexNameOfOperativeParagraph(paragraphTwo);
            Assert.AreEqual("1", pathOne);
            Assert.AreEqual("2", pathTwo);
        }

        [Test]
        public void TestSecondLevelPathnames()
        {
            var resolution = new Resolution();
            var paragraphOne = resolution.OperativeSection.CreateOperativeParagraph();
            var paragraphTwo = resolution.OperativeSection.CreateOperativeParagraph();
            var subOne = resolution.OperativeSection.CreateChildParagraph(paragraphOne);
            var pathOne = resolution.OperativeSection.GetIndexNameOfOperativeParagraph(paragraphOne);
            var pathOneOne = resolution.OperativeSection.GetIndexNameOfOperativeParagraph(subOne);
            var pathTwo = resolution.OperativeSection.GetIndexNameOfOperativeParagraph(paragraphTwo);
            Assert.AreEqual("1", pathOne);
            Assert.AreEqual("1.a", pathOneOne);
            Assert.AreEqual("2", pathTwo);
        }

        [Test]
        public void TestThirdLEvelPathnames()
        {
            var resolution = new Resolution();
            var paragraphOne = resolution.OperativeSection.CreateOperativeParagraph();
            var subOne = resolution.OperativeSection.CreateChildParagraph(paragraphOne);
            var subSubOne = resolution.OperativeSection.CreateChildParagraph(subOne);
            var paragraphTwo = resolution.OperativeSection.CreateOperativeParagraph();

            var pathOne = resolution.OperativeSection.GetIndexNameOfOperativeParagraph(paragraphOne);
            var pathOneOne = resolution.OperativeSection.GetIndexNameOfOperativeParagraph(subOne);
            var pathOneai = resolution.OperativeSection.GetIndexNameOfOperativeParagraph(subSubOne);
            var pathTwo = resolution.OperativeSection.GetIndexNameOfOperativeParagraph(paragraphTwo);
            Assert.AreEqual("1", pathOne);
            Assert.AreEqual("1.a", pathOneOne);
            Assert.AreEqual("1.a.i", pathOneai);
            Assert.AreEqual("2", pathTwo);
        }

        [Test]
        public void TestGetAllParagraphInfos()
        {
            var resolution = new Resolution();
            var paragraphOne = resolution.OperativeSection.CreateOperativeParagraph("Paragraph 1");
            var subOne = resolution.OperativeSection.CreateChildParagraph(paragraphOne, "Paragraph 1.a");
            var subSubOne = resolution.OperativeSection.CreateChildParagraph(subOne, "Paragraph 1.a.i");
            var paragraphTwo = resolution.OperativeSection.CreateOperativeParagraph("Paragraph 2");
            var infos = resolution.GetRealOperativeParagraphsInfo();
            Assert.AreEqual(4, infos.Count);
            // Check 1
            Assert.AreEqual(paragraphOne.OperativeParagraphId, infos[0].id);
            Assert.AreEqual("1", infos[0].path);
            Assert.AreEqual("Paragraph 1", infos[0].text);

            // Check 1.a
            Assert.AreEqual(subOne.OperativeParagraphId, infos[1].id);
            Assert.AreEqual("1.a", infos[1].path);
            Assert.AreEqual("Paragraph 1.a", infos[1].text);

            // Check 1.a.i
            Assert.AreEqual(subSubOne.OperativeParagraphId, infos[2].id);
            Assert.AreEqual("1.a.i", infos[2].path);
            Assert.AreEqual("Paragraph 1.a.i", infos[2].text);

            // Check 2
            Assert.AreEqual(paragraphTwo.OperativeParagraphId, infos[3].id);
            Assert.AreEqual("2", infos[3].path);
            Assert.AreEqual("Paragraph 2", infos[3].text);
        }

        [Test]
        public void TestRemoveUnknownAmendmentType()
        {
            var resolution = new Resolution();
            var fakeType = new FakeAmendmentType();
            Assert.Throws<UnsupportedAmendmentTypeException>(() => resolution.OperativeSection.RemoveAmendment(fakeType));
        }
    }

    public class FakeAmendmentType : AbstractAmendment
    {

    }
}
