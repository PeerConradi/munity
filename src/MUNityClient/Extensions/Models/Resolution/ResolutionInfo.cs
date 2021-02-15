using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MUNityClient.Models.Resolution
{
    /// <summary>
    /// A Resolution Info is a Client Internal Model to save into the Local Storage to identify saved Resolutions.
    /// The Resolution itself is not inside the model but can be found with the name: mtr_RESOLUTIONID
    /// </summary>
    public class ResolutionInfo
    {
        public string ResolutionId { get; set; }

        public string Title { get; set; }

        public DateTime LastChangedDate { get; set; }

        public static explicit operator ResolutionInfo(MUNity.Models.Resolution.Resolution resolution)
        {
            return new ResolutionInfo()
            {
                LastChangedDate = resolution.Date,
                ResolutionId = resolution.ResolutionId,
                Title = resolution.Header.Topic
            };
        }
    }
}
