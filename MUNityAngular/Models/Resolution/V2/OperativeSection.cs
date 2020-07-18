using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MUNityAngular.Models.Resolution.V2
{
    public class OperativeSection : IOperativeSection
    {
        public string OperativeSectionId { get; set; }
        public List<OperativeParagraph> Paragraphs { get; set; }
        public List<ChangeAmendmentModel> ChangeAmendments { get; set; }
        public List<AddAmendmentModel> AddAmendments { get; set; }
        public List<MoveAmendmentModel> MoveAmendments { get; set; }
        public List<DeleteAmendment> DeleteAmendments { get; set; }

        public OperativeSection()
        {
            OperativeSectionId = Util.Tools.IdGenerator.RandomString(36);
            Paragraphs = new List<OperativeParagraph>();
            ChangeAmendments = new List<ChangeAmendmentModel>();
            AddAmendments = new List<AddAmendmentModel>();
            MoveAmendments = new List<MoveAmendmentModel>();
            DeleteAmendments = new List<DeleteAmendment>();
        }
    }
}
