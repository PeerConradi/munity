using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using MUNity.Database.Interfaces;
using MUNity.Database.Models.User;

namespace MUNity.Database.Models.Conference
{

    /// <summary>
    /// An organization can host different projects. For example could the project group
    /// all conferences inside a specific city over the years:
    /// Model United Nations Berlin 2015
    /// Model United Nations Berlin 2016
    ///
    /// The organization could also group different styles of conferences inside a project
    /// for example:
    /// Model United Nations in the classroom
    /// Model United Nations in the university
    /// </summary>
    public class Project : IIsDeleted
    {
        public string ProjectId { get; set; } = "";

        [MaxLength(250)]
        public string ProjectName { get; set; }

        [MaxLength(20)]
        public string ProjectShort { get; set; }

        public Organization.Organization ProjectOrganization { get; set; }

        public ICollection<Conference> Conferences { get; set; }

        public MunityUser CreationUser { get; set; }

        public Project()
        {
            
        }

        public bool IsDeleted { get; set; }
    }
}
