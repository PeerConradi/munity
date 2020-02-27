using MUNityAngular.DataHandlers.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MUNityAngular.Models.Resolution
{
    public class ResolutionConferenceModel
    {
        [DatabaseSave("linkid")]
        [PrimaryKey]
        public int LinkId { get; set; }

        [DatabaseSave("resolutionid")]
        public string ResolutionId { get; set; }

        [DatabaseSave("conferenceid")]
        public string ConferenceId { get; set; }

        [DatabaseSave("committeeid")]
        public string CommitteeId { get; set; }
    }
}
