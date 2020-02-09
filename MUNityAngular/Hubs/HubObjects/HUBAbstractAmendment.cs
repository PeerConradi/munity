using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MUNityAngular.Models.Resolution;

namespace MUNityAngular.Hubs.HubObjects
{
    public class HUBAbstractAmendment
    {
        public string ID { get; set; }

        public string Name { get; set; }

        public string TargetSectionID { get; set; }

        public bool Activated { get; set; }

        public string SubmitterName { get; set; }

        public DateTime SubmitTime { get; set; }

        public string Type { get; set; }

        public HUBAbstractAmendment(AbstractAmendment amendment)
        {
            this.ID = amendment.ID;
            this.Name = amendment.Name;
            this.TargetSectionID = amendment.TargetSectionID;
            this.Activated = amendment.Activated;
            this.SubmitterName = amendment.SubmitterName;
            this.SubmitTime = amendment.SubmitTime;
            this.Type = amendment.Type;
        }

        public HUBAbstractAmendment()
        {
            ID = Guid.NewGuid().ToString();
        }
    }
}
