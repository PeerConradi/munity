using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MUNityAngular.DataHandlers.Database;
using System.Net;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.IO;
using System;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using MongoDB.Driver;
using MUNityAngular.DataHandlers;
using Microsoft.Extensions.Options;
using MUNityAngular.DataHandlers.EntityFramework;
using Microsoft.AspNetCore.SpaServices;
using Microsoft.IdentityModel.Tokens;
using MUNityAngular.Models;

namespace MUNityAngular
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
            services.AddControllersWithViews();
            // In production, the Angular files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/dist";
            });

            services.AddCors(o => o.AddPolicy("CorsPolicy", builder => {
                builder
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials();
                //.AllowAnyOrigin();
            }));

            services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.KnownProxies.Add(IPAddress.Parse("10.0.0.100"));
            });

            // The App Settings contains information like the secret used to 
            // CreatePublic the baerer authentication.
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

            // SignalR is for the WebSockets, they are mainly used in the live Editors for example
            // the Resa Editors etc.
            services.AddSignalR(options =>
            {
                options.MaximumReceiveMessageSize = 68000;
                options.ClientTimeoutInterval = new System.TimeSpan(2,0,0);
            }).AddJsonProtocol(options => {
                //this should also be removed later!
                options.PayloadSerializerOptions.PropertyNamingPolicy = null;
            });

            var mySqlConnectionString = Configuration.GetValue<string>("MySqlSettings:ConnectionString");
            var coreConnectionString = Configuration.GetValue<string>("CoreDatabase:ConnectionString");

            // Add Entity Framework MariaDB Database
            services.AddDbContextPool<MunityContext>(options =>
                options.UseMySql(mySqlConnectionString, mySqlOptions => {
                    mySqlOptions.ServerVersion(new Version(10, 1, 26), ServerType.MariaDb);
                    mySqlOptions.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null);
                }));

            // CreatePublic the core Context
            services.AddDbContextPool<MunCoreContext>(options =>
                options.UseMySql(coreConnectionString, mySqlOptions => {
                    mySqlOptions.ServerVersion(new Version(10, 1, 26), ServerType.MariaDb);
                    mySqlOptions.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null);
                }));

            // Add MongoDb Database
            services.Configure<MunityMongoDatabaseSettings>(Configuration.GetSection(nameof(MunityMongoDatabaseSettings)));
            services.AddSingleton<IMunityMongoDatabaseSettings>(sp =>
                sp.GetRequiredService<IOptions<MunityMongoDatabaseSettings>>().Value);

            // Init the Cache
            services.AddSingleton<Services.CacheService>();
            
            // All services that are used inside the controllers.
            //services.AddScoped<Services.InstallationService>();
            services.AddScoped<Services.IAuthService, Services.AuthService>();
            services.AddScoped<Services.IUserService, Services.UserService>();
            services.AddScoped<Services.IOrganisationService, Services.OrganisationService>();
            services.AddScoped<Services.ResolutionService>();
            services.AddScoped<Services.PresenceService>();
            services.AddScoped<Services.IConferenceService, Services.ConferenceService>();
            services.AddScoped<Services.IResolutionService, Services.NewResolutionService>();
            services.AddSingleton<Services.SpeakerlistService>();
            services.AddSingleton<Services.SimSimService>();

            // Swagger for Documentation
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "MUNity-API", Version = "v0.5" });

                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            

            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });

            

            app.UseCors("CorsPolicy");

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            if (!env.IsDevelopment())
            {
                app.UseSpaStaticFiles();
            }

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=index}/{id?}");
                endpoints.MapHub<Hubs.ResolutionHub>("/resasocket");
                endpoints.MapHub<Hubs.SpeakerListHub>("/slsocket");
                endpoints.MapHub<Hubs.SimulationHub>("/simsocket");
            });


            app.UseSpa(spa =>
            {
                // To learn more about options for serving an Angular SPA from ASP.NET Core,
                // see https://go.microsoft.com/fwlink/?linkid=864501

                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    spa.UseAngularCliServer(npmScript: "start");
                }
            });

            try
            {
                UpdateDatabase(app);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Console.WriteLine("Database-Service not found. Make sure that the MariaDB service is started and the app can access it.");
                throw;
            }
            
        }

        private static void UpdateDatabase(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices
                .GetRequiredService<IServiceScopeFactory>()
                .CreateScope())
            {
                using (var context = serviceScope.ServiceProvider.GetService<MunityContext>())
                {
                    context.Database.Migrate();
                }

                using (var context = serviceScope.ServiceProvider.GetService<MunCoreContext>())
                {
                    context.Database.Migrate();
                }
            }
        }
    }
}
