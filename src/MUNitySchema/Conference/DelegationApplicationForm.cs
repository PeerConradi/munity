using System;
using System.Collections.Generic;
using System.Text;

namespace MUNity.Schema.Conference
{
    public class DelegationApplicationForm
    {
        public List<AddingToApplicationUser> Users { get; set; } = new List<AddingToApplicationUser>();

        public Dictionary<string, string> FieldInputs { get; set; } = new Dictionary<string, string>();

    }
}
