using MUNityAngular.DataHandlers.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace MUNityAngular.Models
{

    [DataContract]
    public class BaseCommitteeModel
    {
        [DataMember]
        [PrimaryKey]
        [DatabaseSave("id", DatabaseSaveAttribute.EFieldType.VARCHAR)]
        public string ID { get; set; }

        [DataMember]
        [DatabaseSave("name")]
        public string Name { get; set; }

        [DataMember]
        [DatabaseSave("fullname")]
        public string FullName { get; set; }

        [DataMember]
        [DatabaseSave("abbreviation")]
        public string Abbreviation { get; set; }

        [DataMember]
        [DatabaseSave("article")]
        public string Article { get; set; }
    }
}
