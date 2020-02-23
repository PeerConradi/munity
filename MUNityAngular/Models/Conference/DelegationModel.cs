using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;
using System.Net;
using MUNityAngular.DataHandlers.Database;


namespace MUNityAngular.Models.Conference
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

        private EDelegationTypes _type = EDelegationTypes.COUNTRY;

        [PrimaryKey]
        [DatabaseSave("id", DatabaseSaveAttribute.EFieldType.VARCHAR)]
        public string ID { get; set; }

        [DatabaseSave("name")]
        public string Name { get; set; }

        [DatabaseSave("fullname")]
        public string FullName { get; set; }

        [DatabaseSave("abbreviation")]
        public string Abbreviation { get; set; }

        [DatabaseSave("type")]
        public string TypeName { get; set; }

        [DatabaseSave("countryid")]
        public string CountryId { get; set; }

        public string ISO { get; set; }

        [DatabaseSave("iconname")]
        public string IconName { get; set; }

        public EDelegationTypes SetTypeByString(string s)
        {
            s = s.ToUpper();
            if (s == "COUNTRY") this._type = EDelegationTypes.COUNTRY;
            else if (s == "NGO") this._type = EDelegationTypes.NGO;
            else if (s == "MEDIA") this._type = EDelegationTypes.MEDIA;
            else if (s == "PERSON") this._type = EDelegationTypes.PERSON;
            else if (s == "TEAM") this._type = EDelegationTypes.TEAM;

            return this._type;
        }

        public string GetTypeAsString()
        {
            switch (this._type)
            {
                case EDelegationTypes.COUNTRY:
                    return "COUNTRY";
                case EDelegationTypes.NGO:
                    return "NGO";
                case EDelegationTypes.PERSON:
                    return "PERSON";
                case EDelegationTypes.TEAM:
                    return "TEAM";
                case EDelegationTypes.MEDIA:
                    return "MEDIA";
                default:
                    return "DELEGATION";
            }
        }

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
