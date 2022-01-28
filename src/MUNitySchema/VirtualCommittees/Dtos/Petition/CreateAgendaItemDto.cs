using MUNity.Base;
using MUNity.Models.Simulation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MUNity.VirtualCommittees.Dtos
{
    /// <summary>
    /// An AgendaItem.
    /// 
    /// There are 
    /// </summary>
    public class CreateAgendaItemDto : SimulationRequest, IAgendaItem
    {

        /// <summary>
        /// The id of the Agenda Item
        /// </summary>
        public int AgendaItemId { get; set; }

        /// <summary>
        /// The display name of the Agenda item
        /// </summary>
        [Required]
        public string Name { get; set; } = "Name";

        /// <summary>
        /// An aditional text for the agenda item
        /// </summary>
        [Required]
        public string Description { get; set; } = "Beschreibung";

        /// <summary>
        /// The current State of the Agenda Item
        /// </summary>
        public EAgendaItemStatuses Status { get; set; }

        /// <summary>
        /// A Date when this Agenda Item is planned to be set.
        /// </summary>
        public DateTime? PlannedDate { get; set; }

        /// <summary>
        /// A time when this agenda Item should be done.
        /// </summary>
        public DateTime? DueDate { get; set; }

        /// <summary>
        /// A datetime when the agenda item was finished.
        /// </summary>
        public DateTime? DoneDate { get; set; }

        /// <summary>
        /// A slot when this Agenda Item should be handled.
        /// </summary>
        public int OrderIndex { get; set; }

        /// <summary>
        /// A list of petitions that are pointin at this agenda Item.
        /// </summary>
        public List<IPetition> Petitions { get; set; }
    }
}
