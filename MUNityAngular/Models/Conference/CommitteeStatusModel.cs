using MUNityAngular.DataHandlers.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MUNityAngular.Models.Conference
{
    public class CommitteeStatusModel
    {
        [DatabaseSave("id")]
        [PrimaryKey]
        public int? Id { get; set; }

        [DatabaseSave("committeeid")]
        public string CommitteeId { get; set; }

        [DatabaseSave("status")]
        public string Status { get; set; }

        [DatabaseSave("agendaitem")]
        public string AgendaItem { get; set; }

        [DatabaseSave("timestamp")]
        public DateTime Timestamp { get; set; }
    }
}
