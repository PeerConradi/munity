using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MUNityAngular.Models
{
    public class ResolutionAdvancedInfoModel : ResolutionInformationModel
    {
        public string OnlineCode { get; set; }

        public bool PublicRead { get; set; }

        public bool PublicWrite { get; set; }

        public bool PublicAmendment { get; set; }

        public DateTime CreationDate { get; set; }

        public DateTime LastChangedDate { get; set; }
    }
}
