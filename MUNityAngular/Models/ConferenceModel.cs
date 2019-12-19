using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using MUNityAngular.DataHandlers.Database;

namespace MUNityAngular.Models
{

    /// <summary>
    /// The Conference holds all needed Data to show Committees, NGOs
    /// 
    /// </summary>
    public class ConferenceModel
    {

        [PrimaryKey]
        [DatabaseSave("id", DatabaseSaveAttribute.EFieldType.VARCHAR)]
        public string ID { get; set; }

        [DatabaseSave("name")]
        public string Name { get; set; }


        [DatabaseSave("fullname")]
        public string FullName { get; set; }

        [DatabaseSave("abbreviation")]
        public string Abbreviation { get; set; }

        public List<CommitteeModel> Committees { get; set; }

        [DatabaseSave("startdate")]
        public DateTime StartDate { get; set; }

        [DatabaseSave("enddate")]
        public DateTime EndDate { get; set; }

        [DatabaseSave("secretarygeneraltitle")]
        public string SecretaryGeneralTitle { get; set; }

        [DatabaseSave("secretarygeneralname")]
        public string SecretaryGerneralName { get; set; }

        [DatabaseSave("creationdate")]
        public DateTime CreationDate { get; set; }

        public ObservableCollection<DelegationModel> NGOs { get; set; }

        public ConferenceModel(string id = null)
        {
            this.ID = id ?? Guid.NewGuid().ToString();
            Committees = new List<CommitteeModel>();
            Delegations = new List<DelegationModel>();
            NGOs = new ObservableCollection<DelegationModel>();
        }

        public ConferenceModel()
        {
            this.ID = Guid.NewGuid().ToString();
            Committees = new List<CommitteeModel>();
            Delegations = new List<DelegationModel>();
            NGOs = new ObservableCollection<DelegationModel>();
        }

        /// <summary>
        /// New: In the future all Delegations should be saved inside the Conference and not
        /// inside the Committees. The Committees only hold a list of IDs that represents the Committees inside them
        /// this way the file will get a lot smaller
        /// </summary>
        public List<DelegationModel> Delegations { get; set; }

        [JsonIgnore]
        public List<DelegationModel> DelegationsAndNGOs
        {
            
            get
            {
                List<DelegationModel> list = new List<DelegationModel>();
                list.AddRange(Delegations);
                list.AddRange(NGOs);
                return list;
            }
        }

        /// <summary>
        /// Returns the Full Name of the Conference.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return FullName;
        }

        /// <summary>
        /// Adds a Committee to the committee list, that is not inside this list already.
        /// Use this function instead of Calling the Committee List itself.
        /// </summary>
        /// <param name="committee"></param>
        public void AddCommittee(CommitteeModel committee)
        {
            if (!Committees.Contains(committee))
            {
                Committees.Add(committee);
                committee.ConferenceID = this.ID;
            }
        }
    }
}
