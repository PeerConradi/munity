using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MUNity.Schema.Project
{
    public class CreateProjectModel
    {
        [Required]
        public string OrganizationId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Short { get; set; }
    }

    public class CreateProjectResponse
    {
        public enum CreateProjectStatus
        {
            Success,
            Error,
            NameTaken,
            ShortTaken,
            OrganizationNotFound,
            NoRights
        }

        public CreateProjectStatus Status { get; set; }

        public string ProjectId { get; set; }
    }
}
