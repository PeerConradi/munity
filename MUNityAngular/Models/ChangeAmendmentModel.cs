using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MUNityAngular.Models
{

    /// <summary>
    /// An Amendment to Change a sections text.
    /// When accepted the NewText will be applied to the Operative Section.
    /// </summary>
    public class ChangeAmendmentModel : AbstractAmendment
    {
        public string NewText { get; set; }

        public ChangeAmendmentModel(string id = null)
        {
            this.ID = id ?? Guid.NewGuid().ToString();
        }

        public override void Submit()
        {
            //Apply the new Text
            if (TargetSection != null)
                TargetSection.Text = NewText;

            TargetSection.ViewModus = OperativeParagraphModel.EViewModus.Normal;
            

            this.Remove();
        }

        public override void Activate()
        {
            TargetSection.ViewModus = OperativeParagraphModel.EViewModus.Overwrite;
            base.Activate();
        }

        public override string ViewValue
        {
            get
            {
                string text;
                text = NewText;
                text += "\n";
                if (Submitter != null)
                {
                    text += Submitter.Name;
                    text += "\n";
                }
                    

                return text;
            }
        }

        public override int ORDER_LEVEL => 2;

        public override string DisplayName => "Bearbeiten";
    }
}
