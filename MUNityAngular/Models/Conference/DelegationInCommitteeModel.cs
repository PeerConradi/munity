using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MUNityAngular.DataHandlers.Database;

namespace MUNityAngular.Models.Conference
{
    public class DelegationInCommitteeModel
    {

        [DatabaseSave("delincommitteeid")]
        [PrimaryKey]
        public int Id { get; set; }

        [DatabaseSave("linkid")]
        public int LinkId { get; set; }

        [DatabaseSave("committeeid")]
        public string CommitteeId { get; set; }

        [DatabaseSave("mincount")]
        public int MinCount { get; set; }

        [DatabaseSave("maxcount")]
        public int MaxCount { get; set; }
    }
}
