using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MUNityClient.Models.ListOfSpeaker
{
    public class SpeakerToAdd
    {
        public string Iso
        {
            get;
            set;
        }

        [Required]
        [MinLength(2)]
        public string Name
        {
            get;
            set;
        }
    }
}
