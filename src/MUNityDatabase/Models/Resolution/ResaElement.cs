using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using MUNity.Database.Models.User;

namespace MUNity.Database.Models.Resolution;

public class ResaElement
{

    [Key]
    public string ResaElementId { get; set; }

    public string Topic { get; set; } = "";

    public string Name { get; set; } = "";

    public string FullName { get; set; } = "";

    public string AgendaItem { get; set; } = "";

    public string Session { get; set; } = "";

    public string SubmitterName { get; set; } = "";

    public string CommitteeName { get; set; } = "";

    public DateTime CreatedDate { get; set; } = DateTime.Now;

    public IList<ResaPreambleParagraph> PreambleParagraphs { get; set; }

    public IList<ResaOperativeParagraph> OperativeParagraphs { get; set; }

    public string SupporterNames { get; set; }

    public IList<ResaAddAmendment> AddAmendments { get; set; }

    public IList<ResolutionAuth> Authorizations { get; set; }

    public IList<ResaSupporter> Supporters { get; set; }

    public ResaElement()
    {
        this.ResaElementId = Guid.NewGuid().ToString();
        this.PreambleParagraphs = new List<ResaPreambleParagraph>();
        this.OperativeParagraphs = new List<ResaOperativeParagraph>();
        this.Supporters = new List<ResaSupporter>();
        this.AddAmendments = new List<ResaAddAmendment>();
        this.Authorizations = new List<ResolutionAuth>();
    }
}
