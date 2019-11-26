using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MUNityAngular.Models
{
    public class MoveAmendment : AbstractAmendment
    {
        public int NewPosition { get; set; }

        public override string ViewValue => "Verschieben";

        public override string DisplayName => "Verschieben";

        public override int ORDER_LEVEL => 4;

        [JsonIgnore]
        public OperativeParagraphModel NewSection { get; set; }

        public override void Activate()
        {
            TargetSection.ViewModus = OperativeParagraphModel.EViewModus.Remove;
            TargetSection.IsLocked = true;
            NewSection = TargetSection.Resolution.AddOperativeParagraph(NewPosition + 1, true);
            NewSection.Text = TargetSection.Text;
            NewSection.ViewModus = OperativeParagraphModel.EViewModus.Add;
            NewSection.IsVirtual = true;
        }

        public override void Deactivate()
        {
            TargetSection.IsLocked = false;
            TargetSection.Resolution.RemoveOperativeSection(NewSection);
            base.Deactivate();
        }

        public override void Submit()
        {
            if (NewSection != null)
            {
                NewSection.Remove();
            }
            int oldIndex = TargetSection.Resolution.OperativeSections.IndexOf(TargetSection);
            if (NewPosition >= TargetSection.Resolution.OperativeSections.Count - 1)
            {
                NewPosition = TargetSection.Resolution.OperativeSections.Count - 1;
            }
            TargetSection.Resolution.OperativeSections.Move(oldIndex, NewPosition);
            TargetSection.ViewModus = OperativeParagraphModel.EViewModus.Normal;
            TargetSection.IsLocked = false;
            TargetSection.IsVirtual = false;
            foreach(var op in TargetSection.Resolution?.OperativeSections)
            {
                op.UpdatePath();
            }
            base.Submit();
        }

        public MoveAmendment(string id = null)
        {
            this.ID = id ?? Guid.NewGuid().ToString();
        }
    }
}
