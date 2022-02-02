using Microsoft.Extensions.DependencyInjection;
using MUNity.Database.Context;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MUNity.Services.Test.ServiceTests
{
    internal class VirtualCommitteeServiceTest
    {
        private IServiceProvider provider;


        [OneTimeSetUp]
        public void Setup()
        {
            provider = TestHelpers.GetCompleteProvider();
        }

        [Test]
        [Order(0)]
        public void TestCreateANewGroup()
        {
            var context = provider.GetRequiredService<MunityContext>();
            Assert.AreEqual(0, context.VirtualCommitteeGroups.Count());

            var service = provider.GetRequiredService<VirtualCommitteeService>();
            var viewModel = service.CreateGroup("MUN-SH 2022");
            Assert.NotNull(viewModel);
            Assert.AreEqual(1, context.VirtualCommitteeGroups.Count());
        }

        [Test]
        [Order(1)]
        public void TestCreateAVertualCommittee()
        {
            var context = provider.GetRequiredService<MunityContext>();
            Assert.AreEqual(0, context.VirtualCommittees.Count());

            var service = provider.GetRequiredService<VirtualCommitteeService>();
            var groupViewModel = service.GetGroupByName("MUN-SH 2022");
            Assert.NotNull(groupViewModel, "There should have been a group/conference called MUN-SH 2022");

            var virtualCommittee = service.CreateVirtualCommittee(groupViewModel, "Generalversammlung", "Password");
            Assert.NotNull(virtualCommittee);
            Assert.AreEqual(1, context.VirtualCommittees.Count());
        }

        [Test]
        [Order(3)]
        public void TestAddSlot()
        {
            var context = provider.GetRequiredService<MunityContext>();

            var service = provider.GetRequiredService<VirtualCommitteeService>();
            var group = service.GetGroupByName("MUN-SH 2022");
            var gvViewModel = group.VirtualCommittees.FirstOrDefault();
            Assert.NotNull(gvViewModel);

            var newSlot = service.AddSlot(gvViewModel, "de", "Deutschland");
            Assert.NotNull(newSlot);
            Assert.AreEqual(1, context.VirtualCommitteeSlots.Count());
        }

        
    }
}
