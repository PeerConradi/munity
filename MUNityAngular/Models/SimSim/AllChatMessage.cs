using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MUNityAngular.Models.SimSim
{
    public class AllChatMessage
    {
        public string AllChatMessageId { get; set; }

        public string AuthorToken { get; set; }

        public string AuthorName { get; set; }

        public string Text { get; set; }

        public DateTime Timestamp { get; set; }

        public AllChatMessage()
        {
            AllChatMessageId = Guid.NewGuid().ToString();
        }

        public AllChatMessage(ISimSimUserFacade user, string message)
        {
            AllChatMessageId = Guid.NewGuid().ToString();
            AuthorToken = user.UserToken;
            AuthorName = user.DisplayName;
            Text = message;
            Timestamp = DateTime.Now;
        }
    }
}
