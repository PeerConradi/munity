using MUNity.Models.Resolution;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace MUNity.Extensions.ResolutionExtensions
{
    public static class OperativeSectionExtensions
    {
        public static int AmendmentCount(this OperativeSection section)
        {
            return section.AddAmendments.Count + section.ChangeAmendments.Count + section.DeleteAmendments.Count + section.MoveAmendments.Count;
        }

        /// <summary>
        /// Only works on one level
        /// </summary>
        /// <param name="section"></param>
        /// <param name="paragraph"></param>
        /// <returns></returns>
        public static int GetIndexOfParagraphNonVirtual(this OperativeSection section, OperativeParagraph paragraph)
        {
            int index = 0;
            foreach(var p in section.Paragraphs)
            {
                if (!p.IsVirtual)
                {
                    if (p == paragraph ||p.OperativeParagraphId == paragraph.OperativeParagraphId)
                    {
                        break;
                    }
                    index++;
                }
            }
            return index;
        }

        public static int GetIndexOfParagraph(this OperativeSection section, OperativeParagraph paragraph)
        {
            return section.Paragraphs.IndexOf(paragraph);
        }

        public static IEnumerable<AbstractAmendment> AmendmentsOrdered (this OperativeSection section)
        {
            return section.GetOrderedAmendments();
        }
    }
}
