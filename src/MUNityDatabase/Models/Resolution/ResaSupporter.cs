using System;
using MUNity.Database.Models.Conference;

namespace MUNity.Database.Models.Resolution
{
    public class ResaSupporter
    {
        public string ResaSupporterId { get; set; }

        public string Name { get; set; }

        public Participation Participation { get; set; }

        public ResaElement Resolution { get; set; }

        public ResaSupporter()
        {
            this.ResaSupporterId = Guid.NewGuid().ToString();
        }
    }
}