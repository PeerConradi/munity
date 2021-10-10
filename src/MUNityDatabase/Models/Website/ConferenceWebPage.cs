﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MUNity.Database.Models.Conference;
using MUNity.Database.Models.User;

namespace MUNity.Database.Models.Website
{
    public class ConferenceWebPage
    {
        public string ConferenceWebPageId { get; set; }

        public string Title { get; set; }

        public string ContentHtml { get; set; }

        public string TextRaw { get; set; }

        public string NormalizedTextRaw { get; set; }

        public MunityUser CreatedUser { get; set; }

        public Models.Conference.Conference Conference { get; set; }

        public DateTime CreationDate { get; set; }

        public DateTime LastUpdateDate { get; set; }

        public bool IsIndexPage { get; set; }

        public ConferenceWebPage()
        {
            ConferenceWebPageId = Guid.NewGuid().ToString();
        }
    }
}