using System;
using NUnit.Framework;
using MUNityCore.Models.Conference;
using MUNityCore.Models.Conference.Roles;
using MUNityCore.Models.Organization;

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
            project.ProjectShort = "MUN-SH";
            project.ProjectOrganization = null;
            Assert.AreEqual("mun-sh", project.ProjectId);
            Assert.AreEqual("Model United Nations Schleswig-Holstein", project.ProjectName);
            Assert.AreEqual("MUN-SH", project.ProjectShort);
        }

        [Test]
        public void TestCanHaveConference()
        {
            var conference = new Conference();
            conference.ConferenceId = "mun-sh2021";
            conference.Name = "Model United Nations Schleswig-Holstein 2021";
            conference.FullName = "Model United Nations Schleswig-Holstein 2021";
            conference.ConferenceShort = "MUN-SH 2021";
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
            committee.CommitteeShort = "GV";
            committee.Article = "die";
            Assert.NotNull(committee);
        }

        [Test]
        public void TestCanHaveVisitor()
        {
            var visitor = new ConferenceVisitorRole();
            Assert.NotNull(visitor);
        }

        [Test]
        public void TestCanHaveDelegate()
        {
            var delegateRole = new ConferenceDelegateRole();
            Assert.NotNull(delegateRole);
        }

        [Test]
        public void TestCanHaveTeamRole()
        {
            var teamRole = new ConferenceTeamRole();
            Assert.NotNull(teamRole);
        }

        [Test]
        public void TestCanHavePressRole()
        {
            var pressRole = new ConferencePressRole();
            Assert.NotNull(pressRole);
        }

        [Test]
        public void TestCanHaveSecretaryGeneral()
        {
            var secretaryGeneral = new ConferenceSecretaryGeneralRole();
            secretaryGeneral.Title = "Seine Exzellenz der Generalseretär";
            Assert.NotNull(secretaryGeneral);
        }

        [Test]
        public void TestCompleteConferenceStructure()
        {
            // Above all Conferences is the organization
            var organisation = new Organization();
            organisation.OrganizationId = "dmun";
            organisation.OrganizationName = "Deutsche Model United Nations e.V.";
            organisation.OrganizationShort = "dmun e.V.";
            
            // Then there is a project
            var project = new Project();
            project.ProjectId = "mun-sh";
            project.ProjectName = "Model United Nations Schleswig-Holstein";
            project.ProjectShort = "MUN-SH";
            project.ProjectOrganization = organisation;

            // Next comes the conference that is part of the project
            var conference = new Conference();
            conference.ConferenceId = "mun-sh2021";
            conference.Name = "Model United Nations Schleswig-Holstein 2021";
            conference.FullName = "Model United Nations Schleswig-Holstein 2021";
            conference.ConferenceShort = "MUN-SH 2021";
            conference.StartDate = new DateTime(2021, 04, 01);
            conference.EndDate = new DateTime(2021, 04, 04);
            conference.CreationDate = DateTime.Now;

            // Team Rollen
            // Projektleitung (PL)
            var projectLeader = new ConferenceTeamRole();
            projectLeader.RoleName = "Projektleitung";
            projectLeader.Conference = conference;

            // EPL
            var cb = new ConferenceTeamRole();
            cb.RoleName = "Chairbetreuung";
            cb.ParentTeamRole = projectLeader;
            cb.Conference = conference;

            // Committee
            var generalAssembly = new Committee();
            generalAssembly.CommitteeId = "mun-sh2021-gv";
            generalAssembly.Name = "Generalversammlung";
            generalAssembly.FullName = "Generalversammlung";
            generalAssembly.CommitteeShort = "GV";
            generalAssembly.Article = "die";

            conference.Committees.Add(generalAssembly);

            // Delegation
            var delegationGermany = new Delegation();
            delegationGermany.Name = "Deutschland";

            // Delegierter (normal/männlich)
            var delegationRoleGermany = new ConferenceDelegateRole();
            delegationRoleGermany.Committee = generalAssembly;
            delegationRoleGermany.Delegation = delegationGermany;
            delegationRoleGermany.Title = "Delegierter";

            // Delegierter (präsident/männlich)
            var delegationRolePresident = new ConferenceDelegateRole();
            delegationRolePresident.Committee = generalAssembly;
            delegationRolePresident.Delegation = delegationGermany;
            delegationRolePresident.Title = "Präsident";

            // Presse Teili
            var pressRole = new ConferencePressRole();
            pressRole.PressCategory = ConferencePressRole.EPressCategories.TV;
        }
    }
}