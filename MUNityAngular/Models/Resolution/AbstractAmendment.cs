using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MUNityAngular.Models.Conference;

namespace MUNityAngular.Models.Resolution
{

    /// <summary>
    /// An Amendment is a form of request to change one Operative Paragraph inside a
    /// resolution.
    /// There are different types of amendments that will work different.
    /// Every Amendment can be deleted, denied or be submitted.
    /// </summary>
    public class AbstractAmendment
    {
        public string ID { get; set; }

        public string Name { get; set; }

        public string TargetSectionID { get; set; }

        public bool Activated { get; set; }

        public string SubmitterName { get; set; }

        [JsonIgnore]
        [BsonIgnore]
        public DelegationModel Submitter { get; set; }

        [JsonIgnore]
        [BsonIgnore]
        public List<DelegationModel> Supporters { get; set; }

        [JsonIgnore]
        [BsonIgnore]
        public ResolutionModel Resolution { get; set; }

        public DateTime SubmitTime { get; set; }

        private OperativeParagraphModel _targetSection;
        [JsonIgnore]
        [BsonIgnore]
        public OperativeParagraphModel TargetSection
        {
            get
            {
                if (string.IsNullOrEmpty(TargetSectionID))
                    return null;

                if (_targetSection == null && !string.IsNullOrEmpty(TargetSectionID))
                    _targetSection = Resolution?.OperativeSections.FirstOrDefault(n => n.ID == TargetSectionID);
                return _targetSection;
            }
            set
            {
                _targetSection = value;

                //Wenn die Section gesetzt wird und keine Resolution angegeben ist
                //oder die Resolution sich von dieser hier unterscheidet, dann setze
                //Die Resolution neu
                
                if (value != null)
                {
                    TargetSectionID = value.ID;
                    if (!_targetSection.Resolution.Amendments.Contains(this))
                    {
                        _targetSection.Resolution.Amendments.Add(this);
                    }
                    
                    Resolution = _targetSection.Resolution;
                }
                else
                {
                    //Verhlaten für einen Änderungsantrag wenn der Zielabschnitt auf null
                    //Gesetzt wird ist noch nicht difiniert.
                    //Möglich wäre hier ein Löschen des Absatzes!
                    TargetSectionID = null;
                    //if (Resolution != null)
                    //{
                    //    Resolution.Amendments.Add(this);
                    //}
                }
            }
        }

        public virtual void Submit()
        {
            if (TargetSection != null)
            {
                this.Remove();
            }
        }

        public virtual void Activate()
        {
            if (TargetSection != null)
                TargetSection.ActiveAmendment = this;
            Activated = true;
        }

        public virtual void Deactivate()
        {
            if (TargetSection != null)
            {
                TargetSection.ViewModus = OperativeParagraphModel.EViewModus.Normal;
                TargetSection.ActiveAmendment = null;
            }
            Activated = false;
            
        }

        public virtual void Remove()
        {
            this.Deactivate();
            if (this.TargetSection != null)
            {
                TargetSection.Resolution?.RemoveAmendment(this);
                TargetSection.ViewModus = OperativeParagraphModel.EViewModus.Normal;
                if (TargetSection.ActiveAmendment == this)
                    TargetSection.ActiveAmendment = null;
            }
        }

        /// <summary>
        /// Die Deny Methode soll ein Ablehnen des Änderungsantrages bewirken.
        /// So können ggf. alle Änderungsanträge der selben Natur ebenfalls 
        /// verworfen werden.
        /// </summary>
        public virtual void Deny()
        {
            this.Remove();
        }

        [JsonIgnore]
        public virtual string ViewValue { get; } = "Änderungsantrag";

        [JsonIgnore]
        public virtual string DisplayName { get; } = "Änderungsantrag";

        [JsonIgnore]
        public virtual int ORDER_LEVEL { get; } = 0;

        public virtual string Type { get => "abstract"; }

        public AbstractAmendment()
        {
            SubmitTime = DateTime.Now;
        }
    }

    public static class AMENDMENT_TYPES
    {
        public static readonly string DELETE_TYPE = "delete";

        public static readonly string CHANGE_TYPE = "change";

        public static readonly string MOVE_TYPE = "move";

        public static readonly string ADD_TYPE = "add";
    }
}
