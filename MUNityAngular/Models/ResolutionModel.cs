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

        public ObservableCollection<DelegationModel> Supporters { get; set; }

        public string SubmitterID { get; set; }

        public PreambleModel Preamble { get; set; }

        
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
            Preamble = new PreambleModel();
            OperativeSections.CollectionChanged += OperativeSections_CollectionChanged;
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
                    item.Resolution = this;
                }
            }
            foreach(var os in OperativeSections)
            {
                os.Resolution = this;
                os.UpdatePath();
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
                foreach (var item in MoveAmendments.Where(n => n.TargetSection.Parent == null && OperativeSections.Where(a => a.Parent == null).ToList().IndexOf(n.TargetSection) < position))
                {
                    item.NewPosition += 1;
                }
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

        public void RemoveAmendment(AbstractAmendment amendment)
        {
            Amendments.Remove(amendment);
        }

        public IEnumerable<ChangeAmendmentModel> ChangeAmendments
        {
            get
            {
                return Amendments.OfType<Models.ChangeAmendmentModel>();
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
            get => Amendments.OfType<DeleteAmendmentModel>();
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
            get => Amendments.OfType<MoveAmendment>();
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
            get => Amendments.OfType<AddAmendmentModel>();
            set {
                foreach (var v in value)
                {
                    v.TargetResolution = this;
                    v.Resolution = this;
                }
            }
        }

    }
}
