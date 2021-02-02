using System;
using System.Collections.Generic;
using System.Text;
using MUNity.Extensions.ResolutionExtensions;
using MUNity.Models.Resolution;
using NUnit.Framework;

namespace MunityNUnitTest.ResolutionWorkerTest
{
    public class OperativeSectionChangedTest
    {
        //[Test]
        //public void TestCalledOnSectionReplaced()
        //{
        //    var resolution = new Resolution();
        //    bool wasRaised = false;
        //    var worker = new MUNity.ServiceWorkers.ResolutionWorker(resolution);
        //    worker.OperativeSectionChanged += delegate { wasRaised = true; };
        //    resolution.OperativeSection = new OperativeSection();
        //    Assert.IsTrue(wasRaised);
        //}

        [Test]
        public void TestCalledOnParagraphAdded()
        {
            var resolution = new Resolution();
            bool wasRaised = false;
            var worker = new MUNity.ServiceWorkers.ResolutionWorker(resolution);
            worker.OperativeSectionChanged += delegate { wasRaised = true; };
            resolution.OperativeSection.CreateOperativeParagraph();
            Assert.IsTrue(wasRaised);
        }

        [Test]
        public void TestCalledOnAddAmendmendmentCreated()
        {
            var resolution = new Resolution();
            bool wasRaised = false;
            var worker = new MUNity.ServiceWorkers.ResolutionWorker(resolution);
            worker.OperativeSectionChanged += delegate { wasRaised = true; };
            resolution.OperativeSection.CreateAddAmendment(0, "new paragraph!");
            Assert.IsTrue(wasRaised);
        }
    }
}
