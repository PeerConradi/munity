using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MUNityAngular.Models
{
    public class AbstractAmendment
    {
        public string ID { get; set; }

        public string Name { get; set; }

        public string TargetSectionID { get; set; }

        public bool Activated { get; set; }

        public string SubmitterID { get; set; }

        [JsonIgnore]
        public DelegationModel Submitter { get => DataHandlers.Database.DelegationHandler.GetDelegation(SubmitterID); }

        public List<DelegationModel> Supporters { get; set; }

        [JsonIgnore]
        public ResolutionModel Resolution { get; set; }

        public DateTime SubmitTime { get; set; }

        private OperativeParagraphModel _targetSection;
        [JsonIgnore]
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

                if (value != null)
                {
                    TargetSectionID = value.ID;
                    _targetSection.Resolution.Amendments.Add(this);
                    Resolution = _targetSection.Resolution;
                }
                else
                {
                    TargetSectionID = null;
                    if (Resolution != null)
                    {
                        Resolution.Amendments.Add(this);
                    }
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

        [JsonIgnore]
        public virtual string ViewValue { get; } = "Änderungsantrag";

        [JsonIgnore]
        public virtual string DisplayName { get; } = "Änderungsantrag";

        [JsonIgnore]
        public virtual int ORDER_LEVEL { get; } = 0;
    }
}
