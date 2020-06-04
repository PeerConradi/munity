using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MUNityAngular.Models.Conference;

namespace MUNityAngular.DataHandlers.EntityFramework.Models
{
    public class Gallery
    {
        public int GalleryId { get; set; }

        public string Name { get; set; }

        public Conference Conference { get; set; }

        public List<MediaImage> Images { get; set; }
    }
}
