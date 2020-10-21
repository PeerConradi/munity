using System;
using System.Collections.Generic;

namespace MUNityCore.Models.Resolution.V2
{
    public interface INoticeModel
    {
        string Id { get; set; }

        string AuthorName { get; set; }

        string AuthorId { get; set; }

        DateTime CreationDate { get; set; }

        string Title { get; set; }

        string Text { get; set; }

        List<TagModel> Tags { get; set; }

        List<string> ReadBy { get; set; }
    }
}