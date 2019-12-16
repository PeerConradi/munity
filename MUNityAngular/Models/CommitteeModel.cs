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
    public class CommitteeModel
    {

        [DataMember]
        [PrimaryKey]
        [DatabaseSave("id", DatabaseSaveAttribute.EFieldType.VARCHAR)]
        public string ID { get; set; }

        [DataMember]
        [DatabaseSave("name")]
        public string Name { get; set; }

        [DataMember]
        [DatabaseSave("fullname")]
        public string FullName { get; set; }

        [DataMember]
        [DatabaseSave("abbreviation")]
        public string Abbreviation { get; set; }

        [DataMember]
        [DatabaseSave("article")]
        public string Article { get; set; }

        [DataMember]
        [DatabaseSave("conferenceid")]
        public string ConferenceID { get; set; }

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

        [JsonIgnore]
        public ConferenceModel Conference
        {
            get
            {
                //if (ConferenceID == null)
                //    throw new NullReferenceException("This Committee is not inside an Conference");

                Models.ConferenceModel conference =
                    DataHandlers.Database.ConferenceHandler.GetConference(this.ConferenceID);

                if (conference != null)
                    return conference;
                else
                    Console.WriteLine("Conference with cannot be found!");

                return null;
            }
        }

        

        [JsonIgnore]
        public List<DelegationModel> MyDelegations
        {
            get
            {
                var conf = Conference;

                var tempList = new List<DelegationModel>();

                if (conf == null)
                    return tempList;

                DelegationList.ToList().ForEach(n =>
                {
                    var d = Conference.Delegations.FirstOrDefault(a => a.ID == n);
                    if (d != null)
                        tempList.Add(d);

                });
                return tempList;
            }
        }
        

        public void AddDelegation(DelegationModel delegation)
        {
            //the new way:
            if (Conference != null)
            {
                if (!Conference.Delegations.Contains(delegation) && Conference.Delegations.FirstOrDefault(n => n.ID == delegation.ID) == null)
                {
                    Conference.Delegations.Add(delegation);
                }
            }

           //TODO: Handle douplicate Delegates
            
        }

        public override string ToString()
        {
            return Name;
        }


    }
}
