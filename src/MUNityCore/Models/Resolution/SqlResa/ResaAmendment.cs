﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MUNityCore.Models.Resolution.SqlResa
{
    public class ResaAmendment
    {
        [Key]
        public string ResaAmendmentId { get; set; }

        public ResaElement Resolution { get; set; }

        public string SubmitterName { get; set; }

        //public Simulation.SimulationUser Submitter { get; set; }

        //public List<Simulation.SimulationUser> Supporter { get; set; }

        public virtual string ResaAmendmentType { get; }

        public bool Activated { get; set; }

        public DateTime SubmitTime { get; set; }

        public ResaAmendment()
        {
            this.ResaAmendmentId = Guid.NewGuid().ToString();
        }
    }
}
