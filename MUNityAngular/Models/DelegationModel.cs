using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;
using System.Net;

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

        

        public string ID { get; set; }

        public string Name { get; set; }

        public string ISO { get; set; }

        public string ImageFilename { get; set; } = "";

        public Uri ImageLink { get; set; }


        public DelegationModel(string id = null)
        {
            this.ID = id ?? Guid.NewGuid().ToString();
        }

        public override string ToString()
        {
            return Name;
        }

        public bool Search(string keyword)
        {
            return Name.ToLower().Contains(keyword.ToLower());
        }

        public bool DeepSearch(string keyword)
        {
            return Name.ToLower().Contains(keyword.ToLower());
        }
    }
}
