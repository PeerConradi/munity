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
    /// A delegation is a model that groups different types of delegate roles
    ///
    /// <seealso cref="Roles.ConferenceDelegateRole"/>
    /// </summary>
    public class Delegation
    {
        public string DelegationId { get; set; }

        [MaxLength(150)]
        [Required]
        public string Name { get; set; }

        [MaxLength(250)]
        public string FullName { get; set; }

        /// <summary>
        /// Short Term with a max length of 10 characters
        /// </summary>
        [MaxLength(10)]
        public string DelegationShort { get; set; }

        public Conference Conference { get; set; }

        public Delegation()
        {
            DelegationId = Guid.NewGuid().ToString();
        }
    }
}
