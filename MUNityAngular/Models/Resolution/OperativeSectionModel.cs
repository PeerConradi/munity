using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace MUNityAngular.Models.Resolution
{

    /// <summary>
    /// The Operative Paragraph is Part of the OperativeSection.
    /// </summary>
    [DataContract]
    public class OperativeParagraphModel
    {

        //public delegate void AmendmentAdded(AbstractAmendment amendment);
        //public event AmendmentAdded OnAmendmentAdded;

        public enum EViewModus
        {
            Normal,
            Remove,
            Overwrite,
            Ghost, 
            Add,
            Highlight
        }

        private enum EMoveDirection
        {
            Up,
            Down
        }

        /// <summary>
        /// This ID has to be unique inside each Resolution.
        /// It is a Guid so it will be unique and you can find the Document with just one Operative Paragraph ID
        /// </summary>
        [DataMember]
        [BsonId]
        public string ID { get; set; }

        /// <summary>
        /// Every Paragraph can have a Visible Name to make it easier for the user to identify
        /// it. The Name doesn't has to be like the Text. The default Name will be the Text first characters.
        /// </summary>
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public bool IsLocked { get; set; }
        
        [DataMember]
        public bool IsVirtual { get; set; }
        
        [DataMember]
        public string Text { get; set; }

        [DataMember]
        public List<NoticeModel> Notices { get; set; }

        [JsonIgnore]
        [IgnoreDataMember]
        [BsonIgnore]
        public ResolutionModel Resolution { get; set; }

        /// <summary>
        /// The Paragraph could be hidden from the User.
        /// </summary>
        [DataMember]
        public bool Visible { get; set; }

        private string _parentid;
        [DataMember]
        public string ParentID
        {
            get => _parentid;
            set
            {
                _parentid = value;

                if (value == null)
                {
                    if (this.Parent != null)
                        this.Parent.Children.Remove(this.ID);
                }

                if (this.Parent != null)
                {
                    if (!this.Parent.Children.Contains(this.ParentID) && value != null)
                        this.Parent.Children.Add(this.ID);
                }

                
            }
        }

        [DataMember]
        public string ResolutionID { get; set; }

        [DataMember]
        public EViewModus ViewModus { get; set; }

        [DataMember]
        public bool AmendmentParagraph { get; set; }

        [JsonIgnore]
        [IgnoreDataMember]
        [BsonIgnore]
        public AbstractAmendment ActiveAmendment { get; set; }

        [JsonIgnore]
        [IgnoreDataMember]
        [BsonIgnore]
        public OperativeParagraphModel Parent
        {
            get
            {
                if (ParentID == null) return null;
                if (Resolution!= null)
                    return Resolution.OperativeSections.FirstOrDefault(n => n.ID == ParentID);
                return null;
            }
        }

        //Ignore this, it is only Get
        //[JsonIgnore]
        //[IgnoreDataMember]
        //[BsonIgnore]
        public short Level
        {
            get
            {
                
                if (ParentID == null) return 0;

                short level = 1;
                OperativeParagraphModel parent = Parent;

                if (parent == null)
                    return level;

                while (parent.ParentID != null)
                {
                    level++;
                    parent = parent.Parent;
                }
                return level;
            }
        }

        [DataMember]
        public ObservableCollection<string> Children { get; set; }

        [JsonIgnore]
        [IgnoreDataMember]
        [BsonIgnore]
        public bool CanMoveLeft { get; set; }

        private string _path;
        [DataMember]
        public string Path
        {
            get
            {
                UpdatePath();
                return _path;
            }
            set
            {
                _path = value;
            }
        }

        
        [DataMember]
        public int DeleteAmendmentCount { get => Amendments.OfType<DeleteAmendmentModel>().Count(); }

        [DataMember]
        public int ChangeAmendmentCount { get => Amendments.OfType<ChangeAmendmentModel>().Count(); }

        [DataMember]
        public int MoveAmendmentCount { get => Amendments.OfType<MoveAmendment>().Count(); }


        private static readonly string levelTwoLetters = "abcdefghijklmnopqrstuvwxyz";

        public void UpdatePath()
        {
            //Get the Index of this In Context to All the parents
            if (string.IsNullOrEmpty(this.ParentID) || Parent == null)
            {
                Path = (Resolution.OperativeSections.Where(n =>
                    string.IsNullOrEmpty(n.ParentID)).ToList().IndexOf(this) + 1).ToString();
            }
            else
            {
                string p = "";

                p = Parent.Path;
                p += ".";

                int myIndex = Parent.GetChildren().ToList().IndexOf(this);

                if (this.Level == 0)
                {
                    //Zahl
                    p += (myIndex + 1).ToString();
                }
                else if (this.Level ==  1)
                {
                    //Buchstabe
                    if (myIndex < 25)
                    {
                        p += levelTwoLetters[myIndex];
                    }
                }
                else if (this.Level == 2)
                {
                    //i
                    for (int i = 0; i <= myIndex; i++)
                    {
                        p += "i";
                    }
                }
                Path = p;
            }

            
        }

        [JsonIgnore]
        [IgnoreDataMember]
        [BsonIgnore]
        public IEnumerable<AbstractAmendment> Amendments
        {
            get
            {
                if (Resolution == null)
                    return new List<AbstractAmendment>();

                if (Resolution.Amendments == null)
                {
                    Resolution.Amendments = new ObservableCollection<AbstractAmendment>();
                }
                return Resolution?.Amendments.Where(n => n.TargetSection == this);
            }
        }

        public OperativeParagraphModel(string id = null, bool amendmentParagraph = false)
        {
            this.AmendmentParagraph = amendmentParagraph;
            this.ID = id ?? Guid.NewGuid().ToString();
            Children = new ObservableCollection<string>();
        }

        public OperativeParagraphModel AddSubSection(string text = "")
        {
            OperativeParagraphModel paragraph = new OperativeParagraphModel();
            paragraph.ResolutionID = this.ResolutionID;
            paragraph.Text = text;
            paragraph.ParentID = this.ID;
            if (!Children.Contains(paragraph.ID))
                this.Children.Add(paragraph.ID);

            int? pos = Resolution.OperativeSections.IndexOf(this);
            if (pos != null)
            {
                if ((int)pos + this.Children.Count > Resolution.OperativeSections.Count - 1)
                    Resolution.OperativeSections.Add(paragraph);
                else
                    Resolution.OperativeSections.Insert((int)pos + this.Children.Count, paragraph);
            }
                
            else
                Resolution.AddOperativeParagraph(paragraph);

            return paragraph;
        }

        public void MoveUp()
        {
            int index = Resolution.OperativeSections.IndexOf(this);
            if (ParentID == null)
            {
                //If this is a Main Point move me Up in the Mainscale
                if (index > 0)
                {
                    Resolution.OperativeSections.Move(index, index - 1);
                    MoveChildrens(this, EMoveDirection.Up);
                }
                    

            }
            else
            {
                //Find the First Sub Object
                if (Resolution.OperativeSections.IndexOf(
                    Resolution.OperativeSections.First(n => n.ParentID == this.ParentID)) < index)
                {
                    Resolution.OperativeSections.Move(index, index - 1);
                    MoveChildrens(this, EMoveDirection.Up);
                }
            }

            UpdatePath();
        }

        private void MoveChildrens(OperativeParagraphModel oa, EMoveDirection dir)
        {
            var resolution = Resolution;
            var childList = new List<OperativeParagraphModel>();
            foreach (var operativeParagraphModel in oa.GetChildren())
            {
                childList.Add(operativeParagraphModel);
            }
            foreach (var child in childList)
            {
                int index = resolution.OperativeSections.IndexOf(child);
                if (dir == EMoveDirection.Up)
                {
                    if (index - 1 > 0)
                    {
                        resolution.OperativeSections.Move(index, index - 1);
                    }
                }
                else
                {
                    if (index + 1 <= resolution.OperativeSections.Count)
                    {
                        resolution.OperativeSections.Move(index, index + 1);
                    }
                }
                    
            }

            foreach (var child in oa.GetChildren())
            {
                MoveChildrens(child, dir);
            }
            

        }

        [Obsolete("Use Resolution!")]
        public ResolutionModel GetResolution()
        {
            return Resolution;
        }

        public void MoveDown()
        {
            int index = Resolution.OperativeSections.IndexOf(this);
            if (ParentID == null)
            {
                if (index < Resolution.OperativeSections.Count - 1)
                {
                    MoveChildrens(this, EMoveDirection.Down);
                    Resolution.OperativeSections.Move(index, index + 1);
                    
                }
                    

            }
            else
            {
                if (Resolution.OperativeSections.IndexOf(
                    Resolution.OperativeSections.Last(n => n.ParentID == this.ParentID)) > index)
                {
                    MoveChildrens(this, EMoveDirection.Down);
                    Resolution.OperativeSections.Move(index, index + 1);
                    
                }
            }

            UpdatePath();
        }

        public void Remove()
        {
            Resolution?.RemoveOperativeSection(this);
        }

        public override string ToString()
        {
            return Path + " " + Text;
        }

        public void MoveRight()
        {
            int index = Resolution.OperativeSections.IndexOf(this);
            int oldIndex = index;
            if (index == 0)
                return;

            index--;
            OperativeParagraphModel above = Resolution.OperativeSections[index];

            while (above.Level > this.Level)
            {
                index--;
                above = Resolution.OperativeSections[index];
                
            }
            Console.WriteLine("Found one at index: " + index.ToString());
            this.ParentID = above.ID;

            if (oldIndex == index + 1)
            {
                //does not need to move
            } 
            else if (oldIndex != index - 1)
            {
                //Operative Section must be moved up

                Resolution.OperativeSections.Move(oldIndex, index + above.Children.Count);
            }

            UpdatePath();
        }

        public void MoveLeft()
        {
            if (this.Level == 1 || this.ParentID == null)
            { 
                this.ParentID = null;
            }
            else
            {
                int index = Resolution.OperativeSections.IndexOf(this);
                int oldIndex = index;

                //Get the paragraph that is above this one.
                index = index - 1;
                OperativeParagraphModel above = Resolution.OperativeSections[index];

                //if the paragraph is the same level or a higher level
                //the next higher paragraph must be picked!
                while (above.Level >= this.Level - 1)
                {
                    index--;
                    above = Resolution.OperativeSections[index];
                }

                this.ParentID = above.ID;
                Console.WriteLine("Founded above: " + above.ID);

                if (oldIndex != index - 1)
                {
                    //Operative Section must be moved up
                    Resolution.OperativeSections.Move(oldIndex, index);
                }
            }
            UpdatePath();
        }

        public IEnumerable<OperativeParagraphModel> GetChildren()
        {
            return Resolution.OperativeSections.Where(n => n.ParentID == this.ID);
        }

        public OperativeParagraphModel()
        {
            this.Notices = new List<NoticeModel>();
        }
    }
}
