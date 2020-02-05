using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MUNityAngular.DataHandlers.Database;

namespace MUNityAngular.Models
{
    public class ResolutionInformationModel
    {

        [DatabaseSave("id")]
        public string ID { get; set; }

        [DatabaseSave("name")]
        public string Name { get; set; }

    }
}
