using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MUNityCore.Models.ListOfSpeakers
{
    public class Speaker
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Iso { get; set; }

        public Speaker()
        {
            this.Id = Guid.NewGuid().ToString();
        }
    }
}
