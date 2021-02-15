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

namespace MUNityClient.Shared.Authentication
{
    public partial class SignUpBox
    {
        public MUNity.Schema.Authentication.RegisterRequest Request
        {
            get;
            set;
        }

        = new MUNity.Schema.Authentication.RegisterRequest();
        public string RepeatPassword
        {
            get;
            set;
        }

        private enum ERegisterState
        {
            NotTried,
            InProgress,
            PasswordsDontMatch,
            Failed,
            Success
        }

        private ERegisterState RegisterState = ERegisterState.NotTried;
        private async Task Register()
        {
            this.RegisterState = ERegisterState.InProgress;
            if (this.Request.Password != RepeatPassword)
            {
                this.RegisterState = ERegisterState.PasswordsDontMatch;
                return;
            }

            var result = await this.userService.Register(Request);
            if (result.IsSuccessStatusCode)
            {
                this.RegisterState = ERegisterState.Success;
                return;
            }

            this.RegisterState = ERegisterState.Failed;
        }
    }
}