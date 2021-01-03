using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using MUNityCore.Controllers;
using MUNityCore.Models.Conference;
using MUNityCore.Models.Conference.Roles;
using MUNityCore.Models.User;
using MUNityCore.Models.Organization;
using MUNityCore.Services;
using Xunit;
using MUNity.Schema.Project;
using MUNity.Schema.Conference;

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

        private MunityUser _testUser;

        public MunityUser GetTestUser()
        {
            return _testUser ??= new MunityUser
            {
                MunityUserId = 0,
                Username = "testuser",
                Forename = "Test",
                Lastname = "User"
            };
        }

        private Organization _testOrganization = null;
        private Organization GetTestOrganisation()
        {
            if (_testOrganization != null)
                return _testOrganization;

            var organization = new Organization()
            {
                OrganizationId = "testorga",
                OrganizationName = "Test Organization",
                OrganizationShort = "TO"
            };

            var user = new MunityUser
            {
                MunityUserId = 0,
                Username = "testuser",
                Forename = "Max",
                Lastname = "Mustermann",
                Birthday = new DateTime(1990, 1, 1)
            };

            var organisationRole = new OrganizationRole
            {
                Organization = organization, CanCreateProject = true, RoleName = "Admin", OrganizationRoleId = 0
            };

            organization.Roles.Add(organisationRole);

            var membership = new OrganizationMember {User = user, Role = organisationRole};

            organization.Member.Add(membership);
            _testOrganization = organization;
            return organization;
        }

        private Project _testProject;
        private Project GetTestProject()
        {
            return _testProject ??= new Project()
            {
                ProjectId = "testproject",
                ProjectName = "Testproject",
                ProjectOrganization = GetTestOrganisation(),
                ProjectShort = "Test"

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
            // Mocking the get Organization to return a valid organization
            var getOrgaMock = mockOrganisationService.Setup(service =>
                service.GetOrganisation("testorga")).ReturnsAsync(GetTestOrganisation);

            mockConferenceService.Setup(service =>
                service.CreateProject("Testproject", "test", GetTestOrganisation())).Returns(GetTestProject);

            var user = new TestPrincipal(new Claim(ClaimTypes.Name, "testuser"));

            controller.ControllerContext = new ControllerContext {HttpContext = new DefaultHttpContext() {User = user}};

            var request = new CreateProjectRequest
            {
                Name = "Testproject", Abbreviation = "test", OrganisationId = "testorga"
            };

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

            controller.ControllerContext = new ControllerContext {HttpContext = new DefaultHttpContext() {User = user}};

            var request = new CreateConferenceRequest
            {
                Name = "Test Conference",
                FullName = "Conference to Test",
                ConferenceShort = "TK",
                ProjectId = "testproject"
            };

            var call = await controller.CreateConference(request);

            var result = call.Result as OkObjectResult;
            //Assert.NotNull(result);
        }
    }
}
