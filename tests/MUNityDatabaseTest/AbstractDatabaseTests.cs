using Microsoft.EntityFrameworkCore;
using MUNity.Database.Context;
using MUNity.Database.Models.Conference;
using MUNity.Database.Models.Organization;
using MUNity.Database.Models.User;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MUNityDatabaseTest
{
    public abstract class AbstractDatabaseTests
    {
        public MunityContext _context;

        private Organization testOrganization;
        public Organization TestOrganization { 
            get
            {
                if (testOrganization == null)
                    testOrganization = CreateTestOrganization();
                return testOrganization;
            }
        }

        private Project testProject;
        public Project TestProject { 
            get
            {
                if (testProject == null)
                    testProject = CreateTestProject();
                return testProject;
            }
        }

        private Conference testConference;
        public Conference TestConference { 
            get
            {
                if (testConference == null)
                    testConference = CreateTestConference();
                return testConference;
            }
        }

        private Committee testCommittee;
        public Committee TestCommittee
        {
            get
            {
                if (testCommittee == null)
                    testCommittee = CreateTestCommittee();
                return testCommittee;
            }
        }

        public Committee TestCommitteeGV { get;  }

        string dbName = "munity_test.db";

        [OneTimeSetUp]
        public void Setup()
        {
            // Datenbank für den Test erzeugen und falls vorhanden erst einmal leeren und neu erstellen!
            var optionsBuilder = new DbContextOptionsBuilder<MunityContext>();
            optionsBuilder.UseSqlite("Data Source=test_conference.db");
            _context = new MunityContext(optionsBuilder.Options);
            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();
        }

        private Organization CreateTestOrganization()
        {
            var organization = new Organization()
            {
                OrganizationName = "Deutsche Model United Nations e.V.",
                OrganizationShort = "DMUN e.V."
            };
            _context.Organizations.Add(organization);
            _context.SaveChanges();
            return organization;
        }

        private Project CreateTestProject()
        {
            var project = new Project()
            {
                ProjectName = "MUN Schleswig-Holstein 2022",
                ProjectOrganization = TestOrganization,
                ProjectShort = "MUN-SH 2022",
            };
            _context.Projects.Add(project);
            _context.SaveChanges();
            return project;
        }

        private Conference CreateTestConference()
        {
            var conference = new Conference()
            {
                ConferenceProject = TestProject,
                ConferenceShort = "MUN-SH 2022",
                Name = "MUN Schleswig-Holstein 2022",
                FullName = "Model United Nations Schleswig-Holstein 2022"
            };
            _context.Conferences.Add(conference);
            _context.SaveChanges();
            return conference;
        }

        private Committee CreateTestCommittee()
        {
            var committee = new Committee()
            {
                Conference = TestConference,
                Article = "die",
                FullName = "Generalversammlung",
                Name = "Generalversammlung"
            };
            _context.Committees.Add(committee);
            _context.SaveChanges();
            return committee;
        }

        public MunityUser CreateATestUser(string username)
        {
            var user = new MunityUser()
            {
                AccessFailedCount = 0,
                Email = $"{username}@mail.com",
                EmailConfirmed = true,
                Forename = "",
                Lastname = "",
                NormalizedUserName = username.ToUpper(),
                NormalizedEmail = $"{username.ToUpper()}@MAIL.COM",
            };
            _context.Users.Add(user);
            return user;
        }


        public AbstractDatabaseTests(string dataSourceName)
        {
            this.dbName = dataSourceName;
        }
    }
}
