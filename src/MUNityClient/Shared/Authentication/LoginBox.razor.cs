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
    public partial class LoginBox
    {
        public MUNity.Schema.Authentication.AuthenticateRequest Request
        {
            get;
            set;
        }

        = new MUNity.Schema.Authentication.AuthenticateRequest();
        private enum ELoginStates
        {
            NOT_TRIED,
            LOGGING_IN,
            FAILED,
            SUCCESS
        }

        private ELoginStates LoginState = ELoginStates.NOT_TRIED;
        private async Task Login()
        {
            this.LoginState = ELoginStates.LOGGING_IN;
            var response = await userService.Login(this.Request);
            this.LoginState = (response == null) ? ELoginStates.FAILED : ELoginStates.SUCCESS;
        }
    }
}