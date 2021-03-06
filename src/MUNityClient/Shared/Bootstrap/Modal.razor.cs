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

namespace MUNityClient.Shared.Bootstrap
{
    public partial class Modal
    {
        [Parameter]
        public string Title { get; set; }

        private bool _isOpen;

        public bool IsOpen => _isOpen;

        [Parameter]
        public RenderFragment ChildContent
        {
            get;
            set;
        }

        [Parameter]
        public EventCallback<MouseEventArgs> OnSubmitCallback
        {
            get;
            set;
        }

        [Parameter]
        public string SubmitText { get; set; } = "Bestštigen";

        [Parameter]
        public bool ShowSubmitButton { get; set; } = true;

        public Guid Guid = Guid.NewGuid();
        public string ModalDisplay = "none;";
        public string ModalClass = "";
        public bool ShowBackdrop = false;
        public void Open()
        {
            ModalDisplay = "block;";
            ModalClass = "Show";
            ShowBackdrop = true;
            _isOpen = true;
            StateHasChanged();
        }

        public void Close()
        {
            ModalDisplay = "none";
            ModalClass = "";
            ShowBackdrop = false;
            _isOpen = false;
            StateHasChanged();
        }
    }
}