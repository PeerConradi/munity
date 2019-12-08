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
            COUNTRY,
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

        [DatabaseSave("abbreviation")]
        public string Abbreviation { get; set; }

        [DatabaseSave("type")]
        public string TypeName { get; set; }

        [DatabaseSave("countryid")]
        public string CountryId { get; set; }

        public string ISO { get; set; }

        public DelegationModel(string id = null)
        {
            this.ID = id ?? Guid.NewGuid().ToString();
        }

        public DelegationModel()
        {
            this.ID = Guid.NewGuid().ToString();
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
