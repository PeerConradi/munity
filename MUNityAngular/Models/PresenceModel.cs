using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MUNityAngular.Models.Resolution;
using MUNityAngular.Models.Conference;

namespace MUNityAngular.Models
{
    public class PresenceModel
    {
        public string Id { get; set; }

        public List<DelegationModel> Present { get; set; }

        public List<DelegationModel> Absent { get; set; }

        public List<DelegationModel> Remaining { get; set; }

        public string CommitteeId { get; set; }

        public DateTime CheckDate { get; set; }
        
        public PresenceModel()
        {
            this.Id = Guid.NewGuid().ToString();
            Present = new List<DelegationModel>();
            Absent = new List<DelegationModel>();
            Remaining = new List<DelegationModel>();
        }

    }
}
