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
            Assert.AreEqual(MUNityBase.EApplicationStates.Closed, delegateRole.ApplicationState);
            var delegateRolesWithClosedApplications = _context.Delegates
                .Where(n => n.ApplicationState == MUNityBase.EApplicationStates.Closed &&
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

        [Test]
        [Order(2)]
        public void TestCreateDelegationApplication()
        {
            var user = CreateATestUser("delegationApplicationUser");
            var role = EnsureGermanyDelegateRole();
            var delegation = _context.Delegation.FirstOrDefault(n => n.Name == "Deutschland");
            Assert.NotNull(delegation);
            var delegationApplication = new DelegationApplication()
            {
                Delegation = delegation,
                ApplyDate = DateTime.Now,
                User = user,
                Title = "Bewerbung auf Delegation Deutschland",
                Content = "Hallo, ich bin Max Mustermann und möchte mich auf die Delegation Deutschland bewerben."
            };
            _context.DelegationApplications.Add(delegationApplication);
            _context.SaveChanges();
            Assert.AreEqual(1, _context.DelegationApplications.Count());

            var delegationApplicationsOfConference =
                _context.DelegationApplications.Where(n => n.Delegation.Conference.ConferenceId == TestConference.ConferenceId);
            Assert.AreEqual(1, delegationApplicationsOfConference.Count());
        }

        [Test]
        [Order(3)]
        public void TestGroupedRoleApplication()
        {
            var userOne = CreateATestUser("UserOne");
            var userTwo = CreateATestUser("UserTwo");
            var roleOne = EnsureGermanyDelegateRole();
            var committeeTwo = CreateCommittee("Hauptausschuss 3", "Hauptausschuss 3", "HA 3");
            var roleTwo = CreateDelegateRole(roleOne.Delegation, committeeTwo, "Deutschland im HA3", "Deutschland", "de");
            var applicationOne = new RoleApplication()
            {
                ApplyDate = DateTime.Now,
                Content = "BLOB",
                Role = roleOne,
                Title = "Title",
                User = userOne
            };

            _context.RoleApplications.Add(applicationOne);
            

            var applicationTwo = new RoleApplication()
            {
                ApplyDate = DateTime.Now,
                Content = "BLOB",
                Role = roleTwo,
                Title = "Title",
                User = userTwo
            };

            _context.RoleApplications.Add(applicationTwo);

            var list = new List<RoleApplication>();
            list.Add(applicationOne);
            list.Add(applicationTwo);

            var group = new GroupedRoleApplication()
            {
                Applications = list,
                CreateTime = DateTime.Now,
                GroupName = "Hello World"
            };
            _context.GroupedRoleApplications.Add(group);

            _context.SaveChanges();
            Assert.AreEqual(1, _context.GroupedRoleApplications.Count());
            Assert.AreEqual(3, _context.RoleApplications.Count());
        }

        public ApplicationTests() : base("applicationtest.db")
        {

        }
    }
}
