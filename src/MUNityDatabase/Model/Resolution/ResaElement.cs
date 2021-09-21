using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using MUNity.Database.Models.User;

namespace MUNity.Database.Models.Resolution
{
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

        public List<ResaPreambleParagraph> PreambleParagraphs { get; set; }

        public List<ResaOperativeParagraph> OperativeParagraphs { get; set; }

        public string SupporterNames { get; set; }

        public List<ResaAddAmendment> AddAmendments { get; set; }

        public ResolutionAuth Authorization { get; set; }

        public ResaElement()
        {
            this.ResaElementId = Guid.NewGuid().ToString();
            this.PreambleParagraphs = new List<ResaPreambleParagraph>();
            this.OperativeParagraphs = new List<ResaOperativeParagraph>();
            //this.Supporters = new List<ResaSupporter>();
            this.AddAmendments = new List<ResaAddAmendment>();
            this.Authorization = new ResolutionAuth();
        }
    }
}
