using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MUNity.Database.Models.Simulation;

public class AllChatMessage
{
    public string AllChatMessageId { get; set; }

    public int AuthorId { get; set; }

    public string AuthorName { get; set; }

    public string Text { get; set; }

    public DateTime Timestamp { get; set; }

    public AllChatMessage()
    {
        AllChatMessageId = Guid.NewGuid().ToString();
    }

    public AllChatMessage(SimulationUser user, string message)
    {
        AllChatMessageId = Guid.NewGuid().ToString();
        this.AuthorId = user.SimulationUserId;
        AuthorName = user.DisplayName;
        Text = message;
        Timestamp = DateTime.Now;
    }
}
