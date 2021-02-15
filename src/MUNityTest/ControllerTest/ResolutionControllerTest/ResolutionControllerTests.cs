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
            Assert.Equal("test", result.Value.Header.Topic);
        }

        //[Fact]
        //public async Task UpdateNotExistingResolutionTexst()
        //{
        //    var mockResolutionService = new Mock<IResolutionService>();
        //    mockResolutionService.Setup(n => n.GetResolution(It.IsAny<string>())).Returns<MUNity.Models.Resolution.Resolution>(n => null);


        //    var mockAuthService = new Mock<IAuthService>();

        //    var mockHub = new Mock<IHubContext<ResolutionHub, MUNity.Hubs.ITypedResolutionHub>>();

        //    var controller = new ResolutionController(mockHub.Object, mockResolutionService.Object, mockAuthService.Object);

        //    var newResolution = new MUNity.Models.Resolution.Resolution();
        //    var actionResult = await controller.UpdateResolution(newResolution);
        //    var result = actionResult.Result as ForbidResult;
        //    Assert.NotNull(result);
        //}

        private MUNity.Models.Resolution.Resolution GetTestResolution()
        {
            var resa = new MUNity.Models.Resolution.Resolution();
            resa.Header.Topic = "test";
            return resa;
        }
    }
}
