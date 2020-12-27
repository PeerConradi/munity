using MUNityCore.Models.Resolution.V2;
using MUNitySchema.Models.Resolution;
using NUnit.Framework;

namespace MUNityTest.Resolution.V2_Tests
{
    [TestFixture]
    public class V2DeleteAmendmentTest
    {
        [Test]
        [Author("Peer Conradi")]
        [Description("Test creating instance of Delete Amendment")]
        public void TestCreateDeleteAmendment()
        {
            var instance = new DeleteAmendment();
            Assert.NotNull(instance);
            Assert.False(string.IsNullOrEmpty(instance.Id));
        }

        [Test]
        [Author("Peer Conradi")]
        [Description("Test add a delete Amendment to a resolution")]
        public void TestAddDeleteAmendmentToAResolution()
        {
            // Setup
            var resolution = new MUNitySchema.Models.Resolution.Resolution();
            var paragraph = new OperativeParagraph();
            resolution.OperativeSection.Paragraphs.Add(paragraph);

            var amendment = new DeleteAmendment
            {
                TargetSectionId = paragraph.OperativeParagraphId
            };
            
            resolution.OperativeSection.DeleteAmendments.Add(amendment);
            Assert.Contains(amendment, resolution.OperativeSection.DeleteAmendments);
        }

        [Test]
        [Author("Peer Conradi")]
        [Description("Test Apply the delete amendment. This should remove the delete Amendment and target section")]
        public void TestApplyDeleteAmendment()
        {
            // Setup
            var resolution = new MUNitySchema.Models.Resolution.Resolution();
            var paragraph = new OperativeParagraph();
            resolution.OperativeSection.Paragraphs.Add(paragraph);

            var amendment = new DeleteAmendment
            {
                TargetSectionId = paragraph.OperativeParagraphId
            };
            resolution.OperativeSection.DeleteAmendments.Add(amendment);

            amendment.Apply(resolution.OperativeSection);
            Assert.Zero(resolution.OperativeSection.Paragraphs.Count);
            Assert.Zero(resolution.OperativeSection.DeleteAmendments.Count);
        }

        [Test]
        [Author("Peer Conradi")]
        [Description("Test deny the delete amendment should remove all other delete amendments")]
        public void TestDenyDeleteAmendment()
        {
            // Setup
            var resolution = new MUNitySchema.Models.Resolution.Resolution();
            var paragraph = new OperativeParagraph();
            resolution.OperativeSection.Paragraphs.Add(paragraph);

            var amendmentOne = new DeleteAmendment
            {
                TargetSectionId = paragraph.OperativeParagraphId
            };

            var amendmentTwo = new DeleteAmendment
            {
                TargetSectionId = paragraph.OperativeParagraphId
            };
            resolution.OperativeSection.DeleteAmendments.Add(amendmentOne);
            resolution.OperativeSection.DeleteAmendments.Add(amendmentTwo);

            amendmentOne.Deny(resolution.OperativeSection);
            Assert.Contains(paragraph, resolution.OperativeSection.Paragraphs);
            Assert.Zero(resolution.OperativeSection.DeleteAmendments.Count);
        }
    }
}