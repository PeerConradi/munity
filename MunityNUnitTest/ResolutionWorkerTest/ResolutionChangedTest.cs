using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using MUNity.Models.Resolution;
using MUNity.Extensions.ResolutionExtensions;

namespace MunityNUnitTest.ResolutionWorkerTest
{
    public class ResolutionChangedTest
    {
        [Test]
        public void TestCreateInstance()
        {
            var resolution = new Resolution();
            var worker = new MUNity.ServiceWorkers.ResolutionWorker(resolution);
            Assert.NotNull(worker);
        }

        [Test]
        public void ChangeCalledOnHeaderPropertyChangedTest()
        {
            var resolution = new Resolution();
            var worker = new MUNity.ServiceWorkers.ResolutionWorker(resolution);
            bool wasRaised = false;
            worker.ResolutionChanged += delegate { wasRaised = true; };
            resolution.Header.Name = "New Name";
            Assert.IsTrue(wasRaised);
        }

        [Test]
        public void ChangeCalledOnHeaderChangedTest()
        {
            var resolution = new Resolution();
            var worker = new MUNity.ServiceWorkers.ResolutionWorker(resolution);
            bool wasRaised = false;
            worker.ResolutionChanged += delegate { wasRaised = true; };
            resolution.Header = new ResolutionHeader();
            Assert.IsTrue(wasRaised);
        }

        [Test]
        public void ChangeCalledOnPreambleParagraphAdded()
        {
            var resolution = new Resolution();
            var worker = new MUNity.ServiceWorkers.ResolutionWorker(resolution);
            bool wasRaised = false;
            worker.ResolutionChanged += delegate { wasRaised = true; };
            resolution.CreatePreambleParagraph();
            Assert.IsTrue(wasRaised);
        }

        [Test]
        public void ChangeCalledOnPreambleParagraphTextChanged()
        {
            var resolution = new Resolution();
            var paragraph = resolution.CreatePreambleParagraph();
            var worker = new MUNity.ServiceWorkers.ResolutionWorker(resolution);
            bool wasRaised = false;
            worker.ResolutionChanged += delegate { wasRaised = true; };
            paragraph.Text = "Neuer Text";
            Assert.IsTrue(wasRaised);
        }

        [Test]
        public void ChangeCalledOnPreabmeParagraphRemoved()
        {
            var resolution = new Resolution();
            var paragraph = resolution.CreatePreambleParagraph();
            var worker = new MUNity.ServiceWorkers.ResolutionWorker(resolution);
            bool wasRaised = false;
            worker.ResolutionChanged += delegate { wasRaised = true; };
            resolution.Preamble.Paragraphs.Remove(paragraph);
            Assert.IsTrue(wasRaised);
        }

        [Test]
        public void ChangeCalledOnPreambleCommentAdded()
        {
            var resolution = new Resolution();
            var paragraph = resolution.CreatePreambleParagraph("Paragraph");
            var worker = new MUNity.ServiceWorkers.ResolutionWorker(resolution);
            bool wasRaised = false;
            worker.ResolutionChanged += delegate { wasRaised = true; };
            paragraph.Comments.Add(new Comment());
            Assert.IsTrue(wasRaised);
        }

        [Test]
        public void ChangeCalledOnPreambleCommentTextChanged()
        {
            var resolution = new Resolution();
            var paragraph = resolution.CreatePreambleParagraph("Paragraph");
            var comment = new Comment();
            paragraph.Comments.Add(comment);
            var worker = new MUNity.ServiceWorkers.ResolutionWorker(resolution);
            bool wasRaised = false;
            worker.ResolutionChanged += delegate { wasRaised = true; };
            comment.Text = "New Text";
            Assert.IsTrue(wasRaised);
        }

