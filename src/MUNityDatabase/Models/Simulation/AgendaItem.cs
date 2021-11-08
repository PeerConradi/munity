using MUNity.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MUNity.Database.Models.Simulation;

/// <summary>
/// A Simulation (virtual Committee) can have multiple AgendaItems. Each AgendaItem can have multiple petitions.
/// </summary>
public class AgendaItem
{
    /// <summary>
    /// The index to identify the agenda item at the backend.
    /// </summary>
    public int AgendaItemId { get; set; }

    /// <summary>
    /// A displayname of the agenda Item.
    /// </summary>
    public string Name { get; set; }
    public string Description { get; set; }
    public EAgendaItemStatuses Status { get; set; }
    public DateTime? PlannedDate { get; set; }
    public DateTime? DueDate { get; set; }
    public DateTime? DoneDate { get; set; }
    public int OrderIndex { get; set; }
    public ICollection<Petition> Petitions { get; set; }

    public Simulation Simulation { get; set; }

    public AgendaItem()
    {
        this.Petitions = new List<Petition>();
    }
    //public AgendaItem(IAgendaItem agendaItem)
    //{
    //    this.Name = agendaItem.Name;
    //    this.Description = agendaItem.Description;
    //    this.Status = agendaItem.Status;
    //    this.PlannedDate = agendaItem.PlannedDate;
    //    this.DueDate = agendaItem.DueDate;
    //    this.DoneDate = agendaItem.DoneDate;
    //    this.OrderIndex = agendaItem.OrderIndex;
    //    this.Petitions = new List<Petition>();
    //}
}
