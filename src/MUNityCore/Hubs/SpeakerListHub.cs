﻿using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MUNityCore.Hubs
{
    public class SpeakerListHub : Hub<MUNity.Hubs.ITypedListOfSpeakerHub>
    {

    }
}
