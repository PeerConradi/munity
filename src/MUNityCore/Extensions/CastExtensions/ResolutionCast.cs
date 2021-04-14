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
    }
}
