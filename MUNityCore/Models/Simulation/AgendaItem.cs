using MUNity.Models.Simulation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MUNityCore.Models.Simulation
{
    public class AgendaItem
    {
        public int AgendaItemId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public EAgendaItemStatuses Status { get; set; }
        public DateTime? PlannedDate { get; set; }
        public DateTime? DueDate { get; set; }
        public DateTime? DoneDate { get; set; }
        public int OrderIndex { get; set; }
        public List<Petition> Petitions { get; set; }

        public Simulation Simulation { get; set; }

        public AgendaItem()
        {
            this.Petitions = new List<Petition>();
        }
        public AgendaItem(IAgendaItem agendaItem)
        {
            this.Name = agendaItem.Name;
            this.Description = agendaItem.Description;
            this.Status = agendaItem.Status;
            this.PlannedDate = agendaItem.PlannedDate;
            this.DueDate = agendaItem.DueDate;
            this.DoneDate = agendaItem.DoneDate;
            this.OrderIndex = agendaItem.OrderIndex;
            this.Petitions = new List<Petition>();
        }
    }
}
