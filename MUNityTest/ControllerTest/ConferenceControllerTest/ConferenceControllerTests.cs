using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Moq;
using MUNityAngular.Controllers;
using MUNityAngular.Models.Conference;
using MUNityAngular.Services;
using Xunit;

namespace MUNityTest.ControllerTest.ConferenceControllerTest
{
    public class ConferenceControllerTests
    {
        [Fact]
        public async Task GetConferenceReturnsNull()
        {
            var mockService = new Mock<IConferenceService>();
            mockService.Setup(service =>
                service.GetConference("yolo")).ReturnsAsync(GetTestConference);
            var controller = new ConferenceController();

            var result = await controller.GetConference(mockService.Object, "yolo");

            Assert.NotNull(result);
            Assert.Equal(GetTestConference().Name, result.Name);
        }

        private Conference GetTestConference()
        {
            var conference = new Conference
            {
                ConferenceId = "yolo",
                Name = "Test Konferenz"
            };
            return conference;
        }
    }
}
