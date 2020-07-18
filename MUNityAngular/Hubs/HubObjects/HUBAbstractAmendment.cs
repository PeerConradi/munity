using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MUNityAngular.Models.Resolution;

namespace MUNityAngular.Hubs.HubObjects
{

    /// <summary>
    /// The HUBAbstractAmendment is a one on one copy of the AbstractAmendmentModel.
    /// The only difference is:
    /// It only stores Data and has absolutly no logic. We need to create this class
    /// because SignalR will otherwise send Data that is tagged with [JsonIgnore].
    /// Its not actualy an Abstract class in this case!
    /// </summary>
    public class HUBAbstractAmendment
    {
        public string ID { get; set; }

        public string Name { get; set; }

        public string TargetSectionID { get; set; }

        public bool Activated { get; set; }

        public string SubmitterName { get; set; }

        public DateTime SubmitTime { get; set; }

        public string Type { get; set; }

        public HUBAbstractAmendment(AbstractAmendment amendmentModel)
        {
            this.ID = amendmentModel.ID;
            this.Name = amendmentModel.Name;
            this.TargetSectionID = amendmentModel.TargetSectionID;
            this.Activated = amendmentModel.Activated;
            this.SubmitterName = amendmentModel.SubmitterName;
            this.SubmitTime = amendmentModel.SubmitTime;
            this.Type = amendmentModel.Type;
        }

        public HUBAbstractAmendment()
        {
            ID = Guid.NewGuid().ToString();
        }
    }
}
