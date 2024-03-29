﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MUNity.Database.Models.Resolution;

public class ResaPreambleParagraph
{
    public string ResaPreambleParagraphId { get; set; }

    public string Text { get; set; } = "";

    public bool IsLocked { get; set; } = false;

    public bool IsCorrected { get; set; } = false;

    public string Comment { get; set; } = "";

    [ForeignKey(nameof(ResaElement))]
    public string ResolutionId { get; set; }

    public ResaElement ResaElement { get; set; }
    public int OrderIndex { get; set; }

    public ResaPreambleParagraph()
    {
        this.ResaPreambleParagraphId = Guid.NewGuid().ToString();
    }
}
