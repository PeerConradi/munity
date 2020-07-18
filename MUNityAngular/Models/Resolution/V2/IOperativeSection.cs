﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MUNityAngular.Models.Resolution.V2
{
    public interface IOperativeSection
    {
        public string OperativeSectionId { get; set; }

        List<OperativeParagraph> Paragraphs { get; set; }

        List<ChangeAmendmentModel> ChangeAmendments { get; set; }

        List<AddAmendmentModel> AddAmendments { get; set; }

        List<MoveAmendmentModel> MoveAmendments { get; set; }

        List<DeleteAmendment> DeleteAmendments { get; set; }
    }
}
