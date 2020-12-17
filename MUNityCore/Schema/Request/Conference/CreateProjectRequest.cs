﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MUNityCore.Schema.Request.Conference
{
    public class CreateProjectRequest
    {
        [Required]
        public string OrganisationId { get; set; }

        [Required]
        [MaxLength(300)]
        public string Name { get; set; }

        [Required]
        [MaxLength(16)]
        public string Abbreviation { get; set; }
    }
}