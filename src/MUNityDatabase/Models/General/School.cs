using MUNity.Database.General;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MUNity.Database.Models.General
{
    public class School
    {
        public int SchoolId { get; set; }

        public string SchoolName { get; set; }

        public string SchoolFullName { get; set; }

        public Country Country { get; set; }

        [MaxLength(200)]
        public string Street { get; set; }

        [MaxLength(10)]
        public string HouseNumber { get; set; }

        [MaxLength(200)]
        public string City { get; set; }

        [MaxLength(200)]
        public string State { get; set; }

        [MaxLength(20)]
        public string PostalCode { get; set; }
    }
}
