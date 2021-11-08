using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MUNity.Database.Context;
using MUNity.Database.FluentAPI;
using MUNity.Database.Models.Organization;
using NUnit.Framework;

namespace MUNity.Database.Test.FluentTest
{
    public class OrganizationFluentTest
    {
        private MunityContext _context;

        [SetUp]
        public void SetupTest()
        {
            _context = MunityContext.FromSqlLite("testfluent");
            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();
        }

        [Test]
        public void TestCanCreateOrganization()
        {
            
            Assert.Zero(_context.Organizations.Count());
            var orga = _context.Fluent.Organization.AddOrganization(orga =>
                orga.WithName("Deutsche Model United Nations e.V.")
                    .WithShort("DMUN e.V."));
            Assert.NotZero(_context.Organizations.Count());
            Assert.NotNull(orga);

            var refetchOrga = _context.Organizations.First();
            Assert.AreEqual("Deutsche Model United Nations e.V.", refetchOrga.OrganizationName);
            Assert.AreEqual("DMUN e.V.", refetchOrga.OrganizationShort);
            Assert.AreEqual("dmunev", refetchOrga.OrganizationId);
        }

        [Test]
        public void TestCanCreateOrganizationWithProject()
        {
            var orga = _context.Fluent.Organization.AddOrganization(orga =>
                orga.WithShort("DMUN e.V.")
                    .WithName("Deutsche Model United Nations e.V.")
                    .WithProject(projectBuilder => 
                        projectBuilder
                            .WithName("Model United Nations Baden-Würrtemberg")
                            .WithShort("MUNBW")));
            Assert.NotZero(_context.Organizations.Count(), "The context should contain an organization but it doesnt");
            Assert.NotZero(_context.Projects.Count(), "The context should contain a project, but it didnt");

            var refetchProject = _context.Projects.First();
            Assert.AreEqual("Model United Nations Baden-Würrtemberg", refetchProject.ProjectName);
            Assert.AreEqual("MUNBW", refetchProject.ProjectShort);
        }

        [Test]
        public void TestCanCreateWithConference()
        {
            var orga = _context.Fluent.Organization.AddOrganization(orga =>
                orga.WithShort("DMUN e.V.")
                    .WithName("Deutsche Model United Nations e.V.")
                    .WithProject(projectBuilder =>
                        projectBuilder
                            .WithName("Model United Nations Baden-Würrtemberg")
                            .WithShort("MUNBW")
                            .WithConference(conference => 
                                conference.WithName("Model United Nations Baden-Würrtemberg 2022")
                                    .WithFullName("Model United Nations Baden-Würrtemberg 2022")
                                    .WithShort("MUNBW 2022"))));
            Assert.NotZero(_context.Organizations.Count());
            Assert.NotZero(_context.Projects.Count());
            Assert.NotZero(_context.Conferences.Count());
            var refetchConference = _context.Conferences.First();
            Assert.AreEqual("Model United Nations Baden-Würrtemberg 2022", refetchConference.Name);
            Assert.AreEqual("Model United Nations Baden-Würrtemberg 2022", refetchConference.FullName);
            Assert.AreEqual("MUNBW 2022", refetchConference.ConferenceShort);
        }

        [Test]
        public void TestCanCreateWithCommittee()
        {
            var orga = _context.Fluent.Organization.AddOrganization(orga =>
                orga.WithShort("DMUN e.V.")
                    .WithName("Deutsche Model United Nations e.V.")
                    .WithProject(projectBuilder =>
                        projectBuilder
                            .WithName("Model United Nations Baden-Würrtemberg")
                            .WithShort("MUNBW")
                            .WithConference(conference =>
                                conference.WithName("Model United Nations Baden-Würrtemberg 2022")
                                    .WithFullName("Model United Nations Baden-Würrtemberg 2022")
                                    .WithShort("MUNBW 2022")
                                    .WithCommittee(committee =>
                                        committee.WithName("Generalversammlung")
                                            .WithFullName("Generalversammlung")))));
            Assert.NotZero(_context.Organizations.Count());
            Assert.NotZero(_context.Projects.Count());
            Assert.NotZero(_context.Conferences.Count());
            Assert.NotZero(_context.Committees.Count());
            var refetchConference = _context.Conferences.First();
            Assert.AreEqual("Model United Nations Baden-Würrtemberg 2022", refetchConference.Name);
            Assert.AreEqual("Model United Nations Baden-Würrtemberg 2022", refetchConference.FullName);
            Assert.AreEqual("MUNBW 2022", refetchConference.ConferenceShort);
        }

    }
}
