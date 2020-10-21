using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Moq;
using MUNityCore.Controllers;
using MUNityCore.Models.Conference;
using MUNityCore.Models.Resolution.V2;
using MUNityCore.Services;
using Xunit;

namespace MUNityTest.ControllerTest.ResolutionControllerTest
{
    public class ResolutionControllerTests
    {
        [Fact]
        public async Task TestCreatePublicResolution()
        {
            var mockService = new Mock<IResolutionService>();
            mockService.Setup(service =>
                service.CreatePublicResolution("test")).ReturnsAsync(GetTestResolution);

            var mockAuthService = new Mock<IAuthService>();

            var controller = new ResolutionController(null, mockService.Object, mockAuthService.Object);

            var result = await controller.CreatePublic("test");

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
