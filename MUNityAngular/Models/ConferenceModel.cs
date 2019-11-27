using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace MUNityAngular.Models
{

    /// <summary>
    /// The Conference holds all needed Data to show Committees, NGOs
    /// 
    /// </summary>
    public class ConferenceModel
    {

        public string ID { get; set; }

        public string Name { get; set; }

        public string FullName { get; set; }

        public string Abbreviation { get; set; }

        public ObservableCollection<CommitteeModel> Committees { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public string SecretaryGeneralTitle { get; set; }

        public string SecretaryGerneralName { get; set; }

        public ObservableCollection<DelegationModel> NGOs { get; set; }

        /// <summary>
        /// The conference holds a List of Resolution that it expects to be loaded
        /// </summary>
        public ObservableCollection<string> ResolutionIDs { get; set; }

        /// <summary>
        /// DO NOT TOUCH, DO NOT ADD ITEMS OUTSIDE OF THE RESOLUTION MANAGER TO THIS!
        /// </summary>
        [JsonIgnore]
        public ObservableCollection<ResolutionModel> AvailableResolutions { get; set; }

        public ConferenceModel(string id = null)
        {
            this.ID = id ?? Guid.NewGuid().ToString();
            Committees = new ObservableCollection<CommitteeModel>();
            ResolutionIDs = new ObservableCollection<string>();
            AvailableResolutions = new ObservableCollection<ResolutionModel>();
            Delegations = new ObservableCollection<DelegationModel>();
            NGOs = new ObservableCollection<DelegationModel>();
        }

        /// <summary>
        /// New: In the future all Delegations should be saved inside the Conference and not
        /// inside the Committees. The Committees only hold a list of IDs that represents the Committees inside them
        /// this way the file will get a lot smaller
        /// </summary>
        public ObservableCollection<DelegationModel> Delegations { get; set; }

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
