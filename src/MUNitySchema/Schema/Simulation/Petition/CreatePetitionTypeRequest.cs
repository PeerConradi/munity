using MUNityBase;
using System;
using System.Collections.Generic;
using System.Text;

namespace MUNity.Schema.Simulation
{
    public class CreatePetitionTypeRequest : SimulationRequest
    {
        /// <summary>
        /// A Name to Display this petition. For example: Right of information.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// More information to give about this petition type
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// A Reference to where you get more information about this petition. This could be a link or
        /// a name of the paragraph insiide the rules of procedure.
        /// </summary>
        public string Reference { get; set; }

        /// <summary>
        /// How do you get to vote on this Petitiontype
        /// </summary>
        public PetitionRulings Ruling { get; set; }

        /// <summary>
        /// A category for the petitiontype
        /// </summary>
        public string Category { get; set; }
    }
}
