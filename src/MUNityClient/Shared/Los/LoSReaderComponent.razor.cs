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

namespace MUNityClient.Shared.Los
{
    public partial class LoSReaderComponent
    {
        [Parameter]
        public string ListOfSpeakersId { get; set; }

        private ViewModels.ListOfSpeakerViewModel ViewModel { get; set; }
    }
}