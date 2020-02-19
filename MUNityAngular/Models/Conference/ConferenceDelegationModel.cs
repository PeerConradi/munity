using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MUNityAngular.DataHandlers.Database;

namespace MUNityAngular.Models.Conference
{
    public class ConferenceDelegationModel
    {

        [DatabaseSave("linkid")]
        [PrimaryKey]
        public int Id { get; set; }

        [DatabaseSave("conference_id")]
        public string ConferenceId { get; set; }

        [DatabaseSave("delegation_id")]
        public string DelegationId { get; set; }

        [DatabaseSave("mincount")]
        public int MinCount { get; set; }

        [DatabaseSave("maxcount")]
        public int MaxCount { get; set; }
    }
}
