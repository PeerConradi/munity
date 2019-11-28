using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace MUNityAngular.Models
{
    public class CommitteeModel
    {
        public string ID { get; set; }

        public string Name { get; set; }

        public string FullName { get; set; }

        public string Abbreviation { get; set; }

        public string Article { get; set; }

        public ObservableCollection<Models.SubjectModel> Subjects;



        [JsonIgnore]
        public PresenceModel Presents { get; set; }

        public string ConferenceID { get; set; }

        [JsonIgnore]
        public SpeakerlistModel Speakerlist { get; set; }

        public CommitteeModel(string id = null)
        {
            this.ID = id ?? Guid.NewGuid().ToString();
            Speakerlist = new SpeakerlistModel();
            Subjects = new ObservableCollection<SubjectModel>();
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

        public List<string> DelegationList { get; set; }

        [JsonIgnore]
        public List<DelegationModel> MyDelegations
        {
            get
            {
                var conf = Conference;

                var tempList = new List<DelegationModel>();

                if (conf == null)
                    return tempList;

                DelegationList.ForEach(n =>
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
            if (this.DelegationList.FirstOrDefault(n => n == delegation.ID) == null)
                this.DelegationList.Add(delegation.ID);
        }

        public void AddSubject(string subjectName)
        {
            Models.SubjectModel newSubject = new SubjectModel();
            newSubject.Name = subjectName;
            Subjects.Add(newSubject);
        }


        public override string ToString()
        {
            return Name;
        }


    }
}
