using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MUNityCore.Schema.Request
{
    public class AdminSchema
    {
        public class CreateUserAuthBody
        {
            [Required]
            public string Name { get; set; }

            [Required]
            public bool CanCreateOrganisation { get; set; }
        }
    }
}
