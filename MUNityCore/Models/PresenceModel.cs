using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MUNityCore.Models.Resolution;
using MUNityCore.DataHandlers.EntityFramework.Models;
using MUNityCore.Models.Conference;

namespace MUNityCore.Models
{
    public class PresenceModel
    {
        public string Id { get; set; }

        public List<Delegation> Present { get; set; }

        public List<Delegation> Absent { get; set; }

        public List<Delegation> Remaining { get; set; }

        public string CommitteeId { get; set; }

        public DateTime CheckDate { get; set; }
        
        public PresenceModel()
        {
            this.Id = Guid.NewGuid().ToString();
            Present = new List<Delegation>();
            Absent = new List<Delegation>();
            Remaining = new List<Delegation>();
        }

    }
}
