using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MUNityAngular.Models.SimSim
{
    public class AllChatMessage
    {
        public int AllChatMessageId { get; set; }

        public string AuthorName { get; set; }

        public string Text { get; set; }

        public DateTime Timestamp { get; set; }

        public List<AllChatMessage> AllChat { get; set; }


    }
}
