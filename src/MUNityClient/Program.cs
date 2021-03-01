using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Ganss.XSS;
using Blazored.LocalStorage;

namespace MUNityClient
{
    public class Program
    {
        public static string API_URL = "https://mun-tools.com:5000";
        //public static string API_URL = "https://localhost:44303";

        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            

            builder.RootComponents.Add<App>("app");
            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(API_URL) });
            builder.Services.AddBlazoredLocalStorage();
            builder.Services.AddScoped<Services.HttpService>();
            builder.Services.AddScoped<Services.UserService>();
            builder.Services.AddScoped<Services.IResolutionService, Services.ResolutionService>();
            builder.Services.AddScoped<Services.ListOfSpeakerService>();
            builder.Services.AddScoped<Services.SimulationService>();
            builder.Services.AddScoped<IHtmlSanitizer, HtmlSanitizer>(x =>
            {
                // Configure sanitizer rules as needed here.
                // For now, just use default rules + allow class attributes
                var sanitizer = new Ganss.XSS.HtmlSanitizer();
                sanitizer.AllowedAttributes.Add("class");
                return sanitizer;
            });

            await builder.Build().RunAsync();
        }
    }
}
