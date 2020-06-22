using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MUNityAngular.DataHandlers.EntityFramework;
using MUNityAngular.Models.Conference;
using MUNityAngular.Models.Conference.Roles;
using MUNityAngular.Models.Organisation;
using NUnit.Framework;
using MUNityAngular.Services;

namespace MUNityTest.WorkflowTests
{
    [TestFixture]
    [Author("Peer Conradi")]
    [Description("Test the Conference Service implementation with a SqLite Database")]
    public class ConferenceWorkflowTest
    {
        private static MunCoreContext _context;
        private static Organisation _organisation;
        private static Project _project;
        private static Conference _conference;
        private static Committee _committeeGv;

        [OneTimeSetUp]
        public void Setup()
        {
            Console.WriteLine("Setup Test Environment");
            // Datenbank für den Test erzeugen und falls vorhanden erst einmal leeren und neu erstellen!
            var optionsBuilder = new DbContextOptionsBuilder<MunCoreContext>();
            optionsBuilder.UseSqlite("Data Source=test.db");
            _context = new MunCoreContext(optionsBuilder.Options);
            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();
        }

        [Test]
        [Order(1)]
        [Author("Peer Conradi")]
        [Description("First Test Creates an Organisation that can create projects etc.")]
        public async Task TestCreateAndGetOrganisation()
        {
            var service = new OrganisationService(_context);
            var result = service.CreateOrganisation("Deutsche Model United Nations e.V.", "dmun e.V.");
            var dbResult = await service.GetOrganisation(result.OrganisationId);
            Assert.AreEqual("Deutsche Model United Nations e.V.", dbResult.OrganisationName);
            _organisation = dbResult;
        }

        [Test]
        [Order(2)]
        [Author("Peer Conradi")]
        [Description("Test Create Project. A Project is kind of a group of Conferences.")]
        public async Task TestCreateAndGetProject()
        {
            if (_organisation == null)
                Assert.Fail("This test needs an organisation created before it can run!");

            var service = new ConferenceService(_context);
            var result = service.CreateProject("Test Konferenz", "TK", _organisation);
            var dbResult = await service.GetProject(result.ProjectId);
            Assert.AreEqual("Test Konferenz", dbResult.ProjectName);
            _project = dbResult;
        }

        [Test]
        [Order(3)]
        [Author("Peer Conradi")]
        [Description("Test that a conference can be created")]
        public async Task TestCreateAndGetConference()
        {
            if (_project == null)
            {
                Assert.Fail("This test needs a created project to run!");
            }
            var service = new ConferenceService(_context);
            var conference = service.CreateConference("Test Konferenz 2021", 
                "Konferenz zum Testen 2021" , "TK 2021", _project);
            var result = await service.GetConference(conference.ConferenceId);
            Assert.NotNull(result);
            _conference = conference;
        }

        [Test]
        [Order(4)]
        [Author("Peer Conradi")]
        [Description("Test that a committee can be added to a Conference.")]
        public async Task TestCreateAndGetCommittee()
        {
            if (_conference == null)
                Assert.Fail("This test needs a conference before it can run!");

            var service = new ConferenceService(_context);
            var committee = service.CreateCommittee(_conference, "Generalversammlung",
                "Generalversammlung", "GV");
            var result = await service.GetCommittee(committee.CommitteeId);
            Assert.NotNull(committee);
            var conference = await service.GetConference(_conference.ConferenceId);
            Assert.AreEqual(1, conference.Committees.Count);
            _committeeGv = committee;
        }

        [Test]
        [Order(5)]
        [Author("Peer Conradi")]
        [Description("Test Creating a Team structure.")]
        public void TestCreateTeam()
        {
            if (_conference == null)
                Assert.Fail("This test needs to have a conference first!");

            var service = new ConferenceService(_context);
            var projectLeader = service.CreateTeamRole(_conference, "Projektleitung");

            var tnbRole = service.CreateTeamRole(_conference, "Teilnehmendenbetreuung", projectLeader);
            var inhaltRole = service.CreateTeamRole(_conference, "Leitung Inhalt", projectLeader);
            var cbRole = service.CreateTeamRole(_conference, "Chairbetreuung", projectLeader);
            var publicRelationsRole =
                service.CreateTeamRole(_conference, "Leitung Öffentlichkeitsarbeit", projectLeader);
            var finances = service.CreateTeamRole(_conference, "Kassenwart", projectLeader);
            var logisticsRole = service.CreateTeamRole(_conference, "Leitung Logistik", projectLeader);
            var fundraisingRole = service.CreateTeamRole(_conference, "Leitung Fundraising", projectLeader);

            var secretaryGeneral = service.CreateSecretaryGeneralRole(_conference, 
                "Generalsekretäring", "Ihre Exzellenz");

            var teamRoles = service.GetTeamRoles(_conference.ConferenceId);
            Assert.AreEqual(8, teamRoles.Count());
        }

