using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MUNityAngular.DataHandlers.Database;

namespace MUNityAngular.Models.Resolution
{
    public class ResolutionAdvancedInfoModel : ResolutionInformationModel
    {

        [DatabaseSave("onlinecode")]
        public string OnlineCode { get; set; }

        [DatabaseSave("ispublicread")]
        public bool PublicRead { get; set; }

        [DatabaseSave("ispublicwrite")]
        public bool PublicWrite { get; set; }

        [DatabaseSave("allowamendments")]
        public bool PublicAmendment { get; set; }

        [DatabaseSave("creationdate")]
        public DateTime CreationDate { get; set; }

        [DatabaseSave("lastchangeddate")]
        public DateTime LastChangedDate { get; set; }

        public ResolutionAdvancedInfoModel()
        {

        }

        public ResolutionAdvancedInfoModel(ResolutionModel resolution, string userid)
        {
            this.ID = resolution.ID;
            this.Name = resolution.Name;
        }
    }
}
