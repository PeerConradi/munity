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

        //TODO: Get the Submitter from the Delegation Service!
        public DelegationModel Submitter { get; }

        public List<DelegationModel> Supporters { get; set; }

        public ResolutionModel Resolution { get;  }

        public DateTime SubmitTime { get; set; }

        private OperativeParagraphModel _targetSection;
        public OperativeParagraphModel TargetSection
        {
            get => _targetSection;
            set
            {
                if (value == null && _targetSection != null)
                    _targetSection.Amendments.Remove(this);
                _targetSection = value;
                if (value != null)
                    if (!_targetSection.Amendments.Contains(this))
                        _targetSection.Amendments.Add(this);

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