        [Test]
        public void ChangeCalledOnNewOperativeParagraph()
        {
            var resolution = new Resolution();
            var worker = new MUNity.ServiceWorkers.ResolutionWorker(resolution);
            bool wasRaised = false;
            worker.ResolutionChanged += delegate { wasRaised = true; };
            var paragraph = resolution.OperativeSection.CreateOperativeParagraph("New Paragraph");
            Assert.IsTrue(wasRaised);
        }

        [Test]
        public void ChangeCalledOnNewSubOperativeParagraph()
        {
            var resolution = new Resolution();
            var headParagraph = resolution.OperativeSection.CreateOperativeParagraph("Parent");
            bool wasRaised = false;
            var worker = new MUNity.ServiceWorkers.ResolutionWorker(resolution);
            worker.ResolutionChanged += delegate { wasRaised = true; };
            var subParagraph = resolution.OperativeSection.CreateChildParagraph(headParagraph, "Child");
            Assert.IsTrue(wasRaised);
        }

        [Test]
        public void ChangeCalledOnParagraphTextChanged()
        {
            var resolution = new Resolution();
            var headParagraph = resolution.OperativeSection.CreateOperativeParagraph("Parent");
            bool wasRaised = false;
            var worker = new MUNity.ServiceWorkers.ResolutionWorker(resolution);
            worker.ResolutionChanged += delegate { wasRaised = true; };
            headParagraph.Text = "Neuer Text";
            Assert.IsTrue(wasRaised);
        }

        [Test]
        public void ChangeCalledOnNewAddAmendment()
        {
            var resolution = new Resolution();
            bool wasRaised = false;
            var worker = new MUNity.ServiceWorkers.ResolutionWorker(resolution);
            worker.ResolutionChanged += delegate { wasRaised = true; };
            resolution.OperativeSection.CreateAddAmendment(0, "");
            Assert.IsTrue(wasRaised);
        }

        [Test]
        public void ChangeCalledOnNewAddAmendmentInSub()
        {
            var resolution = new Resolution();
            var paragraph = resolution.OperativeSection.CreateOperativeParagraph("Paragraph Head");
            bool wasRaised = false;
            var worker = new MUNity.ServiceWorkers.ResolutionWorker(resolution);
            worker.ResolutionChanged += delegate { wasRaised = true; };
            resolution.OperativeSection.CreateAddAmendment(0, "", paragraph);
            Assert.IsTrue(wasRaised);
        }

        [Test]
        public void ChangeCalledOnNewDeleteAmendment()
        {
            var resolution = new Resolution();
            var paragraph = resolution.OperativeSection.CreateOperativeParagraph("Paragraph One");
            bool wasRaised = false;
            var worker = new MUNity.ServiceWorkers.ResolutionWorker(resolution);
            worker.ResolutionChanged += delegate { wasRaised = true; };
            resolution.OperativeSection.CreateChangeAmendment(paragraph, "New Text");
            Assert.IsTrue(wasRaised);
        }

        [Test]
        public void ChangeCalledOnNewMoveAmendment()
        {
            var resolution = new Resolution();
            var paragraphOne = resolution.OperativeSection.CreateOperativeParagraph("Paragraph One");
            var paragraphTwo = resolution.OperativeSection.CreateOperativeParagraph("Paragraph One");
            bool wasRaised = false;
            var worker = new MUNity.ServiceWorkers.ResolutionWorker(resolution);
            worker.ResolutionChanged += delegate { wasRaised = true; };
            resolution.OperativeSection.CreateMoveAmendment(paragraphOne, 1);
            Assert.IsTrue(wasRaised);
        }

        [Test]
        public void ChangeCalledOnNewChangeAmendment()
        {
            var resolution = new Resolution();
            var paragraphOne = resolution.OperativeSection.CreateOperativeParagraph("Paragraph One");
            bool wasRaised = false;
            var worker = new MUNity.ServiceWorkers.ResolutionWorker(resolution);
            worker.ResolutionChanged += delegate { wasRaised = true; };
            resolution.OperativeSection.CreateChangeAmendment(paragraphOne, "New Text");
            Assert.IsTrue(wasRaised);
        }

    }
}
