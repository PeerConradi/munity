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
    public class Project
    {
        public string ProjectId { get; set; }

        public string ProjectName { get; set; }

        public string ProjectShort { get; set; }

        public Organization.Organization ProjectOrganization { get; set; }

        public List<Conference> Conferences { get; set; }

        [Timestamp]
        public byte[] ProjectTimestamp { get; set; }

        public Project()
        {
            ProjectId = Guid.NewGuid().ToString();
        }
    }
}
