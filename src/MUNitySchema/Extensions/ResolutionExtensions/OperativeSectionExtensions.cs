using MUNity.Models.Resolution;
using System;
using System.Collections.Generic;
using System.Text;

namespace MUNity.Extensions.ResolutionExtensions
{
    public static class OperativeSectionExtensions
    {
        public static int AmendmentCount(this OperativeSection section)
        {
            return section.AddAmendments.Count + section.ChangeAmendments.Count + section.DeleteAmendments.Count + section.MoveAmendments.Count;
        }

        public static IEnumerable<AbstractAmendment> AmendmentsOrdered (this OperativeSection section)
        {
            return section.GetOrderedAmendments();
        }
    }
}
