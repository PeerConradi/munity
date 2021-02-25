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
using System.Net.Http.Json;

namespace MUNityClient.Pages.LoS
{
    public partial class ListOfSpeakerHome
    {
        private async Task CreateListOfSpeakers()
        {
            var list = await this.listOfSpeakerService.CreateListOfSpeakers();
            if (list != null)
            {
                navigationManager.NavigateTo($"/los/edit/{list.ListOfSpeakersId}");
            }
        }

        private async Task CreateListOnline()
        {
            var created = await this.listOfSpeakerService.CreateOnline();
            if (created.IsSuccessStatusCode)
            {
                var result = await created.Content.ReadFromJsonAsync<MUNity.Schema.ListOfSpeakers.CreatedResponse>();
                if (result != null)
                {
                    navigationManager.NavigateTo($"/los/edit/{result.ListOfSpeakersId}");
                }
                else
                {
                    // TODO: Write an error message, list was created but something went wrong with the result
                }
            }
            else
            {
                // TODO: Write an error message: List was not created.
            }
        }
    }
}