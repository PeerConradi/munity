using MUNity.Models.Simulation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MUNityCore.Models.Simulation
{

    /// <summary>
    /// A Petition is a request inside a simulation made by any instance of a simulation user.
    /// The Petition has a specific type <see cref="Petition.PetitionType"/>.
    /// </summary>
    public class Petition
    {
        /// <summary>
        /// The Id of the Petition generated as a new Guid when the Petition object is created.
        /// </summary>
        public string PetitionId { get; set; }

        /// <summary>
        /// The state of this petition. For example is it active (the committee is currently working on this petition).
        /// </summary>
        public EPetitionStates Status { get; set; }

        /// <summary>
        /// An optional extra text for the petition. This differs from the type of petition display name.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// The timestamp when this petition was created. Is set to the current DateTime when the object is created.
        /// </summary>
        public DateTime PetitionDate { get; set; }

        /// <summary>
        /// The Type of petition. This link makes it possible to link the PetitionType that is not Inside a simulaiton.
        /// This is a possible weakness. The PetitionType will maybe switched to PetitionTypeSimulation.
        /// </summary>
        public PetitionType PetitionType { get; set; }

        /// <summary>
        /// The Slot of the simulation (Simulation User) that made this petition.
        /// </summary>
        public SimulationUser SimulationUser { get; set; }

        /// <summary>
        /// The parent agendaitem that this petition refers to.
        /// </summary>
        public AgendaItem AgendaItem { get; set; }

        public Petition()
        {
            PetitionId = Guid.NewGuid().ToString();
            PetitionDate = DateTime.Now;
        }
    }
}
