using System.Linq;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Engine;
using MUNityCore.Models.Resolution.V2;
using NUnit.Framework;

namespace MUNityTest.Resolution.V2_Tests
{
    [TestFixture]
    public class ResolutionV2_Tests
    {
        private ResolutionV2 resolution;

        [Test]
        [Order(1)]
        [Author("Peer Conradi")]
        [Description("Test to create the new Resolution object")]
        public void TestCreateResolution()
        {
            resolution = new ResolutionV2();
            Assert.NotNull(resolution);
            Assert.NotNull(resolution.Header);
            Assert.NotNull(resolution.Preamble);
            Assert.NotNull(resolution.OperativeSection);
        }

        [Test]
        [Order(2)]
        [Author("Peer Conradi")]
        [Description("Test setting the Header")]
        public void TestSetTopic()
        {
            resolution.Header.Topic = "Topic";
            resolution.Header.FullName = "Full Document Name";
            resolution.Header.Name = "Document Name";
            resolution.Header.CommitteeName = "Committee Name";
            Assert.AreEqual("Topic", resolution.Header.Topic);
            Assert.AreEqual("Full Document Name", resolution.Header.FullName);
            Assert.AreEqual("Document Name", resolution.Header.Name);
            Assert.AreSame("Committee Name", resolution.Header.CommitteeName);
        }

        [Test]
        [Order(3)]
        [Author("Peer Conradi")]
        [Description("Test Creating a new Preamble Paragraph")]
        public void TestCreatePreambleParagraph()
        {
            var paragraph = new PreambleParagraph();
            paragraph.Text = "Test";
            resolution.Preamble.Paragraphs.Add(paragraph);

            // Test that the Id is generated!
            Assert.False(string.IsNullOrEmpty(paragraph.PreambleParagraphId));
            Assert.NotNull(paragraph.Notices);
            Assert.AreEqual(1, resolution.Preamble.Paragraphs.Count);
            Assert.Contains(paragraph, resolution.Preamble.Paragraphs);
        }

        [Test]
        [Order(4)]
        [Author("Peer Conradi")]
        [Description("Test Change the Preamble Paragraph Text")]
        public void TestChangingParagraphText()
        {
            var paragraph = resolution.Preamble.Paragraphs.First();
            paragraph.Text = "New Text";
            Assert.AreEqual("New Text", paragraph.Text);
        }

        [Test]
        [Order(5)]
        [Author("Peer Conradi")]
        [Description("Test adding a notice to the Preamble Paragraph")]
        public void TestAddANotice()
        {
            var paragraph = resolution.Preamble.Paragraphs.First();
            var notice = new Notice()
            {
                Text = "New Notice"
            };
            paragraph.Notices.Add(notice);
            Assert.False(string.IsNullOrEmpty(notice.Id));
            Assert.NotNull(notice.ReadBy);
            Assert.Contains(notice, paragraph.Notices);
        }

        [Test]
        [Order(6)]
        [Author("Peer Conradi")]
        [Description("Test removing the notice again")]
        public void TestRemoveNotice()
        {
            var paragraph = resolution.Preamble.Paragraphs.First();
            var notice = paragraph.Notices.First();
            paragraph.Notices.Remove(notice);
            Assert.AreEqual(0, paragraph.Notices.Count);
        }

        [Test]
        [Order(7)]
        [Author("Peer Conradi")]
        [Description("Test Removing the Preamble paragraph")]
        public void TestRemovePreambleParagraph()
        {
            var paragraph = resolution.Preamble.Paragraphs.First();
            resolution.Preamble.Paragraphs.Remove(paragraph);
            Assert.Zero(resolution.Preamble.Paragraphs.Count);
        }

        [Test]
        [Order(8)]
        [Author("Peer Conradi")]
        [Description("Test Creating a operative paragraph")]
        public void TestCreateOperativeParagraph()
        {
            var paragraph = new OperativeParagraph();
            resolution.OperativeSection.Paragraphs.Add(paragraph);
            Assert.False(string.IsNullOrEmpty(paragraph.OperativeParagraphId));
        }

        [Test]
        [Order(9)]
        [Author("Peer Conradi")]
        [Description("Test set the operative paragraph text")]
        public void TestSetOperativeParagraphText()
        {
            var paragraph = resolution.OperativeSection.Paragraphs.First();
            paragraph.Text = "New Text";
            Assert.AreEqual("New Text", paragraph.Text);
        }

        [Test]
        [Order(10)]
        [Author("Peer Conradi")]
        [Description("Test creating a Child for the operative paragraph")]
        public void TestCreatingOperativeParagraphChild()
        {
            var paragraph = resolution.OperativeSection.Paragraphs.First();
            var child = new OperativeParagraph();
            paragraph.Children.Add(child);
            Assert.Contains(child, paragraph.Children);
        }
        
    }
}