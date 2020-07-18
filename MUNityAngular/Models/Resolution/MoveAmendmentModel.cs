using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MUNityAngular.Util.Extenstions;
using MongoDB.Bson.Serialization.Attributes;

namespace MUNityAngular.Models.Resolution
{

    
    public class MoveAmendmentModel : AbstractAmendment
    {
        public int NewPosition { get; set; }

        public override string ViewValue => "Verschieben";

        public override string DisplayName => "Verschieben";

        public override int ORDER_LEVEL => 4;

        public override string Type => AMENDMENT_TYPES.MOVE_TYPE;

        public string NewSectionID { get; set; }

        [JsonIgnore]
        [BsonIgnore]
        public OperativeParagraphModel NewSection { get; set; }

        public override void Activate()
        {
            NewSection = TargetSection.Resolution.OperativeSections.FirstOrDefault(n => n.ID == NewSectionID);

            TargetSection.ViewModus = OperativeParagraphModel.EViewModus.Remove;
            TargetSection.IsLocked = true;

            if (NewSection == null)
            {
                NewSection = TargetSection.Resolution.AddOperativeParagraph(NewPosition, true);
            }
            
            NewSection.Text = TargetSection.Text;
            NewSection.ViewModus = OperativeParagraphModel.EViewModus.Add;
            NewSection.IsVirtual = true;
            NewSectionID = NewSection.ID;
            base.Activate();
        }

        public override void Deactivate()
        {
            TargetSection.IsLocked = false;
            if (NewSection == null)
            {
                NewSection = TargetSection.Resolution.OperativeSections.FirstOrDefault(n => n.ID == NewSectionID);
            }
            //TargetSection.Resolution.OperativeSections.Remove(TargetSection);
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

        public MoveAmendmentModel(string id = null)
        {
            this.ID = id ?? Guid.NewGuid().ToString();
        }
    }
}
