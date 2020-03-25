using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MUNityAngular.DataHandlers.EntityFramework.Models
{
    public class TeamRole
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public TeamRole ParentRole { get; set; }

        public int MinCount { get; set; }

        public int MaxCount { get; set; }

        public Conference Conference { get; set; }
    }
}
