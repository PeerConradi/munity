using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Net;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.IO;
using System;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MUNityCore.DataHandlers;
using MUNityCore.DataHandlers.EntityFramework;
using MUNityCore.Models;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Swashbuckle.AspNetCore.SwaggerGen;
using Blazorise.Extensions;
using Blazorise;
using Blazorise.Bootstrap;
using Blazorise.Icons.FontAwesome;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server;
using Blazored.LocalStorage;

namespace MUNityCore
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();


            // Adding a Blazor FrontEnd for configuration and stuff
            services.AddRazorPages();
            services.AddServerSideBlazor();
            services.AddBlazorise(options =>
            {
                options.ChangeTextOnKeyPress = true; // optional
            }).AddBootstrapProviders()
              .AddFontAwesomeIcons();

            // The App Settings contains information like the secret used to 
            var appSettingsSection = Configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingsSection);

            // JWT Authentication is done here!
            var appSettings = appSettingsSection.Get<AppSettings>();
            var key = Encoding.UTF8.GetBytes(appSettings.Secret);
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

            services.AddIdentity<MUNityCore.Models.User.MunityUser, MUNityCore.Models.User.UserRole>()
                .AddEntityFrameworkStores<MunityContext>()
                .AddDefaultUI()
                .AddDefaultTokenProviders();

            // TODO: Add OAuth some day!

            // SignalR is for the WebSockets, they are mainly used in the live Editors for example
            // the Resa Editors etc.
            services.AddSignalR(options =>
            {
                options.MaximumReceiveMessageSize = 68000;
                options.ClientTimeoutInterval = new System.TimeSpan(2,0,0);
                options.KeepAliveInterval = new TimeSpan(0, 0, 30);
            });

            
            var mySqlConnectionString = Configuration.GetValue<string>("MySqlSettings:ConnectionString");

            var version = new Version(10, 1, 26);
            var serverVersion = new MariaDbServerVersion(version);

            services.AddDbContext<MunityContext>(options =>
            {
                options.UseMySql(mySqlConnectionString, serverVersion, builder =>
                {
                    builder.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null);
                });
            });
            
            // All services that are used inside the controllers.
            //services.AddScoped<Services.InstallationService>();
            services.AddBlazoredLocalStorage();
            services.AddScoped<Services.IAuthService, Services.AuthService>();
            services.AddScoped<Services.IUserService, Services.UserService>();
            services.AddScoped<Services.IOrganisationService, Services.OrganisationService>();
            services.AddScoped<Services.IConferenceService, Services.ConferenceService>();
            services.AddScoped<Services.SqlResolutionService>();
            services.AddScoped<Services.SpeakerlistService>();
            services.AddScoped<Services.SimulationService>();
            services.AddScoped<AuthenticationStateProvider, ServerAuthenticationStateProvider>();
            services.AddScoped<Services.FrontendSimulationService>();

            // Swagger for Documentation
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "MUNity-API",
                    Description = "The API Endpoints for MUNity.",
                    Version = "v0.5"
                });

                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);

                c.CustomOperationIds(apiDescription =>
                {
                    return apiDescription.TryGetMethodInfo(out MethodInfo method) ? method.Name : null;
                });
            }).AddSwaggerGenNewtonsoftSupport();


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                //app.UseCors("DevPolicy");
                app.UseDeveloperExceptionPage();
                // Enable middleware to serve generated Swagger as a JSON endpoint.
            }
            else
            {
                Program.MasterToken = Environment.GetEnvironmentVariable("MUNITY_ADMIN_PASS");
                //app.UseCors("ProdAllOrigins");
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            // app.UseSwaggerUI(c =>
            // {
            //     c.SwaggerEndpoint("/swagger/v1/swagger.json", "Munity API V1");
            //     c.DisplayOperationId();
            // });

            // app.UseSwagger();

            //opt.WithOrigins("https://www.mun-hosting.web.app", "mun-hosting.web.app", "https://mun-hosting.web.app", "http://localhost", "https://localhost", "localhost", "127.0.0.1", "*.localhost")
            app.UseCors(opt =>
            {
                opt.SetIsOriginAllowed(origin => true)
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials();
            });

            

            //app.UseForwardedHeaders(new ForwardedHeadersOptions
            //{
            //    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            //});

           app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=index}/{id?}");
                endpoints.MapHub<Hubs.ResolutionHub>("/resasocket");
                endpoints.MapHub<Hubs.SpeakerListHub>("/slsocket");
                endpoints.MapHub<Hubs.SimulationHub>("/simsocket");

                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });

            try
            {
                UpdateDatabase(app);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            
        }

        private static void UpdateDatabase(IApplicationBuilder app)
        {
            using var serviceScope = app.ApplicationServices
                .GetRequiredService<IServiceScopeFactory>()
                .CreateScope();
            using var context = serviceScope.ServiceProvider.GetService<MunityContext>();
            context.Database.Migrate();
        }
    }
}
