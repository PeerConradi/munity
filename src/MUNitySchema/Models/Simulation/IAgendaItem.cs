using MUNity.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace MUNity.Models.Simulation
{
    /// <summary>
    /// An interface that describes the AgendaItem of a simulation.
    /// </summary>
    public interface IAgendaItem
    {
        /// <summary>
        /// The id of the Agenda Item
        /// </summary>
        int AgendaItemId { get; set; }

        /// <summary>
        /// The Target Simulation
        /// </summary>
        int SimulationId { get; set; }

        /// <summary>
        /// The display name of the Agenda item
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// An aditional text for the agenda item
        /// </summary>
        string Description { get; set; }

        /// <summary>
        /// The current State of the Agenda Item
        /// </summary>
        EAgendaItemStatuses Status { get; set; }

        /// <summary>
        /// A Date when this Agenda Item is planned to be set.
        /// </summary>
        DateTime? PlannedDate { get; set; }

        /// <summary>
        /// A time when this agenda Item should be done.
        /// </summary>
        DateTime? DueDate { get; set; }

        /// <summary>
        /// A datetime when the agenda item was finished.
        /// </summary>
        DateTime? DoneDate { get; set; }

        /// <summary>
        /// A slot when this Agenda Item should be handled.
        /// </summary>
        int OrderIndex { get; set; }

        /// <summary>
        /// A list of petitions that are pointin at this agenda Item.
        /// </summary>
        List<IPetition> Petitions { get; set; }
    }
}
