using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using MUNityAngular.Models.Conference;

namespace MUNityAngular.Models.Resolution
{
    public class ResolutionModel
    {

        public delegate void OperativeParagraphAdded(OperativeParagraphModel model);

        public event OperativeParagraphAdded OnOperativeParagraphAdded;

        [BsonId]
        public string ID { get; set; }

        public string Name { get; set; }

        public string FullName { get; set; }

        public string Topic { get; set; }


        public string AgendaItem { get; set; }

        public string Session { get; set; }

        public DateTime Date { get; set; }

        public string ConferenceID { get; set; }

        private ObservableCollection<OperativeParagraphModel> _operativeSections;
        public ObservableCollection<OperativeParagraphModel> OperativeSections 
        { 
            get => _operativeSections;
            set
            {
                _operativeSections = value;
                foreach (var item in value)
                {
                    item.Resolution = this;
                }
            }
        }

        public DocumentTypeModel DocumentType { get; set; }

        public BaseCommitteeModel ResolutlyCommittee { get; set; }
        public int DocumentNumber { get; set; }

        public string CommitteeName { get; set; }

        public List<string> SupporterNames { get; set; }

        public ObservableCollection<DelegationModel> Supporters { get; set; }

        public string SubmitterID { get; set; }

        private PreambleModel _preamble;
        public PreambleModel Preamble
        {
            get => _preamble; 
            set
            {
                this._preamble = value;
                this._preamble.ResolutionID = this.ID;
                foreach(var e in value.Paragraphs)
                {
                    e.ResolutionID = this.ID;
                }
            }
        }

        public string OnlineCode { get; set; }


        /// <summary>
        /// Notices are a List of comments that users can add to the document.
        /// </summary>
        public List<NoticeModel> Notices { get; set; } = new List<NoticeModel>();

        //Make this an read only Observable Collection (only get internal set)
        //And edit its value by just Adding from the different Amendment lists!! 
        private ObservableCollection<AbstractAmendment> _amendments;
        [JsonIgnore]
        [BsonIgnore]
        public ObservableCollection<AbstractAmendment> Amendments { get {
                if (this._amendments == null)
                {
                    this._amendments = new ObservableCollection<AbstractAmendment>();
                }
                return this._amendments;
            } 
            set => this._amendments = value; 
        }

        

        public ResolutionModel(string id = null)
        {
            this.ID = id ?? Guid.NewGuid().ToString();
            this.Topic = "No Name";
            OperativeSections = new ObservableCollection<OperativeParagraphModel>();
            Supporters = new ObservableCollection<DelegationModel>();
            Amendments = new ObservableCollection<AbstractAmendment>();
            Preamble = new PreambleModel();
            Preamble.ResolutionID = this.ID;
            OperativeSections.CollectionChanged += OperativeSections_CollectionChanged;
            Amendments.CollectionChanged += Amendments_CollectionChanged;
            this.Notices = new List<NoticeModel>();
        }

        private void Amendments_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                foreach (var am in e.NewItems.OfType<AddAmendmentModel>())
                {
                    //Abfragen, sonst ruft sich diese Funktion selbst im Kreis auf
                    if (am.TargetResolution != this)
                    {
                        am.TargetResolution = this;
                    }
                    
                }
            }

        }

        private void OperativeSections_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                foreach (var os in OperativeSections)
                {
                    if (os.AmendmentParagraph == false)
                    {
                        os.Resolution = this;
                    }
                    os.UpdatePath();
                }
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
            //Wenn diese Position genommen werden kann
            if (position >= 0 && position < OperativeSections.Count || OperativeSections.Count == 0 && position == 0)
            {
                var paragraph = new OperativeParagraphModel(null, amendmentParagraph);
                paragraph.ResolutionID = this.ID;
                paragraph.Resolution = this;
                paragraph.Text = "";
                //Einfügen des Absatzes an die gegebene Position
                this.OperativeSections.Insert(position, paragraph);
                //OnOperativeParagraphAdded?.Invoke(paragraph);

                //Nummer aller Änderungsanträge auf Verschieben Updaten!
                foreach (var item in MoveAmendments.Where(n => n.TargetSection.Parent == null && OperativeSections.Where(a => a.Parent == null).ToList().IndexOf(n.TargetSection) < position))
                {
                    item.NewPosition += 1;
                }

                //Auch noch die Nummern aller Änderungsanträge auf Hinzufügen verschieben!

                //Update all paths when its inserted
                foreach (var sect in OperativeSections)
                {
                    sect.UpdatePath();
                }
                return paragraph;
            }

            //Falls diese Position außerhalb des Bereichs liegt, ans Ende hängen
            if (position >= this.OperativeSections.Count)
            {
                return this.AddOperativeParagraph();
            }

            return null;
            
        }

        public OperativeParagraphModel AddOperativeParagraph(OperativeParagraphModel paragraphModel)
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

            //Entferne auch alle Unterpunkte
            //Zunächst alle Unterpunkte Sammeln welche entfernt werden soll
            foreach (var child in OperativeSections.Where(n => n.ParentID == paragraphModel.ID))
            {
                DeleteSubSections(child, toDelete);
            }

            //Durch die Unterpunkte gehen und auch alle Änderungsanträge, welche diese betreffen mit entfernen!
            toDelete.ForEach(n =>
            {
                n.Amendments.ToList().ForEach(n => n.Remove());

                //Absatz entfernen
                OperativeSections.Remove(n);
            });
            //Die Änderungsanträge auf den eigentlichen Punkt auch entfernen
            paragraphModel.Amendments.ToList().ForEach(n => n.Remove());

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

        public void RemoveAmendment(AbstractAmendment amendment)
        {
            Amendments.Remove(amendment);
        }

        public IEnumerable<ChangeAmendmentModel> ChangeAmendments
        {
            get
            {
                return Amendments.OfType<ChangeAmendmentModel>();
            }
            set
            {
                foreach(var v in value)
                {
                    v.TargetSection = OperativeSections.FirstOrDefault(n => n.ID == v.TargetSectionID);
                    v.Resolution = this;
                }
            }
        }

        public IEnumerable<DeleteAmendmentModel> DeleteAmendments
        {
            get
            {
                return Amendments.OfType<DeleteAmendmentModel>();
            }
            set
            {
                foreach (var v in value)
                {
                    v.TargetSection = OperativeSections.FirstOrDefault(n => n.ID == v.TargetSectionID);
                    v.Resolution = this;
                }
            }
        }

        public IEnumerable<MoveAmendment> MoveAmendments
        {
            get
            {
                return  Amendments.OfType<MoveAmendment>();
            }
            set
            {
                foreach (var v in value)
                {
                    v.TargetSection = OperativeSections.FirstOrDefault(n => n.ID == v.TargetSectionID);
                }
            }
        }

        public IEnumerable<AddAmendmentModel> AddAmendmentsSave
        {
            get
            {
                return Amendments.OfType<AddAmendmentModel>();
            }
            set {
                foreach (var v in value)
                {
                    v.TargetResolution = this;
                    v.Resolution = this;
                }
            }
        }

        public string SubmitterName { get; set; }

        public void HotFix()
        {
            if (OperativeSections != null)
            {
                foreach (var oa in OperativeSections)
                {
                    oa.Resolution = this;
                }
            }

            if (Preamble != null && Preamble.Paragraphs != null)
            {
                foreach (var ps in Preamble.Paragraphs)
                {
                    ps.Preamble = Preamble;
                }
            }
        }
    }
}
