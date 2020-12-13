using MUNityCore.Models.Resolution.V2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MUNityCore.Extensions.ResolutionExtensions
{
    public static class OperativeSectionExtensions
    {
        public static OperativeParagraph FirstOrDefault(this OperativeSection operativeSection, Func<OperativeParagraph, bool> predicate)
        {
            var result = operativeSection.Paragraphs.FirstOrDefault(predicate);
            if (result != null) return result;
            foreach (var s in operativeSection.Paragraphs)
            {
                result = deepFirstOrDefault(s, predicate);
                if (result != null) return result;
            }
            return null;
        }

        private static OperativeParagraph deepFirstOrDefault(this OperativeParagraph paragraph, Func<OperativeParagraph, bool> predicate)
        {
            var result = paragraph.Children.FirstOrDefault(predicate);
            if (result != null) return result;
            foreach (var child in paragraph.Children)
            {
                return deepFirstOrDefault(child, predicate);
            }
            return null;
        }
    }
}
