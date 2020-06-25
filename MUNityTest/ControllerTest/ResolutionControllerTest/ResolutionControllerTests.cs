using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Moq;
using MUNityAngular.Controllers;
using MUNityAngular.Models.Conference;
using MUNityAngular.Models.Resolution.V2;
using MUNityAngular.Services;
using Xunit;

namespace MUNityTest.ControllerTest.ResolutionControllerTest
{
    public class ResolutionControllerTests
    {
        [Fact]
        public async Task GetConferenceReturnsNull()
        {
            var mockService = new Mock<IResolutionService>();
            mockService.Setup(service =>
                service.CreateResolution("test")).ReturnsAsync(GetTestResolution);
            var controller = new ResolutionController(null);

            var result = await controller.Create("test", mockService.Object);

            Assert.NotNull(result);
            Assert.Equal("test", result.Header.Topic);
        }

        private ResolutionV2 GetTestResolution()
        {
            var resolution = new ResolutionV2();
            resolution.Header.Topic = "test";
            return resolution;
        }
    }
}
