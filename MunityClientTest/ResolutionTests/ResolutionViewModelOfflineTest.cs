using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using MUNity.Models.Resolution;
using MUNityClient.ViewModel;
using System.Linq;
using Moq;

namespace MunityClientTest.ResolutionTests
{
    public class ResolutionViewModelOfflineTest
    {
        Moq.Mock<MUNityClient.Services.IResolutionService> _resolutionService;

        [SetUp]
        public void Setup()
        {
            this._resolutionService = new Moq.Mock<MUNityClient.Services.IResolutionService>();
        }

        [Test]
        public void TestCreateInstance()
        {
            var resolution = new MUNity.Models.Resolution.Resolution();
            var viewModel = ResolutionViewModel.CreateViewModelOffline(resolution, _resolutionService.Object);
            Assert.NotNull(viewModel);
        }

        //[Test]
        //public void TestAddPreambleParagraphOffline()
        //{
        //    var resolution = new Resolution();
        //    bool saveWasRaised = false;
        //    _resolutionService.Setup(n => n.SaveOfflineResolution(It.IsAny<Resolution>())).Callback(delegate { saveWasRaised = true; });
        //    var viewModel = ResolutionViewModel.CreateViewModelOffline(resolution, _resolutionService.Object);
        //    bool wasRaised = false;
        //    viewModel.PreambleParagraphAdded += delegate { wasRaised = true; };
        //    viewModel.AddPreambleParagraph();
        //    Assert.IsTrue(wasRaised);
        //    Assert.IsTrue(saveWasRaised);
        //    Assert.IsTrue(viewModel.Resolution.Preamble.Paragraphs.Any());
        //}
    }
}
