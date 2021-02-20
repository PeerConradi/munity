using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using System.Net.Http;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Http;
using Microsoft.JSInterop;
using MUNityClient;
using MUNityClient.Shared;
using MUNity.Models.ListOfSpeakers;
using MUNity.Extensions.LoSExtensions;
using System.Timers;
using System.ComponentModel.DataAnnotations;
using MUNityClient.Models.ListOfSpeaker;

namespace MUNityClient.Pages.LoS
{
    public partial class ListOfSpeakerEditor
    {
        [Parameter]
        public string Id { get; set; }
    }
}