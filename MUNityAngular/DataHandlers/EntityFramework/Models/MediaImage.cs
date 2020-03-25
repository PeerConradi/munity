using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MUNityAngular.DataHandlers.EntityFramework.Models
{
    public class MediaImage
    {
        public string ImageId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public List<string> Tags { get; set; }

        public User Owner { get; set; }

        public MediaImage()
        {
            ImageId = Guid.NewGuid().ToString();
        }
    }
}