        [Test]
        [Order(6)]
        [Author("Peer Conradi")]
        [Description("Test to create a Delegation Structure")]
        public void TestCreateDelegations()
        {
            if (_conference == null)
                Assert.Fail("You first need to have a conference Created, run this Tests in order!");

            var service = new ConferenceService(_context);

            var delegationGermany = service.CreateDelegation(_conference, "Deutschland");
            var delegationSpain = service.CreateDelegation(_conference, "Spanien");

            Assert.NotNull(delegationGermany);
            Assert.NotNull(delegationSpain);

            var result = service.GetDelegations(_conference.ConferenceId);

            Assert.NotNull(result);
        }

        [Test]
        [Order(7)]
        [Author("Peer Conradi")]
        [Description("Test create a role for a delegate that is inside a Delegation")]
        public void TestCreateDelegateRole()
        {
            if (_conference == null && _committeeGv != null)
                Assert.Fail("You first need to have a conference Created, run this Tests in order!");

            var service = new ConferenceService(_context);

            var delegations = service.GetDelegations(_conference.ConferenceId);

            Assert.IsTrue(delegations.Any());

            var delegationGermany = delegations.FirstOrDefault(n =>
                n.Name == "Deutschland");
            Assert.NotNull(delegationGermany);
            var delegateOne =
                service.CreateDelegateRole(_committeeGv, 
                    delegationGermany, "Delegierter Deutschlands", true);

            var resultConference = service.GetDelegateRolesOfConference(_conference.ConferenceId);
            Assert.AreEqual(1, resultConference.Count());

            var resultCommittee = service.GetDelegateRolesOfCommittee(_committeeGv.CommitteeId);
            Assert.AreEqual(1, resultCommittee.Count());

            var delegationResult = service.GetDelegateRolesOfDelegation(delegationGermany.DelegationId);
            Assert.AreEqual(1, delegationResult.Count());
        }

        [Test]
        [Order(8)]
        [Author("Peer Conradi")]
        [Description("Test to create an NGO Role")]
        public void TestCreateNgoRoles()
        {
            if (_conference == null)
                Assert.Fail("You first need to have a conference Created, run this Tests in order!");

            var service = new ConferenceService(_context);
            var ngo = service.CreateNgoRole(_conference, "Vertreter Greenpeace", "Greenpeace");

            var ngos = service.GetNgoRoles(_conference.ConferenceId);
            Assert.AreEqual(1, ngos.Count());
        }

        [Test]
        [Order(9)]
        [Author("Peer Conradi")]
        [Description("Test to create press Roles.")]
        public void TestCreatePressRoles()
        {
            if (_conference == null )
                Assert.Fail("You first need to have a conference Created, run this Tests in order!");

            // Print
            var service = new ConferenceService(_context);
            var printChef = service.CreatePressRole(_conference, PressRole.EPressCategories.Print, "Chefredakteur/in");
            var printRedakteur = service.CreatePressRole(_conference, PressRole.EPressCategories.Print, "Redakteur/in");
            var printEditor = service.CreatePressRole(_conference, PressRole.EPressCategories.Print, "Editor/in");
            var printLayout = service.CreatePressRole(_conference, PressRole.EPressCategories.Print, "Layouter/in");

            // TV
            var tvRedakteur = service.CreatePressRole(_conference, PressRole.EPressCategories.TV, "Redakteur/in");
            var tvModerator = service.CreatePressRole(_conference, PressRole.EPressCategories.TV, "TV Moderator/in");
            var tvEditor = service.CreatePressRole(_conference, PressRole.EPressCategories.TV, "Cutter");

            // Online
            var onlineRedakteur =
                service.CreatePressRole(_conference, PressRole.EPressCategories.Print, "Online Redakteur");

            var getPress = service.GetPressRoles(_conference.ConferenceId);
            Assert.AreEqual(8, getPress.Count());
        }

        [Test]
        [Order(10)]
        [Author("Peer Conradi")]
        [Description("Test User Participate in the Role of project Leader")]
        public void TestParticipateInRole()
        {
            var service = new ConferenceService(_context);
            var roles = service.GetTeamRoles(_conference.ConferenceId);
            var role = roles.FirstOrDefault(n => n.RoleName == "Projektleitung");
            Assert.NotNull(role);
            
            var userService = new UserService(_context);
            var user = userService.CreateUser("test", "123456", "test@mail.com", new DateTime(1990, 1, 1));
            Assert.NotNull(user);

            var participation = service.Participate(user, role);
            Assert.NotNull(participation);
        }

        [Test]
        [Order(11)]
        [Author("Peer Conradi")]
        [Description("Get the participations of the user test it should be the one at the conferece")]
        public async Task TestGetUserParticipations()
        {
            var service = new ConferenceService(_context);
            var userService = new UserService(_context);

            var user =await userService.GetUserByUsername("test");
            Assert.NotNull(user);

            var participations = service.GetUserParticipations(user);
            Assert.AreEqual(1, participations.Count());

            var participation = participations.First();
            Assert.NotNull(participation.Role);
            Assert.IsTrue(participation.Role is TeamRole);
        }

        [Test]
        [Order(12)]
        [Author("Peer Conradi")]
        [Description("Get all the participations of a user inside a conference")]
        public void TestGetConferenceParticipations()
        {
            var service = new ConferenceService(_context);
            var participations = service.GetConferenceParticipations(_conference.ConferenceId);
            Assert.AreEqual(1, participations.Count());
        }


    }
}