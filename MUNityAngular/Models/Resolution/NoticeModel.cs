using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MUNityAngular.Models.Resolution
{
    public class NoticeModel
    {
        /// <summary>
        /// The Id to change or delete this one later!
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// The notice does not save a Link to the user but the fore- and
        /// lastname of the Author
        /// </summary>
        public string AuthorName { get; set; }


        public string AuthorId { get; set; }

        /// <summary>
        /// The DateTime when this Notice was created
        /// </summary>
        public DateTime CreationDate { get; set; }

        /// <summary>
        /// Notices can have a title
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// The Main content. This could be in markdown
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Notices can have tags
        /// </summary>
        public List<TagModel> Tags { get; set; }

        public NoticeModel()
        {
            this.Tags = new List<TagModel>();
            this.Id = Guid.NewGuid().ToString();
        }
    }
}
