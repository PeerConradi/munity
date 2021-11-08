using MUNity.Base;
using MUNity.Database.Models.Conference;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MUNityDatabaseTest.ConferenceTests
{
    public class ApplicationTests : AbstractDatabaseTests
    {
        [Test]
        [Order(0)]
        public void TestDefaultApplicationStateIsClosed()
        {
            //ResetDatabase();
            var country = EnsureGermany();
            var delegateRole = EnsureGermanyDelegateRole();
            Assert.AreEqual(EApplicationStates.Closed, delegateRole.ApplicationState);
            var delegateRolesWithClosedApplications = _context.Delegates
                .Where(n => n.ApplicationState == EApplicationStates.Closed &&
                n.Conference.ConferenceId == TestConference.ConferenceId);
            Assert.AreEqual(1, delegateRolesWithClosedApplications.Count());
        }

        [Test]
        [Order(1)]
        public void TestCreateDirectApplication()
        {
            //ResetDatabase();
            var user = CreateATestUser("directApp");
            var application = new RoleApplication()
            {
                ApplyDate = DateTime.Now,
                Content = "Ich möchte mich gerne für diese Rolle bewerben",
                Role = EnsureGermanyDelegateRole(),
                Title = "Bewerbung",
                User = user
            };
            _context.RoleApplications.Add(application);
            _context.SaveChanges();

            var getApplicationsToRole = _context.RoleApplications.Where(n => n.Role.RoleId == EnsureGermanyDelegateRole().RoleId);
            Assert.AreEqual(1, getApplicationsToRole.Count());
        }


        public ApplicationTests() : base("applicationtest.db")
        {

        }
    }
}
