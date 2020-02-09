using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MUNityAngular.Models.Resolution
{
    public class DeleteAmendmentModel : AbstractAmendment
    {
        public override string ViewValue => Submitter?.Name;

        public override string DisplayName => "Löschen";

        public override string Type => AMENDMENT_TYPES.DELETE_TYPE;

        public override void Activate()
        {
            TargetSection.ViewModus = OperativeParagraphModel.EViewModus.Remove;
            base.Activate();
        }

        public override void Deny()
        {
            //Collection Amendments:
            var toDelete = TargetSection?.Amendments.OfType<DeleteAmendmentModel>().ToList();
            toDelete?.ForEach(n => n.Remove());
            base.Deny();
        }

        public override int ORDER_LEVEL => 1;

        public override void Submit()
        {
            List<AbstractAmendment> removeAmendmentList = new List<AbstractAmendment>();
            foreach(var am in TargetSection.Resolution.Amendments.Where(n => n.TargetSection == TargetSection))
            {
                removeAmendmentList.Add(am);
            }
            removeAmendmentList.ForEach(n => n.Remove());

            TargetSection.Resolution.RemoveOperativeSection(TargetSection);
            
        }

        public DeleteAmendmentModel(string id = null) : base()
        {
            this.ID = id ?? Guid.NewGuid().ToString();
        }
    }
}
