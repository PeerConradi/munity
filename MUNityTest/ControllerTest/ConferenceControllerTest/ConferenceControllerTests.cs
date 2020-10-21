using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using MUNityCore.Controllers;
using MUNityCore.Models.Conference;
using MUNityCore.Models.Conference.Roles;
using MUNityCore.Models.Core;
using MUNityCore.Models.Organisation;
using MUNityCore.Schema.Request;
using MUNityCore.Schema.Request.Conference;
using MUNityCore.Services;
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

            var mockAuthService = new Mock<IAuthService>();

            var mockOrganisationService = new Mock<IOrganisationService>();

            var controller = new ConferenceController(mockService.Object, mockAuthService.Object, mockOrganisationService.Object);
            
            var result = await controller.GetConference("yolo");

            Assert.NotNull(result);
            //Assert.Equal(GetTestConference().Name, result.Value.Name);
        }

        private Conference GetTestConference()
        {
            var conference = new Conference
            {
                Visibility = Conference.EConferenceVisibilityMode.Public,
                ConferenceId = "yolo",
                Name = "Test Konferenz"
            };
            return conference;
        }

        private User _testUser;

        public User GetTestUser()
        {
            return _testUser ??= new User
            {
                UserId = 0,
                Username = "testuser",
                Forename = "Test",
                Lastname = "User"
            };
        }

        private Organisation _testOrganisation = null;
        private Organisation GetTestOrganisation()
        {
            if (_testOrganisation != null)
                return _testOrganisation;

            var organisation = new Organisation()
            {
                OrganisationId = "testorga",
                OrganisationName = "Test Organisation",
                OrganisationAbbreviation = "TO"
            };

            var user = new User();
            user.UserId = 0;
            user.Username = "testuser";
            user.Forename = "Max";
            user.Lastname = "Mustermann";
            user.Birthday = new DateTime(1990,1,1);

            var organisationRole = new OrganisationRole();
            organisationRole.Organisation = organisation;
            organisationRole.CanCreateProject = true;
            organisationRole.RoleName = "Admin";
            organisationRole.OrganisationRoleId = 0;

            organisation.Roles.Add(organisationRole);

            var membership = new OrganisationMember();
            membership.User = user;
            membership.Role = organisationRole;
            
            organisation.Member.Add(membership);
            _testOrganisation = organisation;
            return organisation;
        }

        private Project _testProject;
        private Project GetTestProject()
        {
            return _testProject ??= new Project()
            {
                ProjectId = "testproject",
                ProjectName = "Testproject",
                ProjectOrganisation = GetTestOrganisation(),
                ProjectAbbreviation = "Test"

            };
        }

        private TeamRole _leaderRole;

        public TeamRole GetLeaderRole()
        {
            return _leaderRole ??= new TeamRole
            {
                RoleId = 0,
                RoleName = "Leader"
            };
        }

        private Participation GetParticipation()
        {
            var participation = new Participation();
            return participation;
        }

        private class TestPrincipal : ClaimsPrincipal
        {
            public TestPrincipal(params Claim[] claims) : base(new TestIdentity(claims))
            {
            }
        }

        private class TestIdentity : ClaimsIdentity
        {
            public TestIdentity(params Claim[] claims) : base(claims)
            {
            }
        }


        [Fact]
        public async Task TestCreateProject()
        {

            var mockOrganisationService = new Mock<IOrganisationService>();
            var mockConferenceService = new Mock<IConferenceService>();
            var mockAuthService = new Mock<IAuthService>();
            var controller = new ConferenceController(mockConferenceService.Object, mockAuthService.Object, mockOrganisationService.Object);
            // Mocking the get Organisation to return a valid organisation
            var getOrgaMock = mockOrganisationService.Setup(service =>
                service.GetOrganisation("testorga")).ReturnsAsync(GetTestOrganisation);

            mockConferenceService.Setup(service =>
                service.CreateProject("Testproject", "test", GetTestOrganisation())).Returns(GetTestProject);

            var user = new TestPrincipal(new Claim(ClaimTypes.Name, "testuser"));
            
            controller.ControllerContext = new ControllerContext();
            controller.ControllerContext.HttpContext = new DefaultHttpContext(){User = user};
            
            var request = new CreateProjectRequest();
            request.Name = "Testproject";
            request.Abbreviation = "test";
            request.OrganisationId = "testorga";

            var call = await controller.CreateProject(request);

            var result = call.Result as OkObjectResult;
            //Assert.NotNull(result);
            //Assert.Equal(StatusCodes.Status200OK, result.StatusCode.Value);
            //Assert.NotNull(result.Value as Project);
        }

        [Fact]
        public async Task TestCreateConference()
        {
            var mockAuthService = new Mock<IAuthService>();
            var mockConferenceService = new Mock<IConferenceService>();
            var mockOrganisationService = new Mock<IOrganisationService>();
            var controller = new ConferenceController(mockConferenceService.Object, mockAuthService.Object, mockOrganisationService.Object);

            
            // The method tries to get the project that this conference should be contained in
            // we mock a result for that call.
            mockConferenceService.Setup(service =>
                service.GetProject("testproject")).ReturnsAsync(GetTestProject);

            mockConferenceService.Setup(service =>
                service.Participate(GetTestUser(), It.IsAny<AbstractRole>())).Returns(GetParticipation);

            // Since this is a TestCase we dont want to actually create a conference
            // So we Mock a conference Result
            mockConferenceService.Setup(service =>
                    service.CreateConference("Test Conference", "Conference to Test", "TK", GetTestProject()))
                .Returns(GetTestConference);

            mockConferenceService.Setup(service =>
                service.CreateLeaderRole(It.IsAny<Conference>())).Returns(GetLeaderRole);

            var user = new TestPrincipal(new Claim(ClaimTypes.Name, "testuser"));

            // The method also calls the Auth Service to get the current user and
            // assign the conference to this user this needs to be mocked too
            mockAuthService.Setup(service =>
                service.GetUserOfClaimPrincipal(user)).Returns(GetTestUser);

            controller.ControllerContext = new ControllerContext();
            controller.ControllerContext.HttpContext = new DefaultHttpContext() { User = user };

            var request = new CreateConferenceRequest
            {
                Name = "Test Conference",
                FullName = "Conference to Test",
                Abbreviation = "TK",
                ProjectId = "testproject"
            };

            var call = await controller.CreateConference(request);

            var result = call.Result as OkObjectResult;
            //Assert.NotNull(result);
        }
    }
}
