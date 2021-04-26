using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MUNityCore.Extensions.CastExtensions
{
    public static class ResolutionCast
    {
        public static MUNity.Models.Resolution.PreambleParagraph ToModel(this MUNityCore.Models.Resolution.SqlResa.ResaPreambleParagraph sourceParagraph)
        {

            MUNity.Models.Resolution.PreambleParagraph model = new MUNity.Models.Resolution.PreambleParagraph()
            {
                Comment = sourceParagraph.Comment,
                Corrected = sourceParagraph.IsCorrected,
                IsLocked = sourceParagraph.IsLocked,
                PreambleParagraphId = sourceParagraph.ResaPreambleParagraphId,
                Text = sourceParagraph.Text
            };
            return model;
        }

        public static MUNity.Models.Resolution.OperativeParagraph ToModel(this MUNityCore.Models.Resolution.SqlResa.ResaOperativeParagraph sourceParagraph)
        {
            if (sourceParagraph.Children == null)
                sourceParagraph.Children = new List<Models.Resolution.SqlResa.ResaOperativeParagraph>();
            var model = new MUNity.Models.Resolution.OperativeParagraph()
            {
                Children = sourceParagraph.Children.Select(n => n.ToModel()).ToList(),
                Comment = sourceParagraph.Comment,
                Corrected = sourceParagraph.Corrected,
                IsLocked = sourceParagraph.IsLocked,
                IsVirtual = sourceParagraph.IsVirtual,
                Name = sourceParagraph.Name,
                OperativeParagraphId = sourceParagraph.ResaOperativeParagraphId,
                Text = sourceParagraph.Text,
                Visible = sourceParagraph.Visible
            };
            return model;
        }

        public static MUNity.Models.Resolution.AddAmendment ToModel(this MUNityCore.Models.Resolution.SqlResa.ResaAddAmendment sourceAmendment)
        {
            var model = new MUNity.Models.Resolution.AddAmendment()
            {
                Activated = sourceAmendment.Activated,
                Id = sourceAmendment.ResaAmendmentId,
                Name = sourceAmendment.GetType().Name,
                SubmitterName = sourceAmendment.SubmitterName,
                SubmitTime = sourceAmendment.SubmitTime,
                TargetSectionId = sourceAmendment.VirtualParagraph.ResaOperativeParagraphId,
                Type = "add"
            };
            return model;
        }

        public static MUNity.Models.Resolution.ChangeAmendment ToModel(this MUNityCore.Models.Resolution.SqlResa.ResaChangeAmendment changeAmendment)
        {
            var model = new MUNity.Models.Resolution.ChangeAmendment()
            {
                Activated = changeAmendment.Activated,
                Id = changeAmendment.ResaAmendmentId,
                Name = changeAmendment.GetType().Name,
                NewText = changeAmendment.NewText,
                SubmitterName = changeAmendment.SubmitterName,
                SubmitTime = changeAmendment.SubmitTime,
                TargetSectionId = changeAmendment.TargetParagraph.ResaOperativeParagraphId,
                Type = "change"
            };
            return model;
        }

        public static MUNity.Models.Resolution.DeleteAmendment ToModel(this MUNityCore.Models.Resolution.SqlResa.ResaDeleteAmendment sourceAmendment)
        {
            var model = new MUNity.Models.Resolution.DeleteAmendment()
            {
                Activated = sourceAmendment.Activated,
                Id = sourceAmendment.ResaAmendmentId,
                Name = sourceAmendment.GetType().Name,
                SubmitterName = sourceAmendment.SubmitterName,
                SubmitTime = sourceAmendment.SubmitTime,
                TargetSectionId = sourceAmendment.TargetParagraph.ResaOperativeParagraphId,
                Type = "delete"
            };
            return model;
        }

        public static MUNity.Models.Resolution.MoveAmendment ToModel(this MUNityCore.Models.Resolution.SqlResa.ResaMoveAmendment sourceAmendment)
        {
            var model = new MUNity.Models.Resolution.MoveAmendment()
            {
                Activated = sourceAmendment.Activated,
                Id = sourceAmendment.ResaAmendmentId,
                Name = sourceAmendment.GetType().Name,
                NewTargetSectionId = sourceAmendment.VirtualParagraph.ResaOperativeParagraphId,
                SubmitterName = sourceAmendment.SubmitterName,
                SubmitTime = sourceAmendment.SubmitTime,
                TargetSectionId = sourceAmendment.SourceParagraph.ResaOperativeParagraphId,
                Type = sourceAmendment.ResaAmendmentType
            };
            return model;
        }
    }
}
