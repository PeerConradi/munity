﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MUNityAngular.Models.Conference
{

    /// <summary>
    /// When users want to apply as a group it is possible to create a collective Application
    /// for the same AbstractRole. If you want to create custom Applications for multiple Roles
    /// but every and ever user of the group should have a custom Application use GroupedRoleApplication.
    /// </summary>
    public class GroupApplication
    {
        public int GroupApplicationId { get; set; }

        public List<Core.User> Users { get; set; }

        public AbstractRole Role { get; set; }

        public Delegation Delegation { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public DateTime ApplicationDate { get; set; }

        public GroupApplication()
        {
            Users = new List<Core.User>();
        }
    }
}