﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MUNityAngular.DataHandlers.EntityFramework.Models
{
    public class Admin
    {
        public int AdminId { get; set; }

        public User User { get; set; }

        public int PowerRank { get; set; }
    }
}