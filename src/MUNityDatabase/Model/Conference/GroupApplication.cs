using MUNity.Database.Models.Conference.Roles;
using MUNity.Database.Models.User;
using MUNityCore.Models.User;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MUNity.Database.Models.Conference
{

    /// <summary>
    /// When users want to apply as a group it is possible to create a collective Application
    /// for the same AbstractConferenceRole. If you want to create custom Applications for multiple Roles
    /// but every and ever user of the group should have a custom Application use GroupedRoleApplication.
    /// </summary>
    public class GroupApplication
    {

        public int GroupApplicationId { get; set; }


        public List<MunityUser> Users { get; set; }

        public AbstractConferenceRole Role { get; set; }

        public Delegation Delegation { get; set; }

        [MaxLength(150)]
        [Required]
        public string Title { get; set; }

        public string Content { get; set; }

        public DateTime ApplicationDate { get; set; }


        public GroupApplication()
        {
            Users = new List<MunityUser>();
        }
    }
}
