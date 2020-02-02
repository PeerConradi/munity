using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MUNityAngular.Hubs
{
    public class ResolutionHub : Hub<ITypedResolutionHub>
    {

        //Bekannte Schwachstelle: an dieser Stelle muss noch einmal überprüft werden, ob ein Benutzer überhaupt das Recht hat
        //sich mit einer Resolution zu verinden, es muss also neben der id auch noch der auth mitgegeben werden und dann
        //im ResolutionService geschaut werden, ob die Verbindung überhaupt erlaubt ist.
        public Task SubscribeToResolution(string id)
        {
            return Groups.AddToGroupAsync(Context.ConnectionId, id);
        }

        public Task WhoAmI()
        {
            return Clients.User(Context.ConnectionId).HubContextIdChanged(Context.ConnectionId);
        }
        
    }
}
