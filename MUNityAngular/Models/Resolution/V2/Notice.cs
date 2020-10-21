using System;
using System.Collections.Generic;

namespace MUNityCore.Models.Resolution.V2
{
    public class Notice : INoticeModel
    {
        public string Id { get; set; }
        public string AuthorName { get; set; }
        public string AuthorId { get; set; }
        public DateTime CreationDate { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public List<TagModel> Tags { get; set; }
        public List<string> ReadBy { get; set; }

        public Notice()
        {
            ReadBy = new List<string>();
            Id = Util.Tools.IdGenerator.RandomString(32);
        }
    }
}