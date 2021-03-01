using System;

namespace MUNityCore.Models.Resolution.SqlResa
{
    public class ResaSupporter
    {
        public string ResaSupporterId { get; set; }

        public string Name { get; set; }

        public ResaElement Resolution { get; set; }

        public ResaSupporter()
        {
            this.ResaSupporterId = Guid.NewGuid().ToString();
        }
    }
}