using MUNity.Database.Models.Simulation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MUNity.Database.Models.Conference;

public class RuleOfProcedure
{
    [Key]
    public int RuleOfProcedureId { get; set; }

    public string Title { get; set; }

    public ICollection<Conference> Conferences { get; set; }

    public ICollection<RuleOfProcedureSection> Sections { get; set; }

    public ICollection<PetitionType> PetitionTypes { get; set; }
}

public class RuleOfProcedureSection
{
    [Key]
    public int RuleOfProcedureSectionId { get; set; }

    public RuleOfProcedure RuleOfProcedure { get; set; }

    public int Index { get; set; }

    public string Name { get; set; }

    public ICollection<RuleOfProcedureParagraph> Paragraphs { get; set; }
}

public class RuleOfProcedureParagraph
{
    [Key]
    public int RuleOfProcedureParagraphId { get; set; }

    public RuleOfProcedureSection Section { get; set; }

    public int Index { get; set; }

    public string Text { get; set; }
}

public class RuleOfProcedureSubParagraph
{
    [Key]
    public int RuleOfProcedureSubParagraphId { get; set; }

    public RuleOfProcedureParagraph Parent { get; set; }

    public int Index { get; set; }

    public string Text { get; set; }
}