using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MUNityCore.Models
{
    public class DocumentTypeModel
    {
        public string ID { get; set; }

        public string Name { get; set; }

        public DocumentTypeModel(string id = null, string name = "NONAME")
        {
            ID = id ?? Guid.NewGuid().ToString();
            Name = name;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
