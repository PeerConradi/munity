using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MUNityAngular.DataHandlers.EntityFramework.Models
{
    public class MediaImage
    {
        public string MediaImageId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public List<MediaTag> Tags { get; set; }

        public User Owner { get; set; }

        public MediaImage()
        {
            MediaImageId = Guid.NewGuid().ToString();
        }
    }
}
