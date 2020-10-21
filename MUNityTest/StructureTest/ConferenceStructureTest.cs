using System;
using NUnit.Framework;
using MUNityCore.Models.Conference;
using MUNityCore.Models.Conference.Roles;
using MUNityCore.Models.Organisation;

namespace MUNityTest.StructureTest
{


    [TestFixture]
    public class ConferenceStructureTest
    {
        

        [Test]
        public void TestCanHaveProject()
        {
            var project = new Project();
            project.ProjectId = "mun-sh";
            project.ProjectName = "Model United Nations Schleswig-Holstein";
            project.ProjectAbbreviation = "MUN-SH";
            project.ProjectOrganisation = null;
            Assert.AreEqual("mun-sh", project.ProjectId);
            Assert.AreEqual("Model United Nations Schleswig-Holstein", project.ProjectName);
            Assert.AreEqual("MUN-SH", project.ProjectAbbreviation);
        }

        [Test]
        public void TestCanHaveConference()
        {
            var conference = new Conference();
            conference.ConferenceId = "mun-sh2021";
            conference.Name = "Model United Nations Schleswig-Holstein 2021";
            conference.FullName = "Model United Nations Schleswig-Holstein 2021";
            conference.Abbreviation = "MUN-SH 2021";
            conference.StartDate = new DateTime(2021, 04,01);
            conference.EndDate = new DateTime(2021, 04, 04);
            conference.CreationDate = DateTime.Now;
            Assert.NotNull(conference);
        }

        [Test]
        public void TestCanHaveCommittee()
        {
            var committee = new Committee();
            committee.CommitteeId = "mun-sh2021-gv";
            committee.Name = "Generalversammlung";
            committee.FullName = "Generalversammlung";
            committee.Abbreviation = "GV";
            committee.Article = "die";
            Assert.NotNull(committee);
        }

        [Test]
        public void TestCanHaveVisitor()
        {
            var visitor = new VisitorRole();
            Assert.NotNull(visitor);
        }

        [Test]
        public void TestCanHaveDelegate()
        {
            var delegateRole = new DelegateRole();
            Assert.NotNull(delegateRole);
        }

        [Test]
        public void TestCanHaveTeamRole()
        {
            var teamRole = new TeamRole();
            Assert.NotNull(teamRole);
        }

        [Test]
        public void TestCanHavePressRole()
        {
            var pressRole = new PressRole();
            Assert.NotNull(pressRole);
        }

        [Test]
        public void TestCanHaveSecretaryGeneral()
        {
            var secretaryGeneral = new SecretaryGeneralRole();
            secretaryGeneral.Title = "Seine Exzellenz der Generalseretär";
            Assert.NotNull(secretaryGeneral);
        }

        [Test]
        public void TestCompleteConferenceStructure()
        {
            // Above all Conferences is the organisation
            var organisation = new Organisation();
            organisation.OrganisationId = "dmun";
            organisation.OrganisationName = "Deutsche Model United Nations e.V.";
            organisation.OrganisationAbbreviation = "dmun e.V.";
            
            // Then there is a project
            var project = new Project();
            project.ProjectId = "mun-sh";
            project.ProjectName = "Model United Nations Schleswig-Holstein";
            project.ProjectAbbreviation = "MUN-SH";
            project.ProjectOrganisation = organisation;

            // Next comes the conference that is part of the project
            var conference = new Conference();
            conference.ConferenceId = "mun-sh2021";
            conference.Name = "Model United Nations Schleswig-Holstein 2021";
            conference.FullName = "Model United Nations Schleswig-Holstein 2021";
            conference.Abbreviation = "MUN-SH 2021";
            conference.StartDate = new DateTime(2021, 04, 01);
            conference.EndDate = new DateTime(2021, 04, 04);
            conference.CreationDate = DateTime.Now;

            // Team Rollen
            // Projektleitung (PL)
            var projectLeader = new TeamRole();
            projectLeader.RoleName = "Projektleitung";
            projectLeader.Conference = conference;

            // EPL
            var cb = new TeamRole();
            cb.RoleName = "Chairbetreuung";
            cb.ParentTeamRole = projectLeader;
            cb.Conference = conference;

            // Committee
            var generalAssembly = new Committee();
            generalAssembly.CommitteeId = "mun-sh2021-gv";
            generalAssembly.Name = "Generalversammlung";
            generalAssembly.FullName = "Generalversammlung";
            generalAssembly.Abbreviation = "GV";
            generalAssembly.Article = "die";

            conference.Committees.Add(generalAssembly);

            // Delegation
            var delegationGermany = new Delegation();
            delegationGermany.Name = "Deutschland";

            // Delegierter (normal/männlich)
            var delegationRoleGermany = new DelegateRole();
            delegationRoleGermany.Committee = generalAssembly;
            delegationRoleGermany.Delegation = delegationGermany;
            delegationRoleGermany.Title = "Delegierter";

            // Delegierter (präsident/männlich)
            var delegationRolePresident = new DelegateRole();
            delegationRolePresident.Committee = generalAssembly;
            delegationRolePresident.Delegation = delegationGermany;
            delegationRolePresident.Title = "Präsident";

            // Presse Teili
            var pressRole = new PressRole();
            pressRole.PressCategory = PressRole.EPressCategories.TV;
        }
    }
}