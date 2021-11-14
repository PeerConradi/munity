using System;
using System.Collections.Generic;
using System.Text;

namespace MUNity.Schema.Account
{
    public class UpdateProfileRequest
    {
        public string UserName { get; set; }

        public int UpdatedBirthdayDay { get; set; }

        public int UpdatedBirthdayMonth { get; set; }

        public int UpdatedBirthdayYear { get; set; }

        public string UpdatedCountry { get; set; }

        public string UpdatedZipCode { get; set; }

        public string UpdatedCity { get; set; }

        public string UpdatedStreet { get; set; }

        public string UpdatedHouseNumber { get; set; }

        public string UpdateForename { get; set; }

        public string UpdateLastname { get; set; }
    }
}
