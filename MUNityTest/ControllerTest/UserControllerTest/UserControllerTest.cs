using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using MUNityCore.Controllers;
using MUNityCore.Models.Conference;
using MUNityCore.Models.User;
using MUNityCore.Services;
using MUNity.Schema.Authentication;
using Xunit;

namespace MUNityTest.ControllerTest.UserControllerTest
{
    public class UserControllerTest
    {
        [Fact]
        public void RegisterTest()
        {
            var userServiceMock = new Mock<IUserService>();

            var authServiceMock = new Mock<IAuthService>();

            userServiceMock.Setup(service =>
                service.CreateUser(It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<DateTime>())).Returns(new MunityUser());

            var request = new RegisterRequest
            {
                Forename = "Max",
                Lastname = "Mustermann",
                Username = "maxmann",
                Mail = "max@mail.de",
                Birthday = new DateTime(1990,1,1),
                Password = "Password"
            };

            var controller = new UserController(authServiceMock.Object, userServiceMock.Object);
            var call = controller.Register(request);
            Assert.NotNull(call);
        }

        [Fact]
        public void RegisterWithInvalidDataTest()
        {
            var userServiceMock = new Mock<IUserService>();

            var authServiceMock = new Mock<IAuthService>();

            userServiceMock.Setup(service =>
                service.CreateUser(It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<DateTime>())).Throws(new ArgumentException("Invalid input"));

            var request = new RegisterRequest
            {
                Forename = "Max",
                Lastname = "Mustermann",
                Username = "maxmann",
                Mail = "notamail",
                Birthday = new DateTime(1990, 1, 1),
                Password = "Password"
            };

            var controller = new UserController(authServiceMock.Object, userServiceMock.Object);
            var call = controller.Register(request);
            var result = call as BadRequestObjectResult;
            Assert.NotNull(result);
            Assert.Equal(StatusCodes.Status400BadRequest, result.StatusCode);
        }



    }
}
