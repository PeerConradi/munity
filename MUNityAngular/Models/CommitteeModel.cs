using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using MUNityAngular.DataHandlers.Database;
using Newtonsoft.Json;

namespace MUNityAngular.Models
{

    [DataContract]
    public class CommitteeModel : BaseCommitteeModel
    {

        [DataMember]
        [DatabaseSave("conferenceid")]
        public string ConferenceID { get; set; }

        [DataMember]
        [DatabaseSave("resolutlycommittee")]
        public string ResolutlyCommitteeID { get; set; }

        [DataMember]
        public List<string> DelegationList { get; set; }

        public CommitteeModel(string id = null)
        {
            this.ID = id ?? Guid.NewGuid().ToString();
            DelegationList = new List<string>();
        }

        public CommitteeModel()
        {
            this.ID = Guid.NewGuid().ToString();
            DelegationList = new List<string>();
        }

        public override string ToString()
        {
            return Name;
        }


    }
}
