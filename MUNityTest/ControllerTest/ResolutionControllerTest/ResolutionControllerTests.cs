using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Moq;
using MUNityCore.Controllers;
using MUNityCore.Hubs;
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

        [Fact]
        public async Task UpdateNotExistingResolutionTexst()
        {
            var mockResolutionService = new Mock<IResolutionService>();
            mockResolutionService.Setup(n => n.GetResolution(It.IsAny<string>())).Returns<ResolutionV2>(n => null);


            var mockAuthService = new Mock<IAuthService>();

            var mockHub = new Mock<IHubContext<ResolutionHub, ITypedResolutionHub>>();

            var controller = new ResolutionController(mockHub.Object, mockResolutionService.Object, mockAuthService.Object);

            var newResolution = new ResolutionV2();
            var actionResult = await controller.UpdateResolution(newResolution);
            var result = actionResult.Result as ForbidResult;
            Assert.NotNull(result);
        }

        private ResolutionV2 GetTestResolution()
        {
            var resolution = new ResolutionV2();
            resolution.Header.Topic = "test";
            return resolution;
        }
    }
}
