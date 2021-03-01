using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Bunit;
using Moq;
using Microsoft.Extensions.DependencyInjection;

namespace MunityClientTest.ResolutionTests
{
    public class ResolutionHomeTest
    {
        [Fact]
        public void TestHomeHasNoLocalResolution()
        {
            var mockedService = new Mock<MUNityClient.Services.IResolutionService>();

            mockedService.Setup(n => n.GetStoredResolutions()).ReturnsAsync(() => new List<MUNityClient.Models.Resolution.ResolutionInfo>());

            using var ctx = new TestContext();
            ctx.Services.AddSingleton(mockedService.Object);

            var home = ctx.RenderComponent<MUNityClient.Pages.Resa.ResolutionHome>();
            Assert.NotNull(home);
            home.CompareTo("<p>Es wurden keine Lokal gespeicherten Resolutionen gefunden</p>");
        }
    }
}
