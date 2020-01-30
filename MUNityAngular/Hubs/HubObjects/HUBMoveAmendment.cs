﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MUNityAngular.Hubs.HubObjects
{
    public class HUBMoveAmendment : HUBAbstractAmendment
    {
        public int NewPosition { get; set; }

        public string NewSectionID { get; set; }

        public HUBMoveAmendment(Models.MoveAmendment amendment) : base (amendment)
        {
            this.NewPosition = amendment.NewPosition;
            this.NewSectionID = amendment.NewSectionID;
        }
    }
}
