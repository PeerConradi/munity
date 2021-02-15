using System;
using System.Collections.Generic;
using System.Text;

namespace MUNity.Models.Simulation
{
    /// <summary>
    /// An enum for possible statuses
    /// </summary>
    public enum EAgendaItemStatuses
    {
        /// <summary>
        /// Unkown status
        /// </summary>
        Unkown,
        /// <summary>
        /// The Agenda Item is currently worked on
        /// </summary>
        Active,
        /// <summary>
        /// The agenda Item is done
        /// </summary>
        Done,
        /// <summary>
        /// The agenda item has to be done
        /// </summary>
        Todo,
        /// <summary>
        /// This agenda item has been moved to a different timeslot
        /// </summary>
        Moved,
        /// <summary>
        /// this agenda item has been deleted.
        /// </summary>
        Deleted
    }
}
