using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MUNityAngular.Models
{
    public class SubjectModel
    {

        public string ID { get; set; }

        public string Name { get; set; }

        public SubjectModel(string id = null)
        {
            this.ID = id ?? Guid.NewGuid().ToString();
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
