using MUNityCore.Models.User;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MUNityCore.Models.Conference
{

    /// <summary>
    /// If a user applies to a Role this will create a RoleApplication.
    /// When an Application is Created to the Role there is a Link between the Role itself
    /// The User that made the application plus some more information of this application
    /// </summary>
    public class RoleApplication
    {
        public int RoleApplicationId { get; set; }

        public MunityUser User { get; set; }


        public AbstractRole Role { get; set; }

        public DateTime ApplyDate { get; set; }

        [Required]
        [MaxLength(200)]
        public string Title { get; set; }

        
        public string Content { get; set; }

        [Timestamp]
        public byte[] RoleApplicationTimestamp { get; set; }
    }
}
