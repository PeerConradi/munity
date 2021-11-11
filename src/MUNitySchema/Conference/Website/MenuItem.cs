using System;
using System.Collections.Generic;
using System.Text;

namespace MUNity.Schema.Conference.Website
{
    public class MenuItem
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string PageId { get; set; }

        public List<MenuItem> Items { get; set; }
    }
}
