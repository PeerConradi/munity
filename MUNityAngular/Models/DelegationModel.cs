using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;
using System.Net;
using MUNityAngular.DataHandlers.Database;


namespace MUNityAngular.Models
{

    public class DelegationModel
    {
        public enum EDelegationTypes
        {
            STATE,
            NGO,
            PERSON,
            TEAM,
            MEDIA
        }

        [PrimaryKey]
        [DatabaseSave("id", DatabaseSaveAttribute.EFieldType.VARCHAR)]
        public string ID { get; set; }

        [DatabaseSave("name")]
        public string Name { get; set; }

        [DatabaseSave("iso")]
        public string ISO { get; set; }

        public DelegationModel(string id = null)
        {
            this.ID = id ?? Guid.NewGuid().ToString();
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
