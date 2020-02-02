using MongoDB.Bson.Serialization.Attributes;
using MUNityAngular.Util.Exceptions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MUNityAngular.Models
{
    public class AddAmendmentModel : AbstractAmendment
    {
        public int TargetPosition { get; set; }

        public string NewText { get; set; }

        public string TargetResolutionID { get; set; }

        public override string Type => AMENDMENT_TYPES.ADD_TYPE;

        private ResolutionModel _targetResolution;
        [JsonIgnore]
        [BsonIgnore]
        public ResolutionModel TargetResolution { get => _targetResolution;
            set
            {
                _targetResolution = value;
                if (value != null)
                {
                    if (value.Amendments.Any(n => n.ID == this.ID) == false)
                    {
                        value.Amendments.Add(this);
                    }
                }
                else
                {
                    _targetResolution?.Amendments.Remove(this);
                }
            }
        }

        [JsonIgnore]
        [BsonIgnore]
        public override string ViewValue => "Hinzufügen";

        [JsonIgnore]
        [BsonIgnore]
        public override string DisplayName => "Absatz hinzufügen";

        public override void Activate()
        {
            if (TargetResolution != null)
            {
                //Wenn noch keine Darstellung für diesen Änderungsantrag exisitert lege diese an
                if (TargetSection == null)
                {
                    TargetSection = TargetResolution.AddOperativeParagraph(TargetPosition, true);
                }
                
                if (!TargetResolution.OperativeSections.Contains(TargetSection))
                {
                    TargetResolution.OperativeSections.Insert(TargetPosition, TargetSection);
                }

                TargetSection.AmendmentParagraph = true;
                TargetSection.Text = NewText;
                TargetSection.IsVirtual = true;
                TargetSection.ViewModus = OperativeParagraphModel.EViewModus.Add;
            }
            else
            {
                throw new ResolutionNotFoundException("Can not activate this AddAmendment because the Resolution Document was not found");
            }
        }

        public override void Remove()
        {
            this.Deactivate();
            TargetResolution?.RemoveAmendment(this);
            base.Remove();
        }

        public override void Deactivate()
        {
            if (TargetResolution != null)
            {
                //Beim deaktivieren dieses Änderungsantrages muss der
                //Hierfür verwendete Operative Absatz wieder aus der Resoltion entfernt werden
                if (TargetSection != null)
                {
                    //Setze den Text auf die letzte Eingabe
                    NewText = TargetSection.Text;
                    TargetResolution.OperativeSections.Remove(TargetSection);
                    //TargetResolution.RemoveOperativeSection(TargetSection);
                    //TargetSection = null;
                }
            }
            Activated = false;

            //base.Deactivate();
        }

        public override void Submit()
        {
            if (TargetSection == null)
            {
                //Add a Target Section if its not existend yet
                TargetSection = TargetResolution.AddOperativeParagraph(TargetPosition, false);
                if (TargetSection != null)
                {
                    TargetSection.Text = NewText;
                    TargetSection.IsVirtual = false;
                    TargetSection.ViewModus = OperativeParagraphModel.EViewModus.Normal;
                }
            }
            else
            {
                TargetSection.AmendmentParagraph = false;
                TargetSection.IsVirtual = false;
                TargetSection.ViewModus = OperativeParagraphModel.EViewModus.Normal;
            }
            TargetResolution.Amendments.Remove(this);
        }

        public AddAmendmentModel(string id = null)
        {
            this.ID = id ?? Guid.NewGuid().ToString();
        }
    }
}
