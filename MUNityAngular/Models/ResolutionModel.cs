using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace MUNityAngular.Models
{
    public class ResolutionModel
    {

        public delegate void OperativeParagraphAdded(OperativeParagraphModel model);
        public event OperativeParagraphAdded OnOperativeParagraphAdded;

        public string ID { get; set; }

        public string Name { get; set; }

        public string FullName { get; set; }

        public string Topic { get; set; }
        public string AgendaItem { get; set; }

        public string Session { get; set; }

        public DateTime Date { get; set; }

        public string ConferenceID { get; set; }

        [JsonIgnore]
        public ConferenceModel Conference
        {
            get
            {
                //Get Conference from Service
                var res = DataHandlers.Database.ConferenceHandler.GetConference(ConferenceID);

                if (res == null)
                {
                    Console.WriteLine("Fehler beim suchen der Konferenz mit der ID: " + ConferenceID);
                }

                return res;
            }
        }

        public ObservableCollection<OperativeParagraphModel> OperativeSections { get; set; }

        [JsonIgnore]
        public IEnumerable<OperativeParagraphModel> OperativeSectionsNoVirtual
        {
            get
            {
                return OperativeSections.Where(n => n.AmendmentParagraph == false);
            }
        }

        public DocumentTypeModel DocumentType { get; set; }

        public string CommitteeID { get; set; }

        public string ResolutlyCommitteeID { get; set; }
        public int DocumentNumber { get; set; }

        [JsonIgnore]
        public ObservableCollection<DelegationModel> Supporters { get; set; }

        private List<string> _supporterIDs;
        public List<string> SupporterIDs
        {
            get => Supporters.Select(n => n.ID).ToList();
            set
            {
                _supporterIDs = value;
                value.ForEach(n =>
                {
                    Models.DelegationModel delegation = DataHandlers.Database.DelegationHandler.GetDelegation(n);
                    if (delegation != null)
                        Supporters.Add(delegation);
                });
            }
        }

        [JsonIgnore]
        public ObservableCollection<AddAmendmentModel> AddAmendments { get; set; } = new ObservableCollection<AddAmendmentModel>();

        public string Filepath { get; set; } = null;

        public DelegationModel Submitter { get; set; }

        public PreambleModel Preamble { get; set; }

        [JsonIgnore]
        public CommitteeModel Committee { get; set; } 

        private CommitteeModel _resolutlyCommittee;
        [JsonIgnore]
        public CommitteeModel ResolutelyCommittee { get => DataHandlers.Database.CommitteeHandler.GetCommittee(ResolutlyCommitteeID); }

        //Make this an read only Observable Collection (only get internal set)
        //And edit its value by just Adding from the different Amendment lists!! 
        [JsonIgnore]
        public ObservableCollection<AbstractAmendment> Amendments { get; set; }

        

        public ResolutionModel(string id = null)
        {
            this.ID = id ?? Guid.NewGuid().ToString();
            OperativeSections = new ObservableCollection<OperativeParagraphModel>();
            Supporters = new ObservableCollection<DelegationModel>();
            Amendments = new ObservableCollection<AbstractAmendment>();
            SupporterIDs = new List<string>();
            Preamble = new PreambleModel();
            OperativeSections.CollectionChanged += OperativeSections_CollectionChanged;
            Supporters.CollectionChanged += Supporters_CollectionChanged;
            Amendments.CollectionChanged += Amendments_CollectionChanged;

        }

        private void Amendments_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                foreach (var am in e.NewItems.OfType<AddAmendmentModel>())
                {
                    am.TargetResolution = this;
                }
            }

        }

        private void OperativeSections_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                foreach(var item in e.NewItems.OfType<OperativeParagraphModel>().Where(n => n.AmendmentParagraph == false))
                {
                    item.Amendments.CollectionChanged += AmendmentsInOperativeSectionChanged;
                }
            }
            foreach(var os in OperativeSections)
            {
                os.UpdatePath();
            }
        }

        private void Supporters_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            SupporterIDs.Clear();
            Supporters.ToList().ForEach(n => SupporterIDs.Add(n.ID));
        }

        private void AmendmentsInOperativeSectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case System.Collections.Specialized.NotifyCollectionChangedAction.Add:
                    foreach(var newItem in e.NewItems.OfType<AbstractAmendment>())
                    {
                        AddAmendment(newItem);
                    }
                    break;
                case System.Collections.Specialized.NotifyCollectionChangedAction.Remove:
                    break;
                case System.Collections.Specialized.NotifyCollectionChangedAction.Replace:
                    break;
                case System.Collections.Specialized.NotifyCollectionChangedAction.Move:
                    break;
                case System.Collections.Specialized.NotifyCollectionChangedAction.Reset:
                    break;
                default:
                    break;
            }
        }

        public OperativeParagraphModel AddOperativeParagraph(string text = "")
        {
            OperativeParagraphModel paragraph = new OperativeParagraphModel();
            paragraph.ResolutionID = this.ID;
            paragraph.Text = text;
            paragraph.Resolution = this;
            this.OperativeSections.Add(paragraph);
            OnOperativeParagraphAdded?.Invoke(paragraph);
            return paragraph;

        }

        public OperativeParagraphModel AddOperativeParagraph(int position, bool amendmentParagraph = false)
        {
            if (position >= 0 && position < OperativeSections.Count || OperativeSections.Count == 0 && position == 0)
            {
                OperativeParagraphModel paragraph = new OperativeParagraphModel(null, amendmentParagraph);
                paragraph.ResolutionID = this.ID;
                paragraph.Resolution = this;
                paragraph.Text = "";
                this.OperativeSections.Insert(position, paragraph);
                OnOperativeParagraphAdded?.Invoke(paragraph);
                return paragraph;
            }

            if (position >= this.OperativeSections.Count)
            {
                return this.AddOperativeParagraph();
            }

            //Update all paths when its inserted
            foreach(var sect in OperativeSections)
            {
                sect.UpdatePath();
            }

            return null;
            
        }

        public OperativeParagraphModel AddOperativeParagraph(Models.OperativeParagraphModel paragraphModel)
        {
            paragraphModel.ResolutionID = this.ID;
            paragraphModel.Resolution = this;
            this.OperativeSections.Add(paragraphModel);
            OnOperativeParagraphAdded?.Invoke(paragraphModel);
            return paragraphModel;
        }

        public string SupportersList
        {
            get
            {
                string suppList = "";
                if (this.Supporters.Count > 0)
                {
                    for (int i = 0; i < this.Supporters.Count - 1; i++)
                    {
                        suppList += Supporters[i].Name;
                        suppList += ", ";
                    }
                    suppList += Supporters[Supporters.Count - 1].Name;
                }
                return suppList;
            }
        }
        
        public void RemoveOperativeSection(OperativeParagraphModel paragraphModel)
        {
            if (paragraphModel == null)
                return;

            List<OperativeParagraphModel> toDelete = new List<OperativeParagraphModel>();

            foreach (var child in OperativeSections.Where(n => n.ParentID == paragraphModel.ID))
            {
                DeleteSubSections(child, toDelete);
            }

            toDelete.ForEach(n => OperativeSections.Remove(n));
            OperativeSections.Remove(paragraphModel);
        }

        private void DeleteSubSections(OperativeParagraphModel paragraphModel, List<OperativeParagraphModel> todelete)
        {
            todelete.Add(paragraphModel);
            foreach (var child in OperativeSections.Where(n => n.ParentID == paragraphModel.ID))
            {
                DeleteSubSections(child, todelete);
            }
        }

        public override string ToString()
        {
            return this.Topic;
        }

        public string SaveToLocal()
        {
            return DataHandlers.FileSystem.ResolutionHandler.SaveResolutionToLocalFile(this);
        }

        public void AddAmendment(AbstractAmendment amendment)
        {
            //Clear the list so it can be reordered
            Amendments.Clear();

            //Add the Amendment to its Target Section if its not already inside
            if (amendment.TargetSection != null)
            {
                if (amendment.TargetSection.Amendments.FirstOrDefault(n => n.ID == amendment.ID) == null &&
                    !amendment.TargetSection.Amendments.Contains(amendment) &&
                    !amendment.TargetSection.AmendmentParagraph)
                {
                    amendment.TargetSection.Amendments.Add(amendment);
                }
            }
            else
            {
                //If it is an Add Amendment add it to the AddAmendments
                if (amendment is Models.AddAmendmentModel amend)
                {
                    if (!AddAmendments.Contains(amend))
                    {
                        AddAmendments.Add(amend);
                        if (amend.TargetResolution != this)
                            amend.TargetResolution = this;
                    }
                }
            }
            
            
            //Hotfixxed to be redone

            //Go through all the Amendments that are connected to a section and add them in the ORDER that
            //they supposed to have
            OperativeSections.Where(n => n.AmendmentParagraph == false).ToList().ForEach(n => {
                foreach(var am in n.Amendments.OrderBy(a => a.ORDER_LEVEL))
                {
                    if (!Amendments.Contains(am))
                    {
                        Amendments.Add(am);
                    }
                        
                }
            });

            //Add all the Add Amendments
            foreach (var am in AddAmendments)
            {
                if (!Amendments.Contains(am))
                    Amendments.Add(am);
            }
        }

        public void RemoveAmendment(AbstractAmendment amendment)
        {
            if (amendment is Models.AddAmendmentModel a)
            {
                AddAmendments.Remove(a);
            }

            OperativeSections.ToList().ForEach(n => n.Amendments.Remove(amendment));
            Amendments.Remove(amendment);
        }

        public IEnumerable<Models.ChangeAmendmentModel> ChangeAmendments
        {
            get
            {
                return Amendments.OfType<Models.ChangeAmendmentModel>();
            }
            set
            {
                foreach(var v in value)
                {
                    //Amendments.Add(v);
                    //Managers.ResolutionManager.GetInstance().FindParagraph(v.TargetSectionID)?.Amendments.Add(v);
                    AddAmendment(v);
                }
            }
        }

        public IEnumerable<DeleteAmendmentModel> DeleteAmendments
        {
            get => Amendments.OfType<DeleteAmendmentModel>();
            set
            {
                foreach (var v in value)
                {
                    //Amendments.Add(v);
                    //Managers.ResolutionManager.GetInstance().FindParagraph(v.TargetSectionID)?.Amendments.Add(v);
                    AddAmendment(v);
                }
            }
        }

        public IEnumerable<MoveAmendment> MoveAmendments
        {
            get => Amendments.OfType<MoveAmendment>();
            set
            {
                foreach (var v in value)
                {
                    //Managers.ResolutionManager.GetInstance().FindParagraph(v.TargetSectionID)?.Amendments.Add(v);
                    AddAmendment(v);
                }
            }
        }

        public IEnumerable<AddAmendmentModel> AddAmendmentsSave
        {
            get => Amendments.OfType<AddAmendmentModel>();
            set {
                foreach (var v in value)
                {
                    AddAmendment(v);
                }
            }
        }

    }
}
