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
using MUNity.Database.Models.User;
using MUNity.Database.Context;
using MUNity.Services;

namespace MUNityCore
{
    public class Startup
    {
        private enum DatabaseConfigurations
        {
            MySql,
            SQLite
        }

        private DatabaseConfigurations databaseConfiguration;

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
            // Lagency stuff from Blazor times
            //services.AddRazorPages();
            //services.AddServerSideBlazor();
            //services.AddBlazorise(options =>
            //{
            //    options.ChangeTextOnKeyPress = true; // optional
            //}).AddBootstrapProviders()
            //.AddFontAwesomeIcons();

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

            services.AddIdentity<MunityUser, MUNityCore.Models.User.MunityRole>()
                .AddEntityFrameworkStores<MunityContext>()
                .AddDefaultUI()
                .AddDefaultTokenProviders();

            // TODO: Add OAuth some day!

            // SignalR is for the WebSockets, they are mainly used in the live Editors for example
            // the Resa Editors etc.
            services.AddSignalR(options =>
            {
                options.MaximumReceiveMessageSize = 68000;
                options.ClientTimeoutInterval = new System.TimeSpan(0,0,30);
                options.KeepAliveInterval = new TimeSpan(0, 0, 10);
            });

            // Setup the Database to use
            SetupDatabaseWithMySql(services);
            //SetupDatabaseWithSQLite(services);
            
            // All services that are used inside the controllers.
            //services.AddScoped<Services.InstallationService>();
            services.AddBlazoredLocalStorage();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IOrganisationService, OrganisationService>();
            services.AddScoped<IConferenceService, ConferenceService>();
            services.AddScoped<SqlResolutionService>();
            services.AddScoped<SpeakerlistService>();
            services.AddScoped<SimulationService>();
            services.AddScoped<AuthenticationStateProvider, ServerAuthenticationStateProvider>();
            services.AddScoped<FrontendSimulationService>();
            services.AddScoped<SpeakerlistHubService>();
            services.AddScoped<MainViewService>();
            services.AddScoped<LogSimulationService>();
            services.AddScoped<PresentsService>();

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
            try
            {
                UpdateDatabase(app);
            }
            catch (Exception e)
            {
                Logger.LogError("Unable to create the database!");
                Logger.LogError(e.StackTrace);
            }

            if (env.IsDevelopment())
            {
                //app.UseCors("DevPolicy");
                app.UseDeveloperExceptionPage();
                // Enable middleware to serve generated Swagger as a JSON endpoint.

                app.UseSwaggerUI(c =>
                 {
                     c.SwaggerEndpoint("/swagger/v1/swagger.json", "Munity API V1");
                     c.DisplayOperationId();
                 });

                app.UseSwagger();
            }
            else
            {
                Program.MasterToken = Environment.GetEnvironmentVariable("MUNITY_ADMIN_PASS");
                //app.UseCors("ProdAllOrigins");
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

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


            
        }

        private void SetupDatabaseWithMySql(IServiceCollection services)
        {
            var sqlServer = Environment.GetEnvironmentVariable("MYSQL_SERVER");
            if (sqlServer == null)
                sqlServer = "localhost";
            var sqlUser = Environment.GetEnvironmentVariable("MYSQL_USER");
            if (sqlUser == null)
                sqlUser = "root";
            var sqlPass = Environment.GetEnvironmentVariable("MYSQL_PASS");
            if (sqlPass == null)
                sqlPass = "";
            var sqlPort = Environment.GetEnvironmentVariable("MYSQL_PORT");
            if (sqlPort == null)
                sqlPort = "3306";
            var dbName = Environment.GetEnvironmentVariable("MYSQL_DATABASE");
            if (dbName == null)
                dbName = "munitybase";


            string mySqlConnectionString = $"server={sqlServer},{sqlPort};userid={sqlUser};password={sqlPass};database={dbName}";
            Console.WriteLine("Start with MariaDB");
            Console.WriteLine(mySqlConnectionString);
            var version = new Version(10, 1, 26);
            var serverVersion = new MariaDbServerVersion(version);

            services.AddDbContextFactory<MunityContext>(options =>
            {
                options.UseMySql(mySqlConnectionString, serverVersion, builder =>
                {
                    builder.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null);
                });

            });
            services.AddScoped<MunityContext>(p =>
                p.GetRequiredService<IDbContextFactory<MunityContext>>()
                .CreateDbContext());
            databaseConfiguration = DatabaseConfigurations.MySql;
        }

        private void SetupDatabaseWithSQLite(IServiceCollection services)
        {
            
            string sqlDbLiteName = "munity";
            // Create database
            var optionsBuilder = new DbContextOptionsBuilder<MunityContext>();
            optionsBuilder.UseSqlite($"Data Source={sqlDbLiteName}.db");
            var context = new MunityContext(optionsBuilder.Options);
            var created = context.Database.EnsureCreated();


            if (created)
            {
                Logger.LogInfo("SQLite Database created!");
            }

            services.AddDbContextFactory<MunityContext>(options =>
            {
                options.UseSqlite($"Data Source={sqlDbLiteName}.db");
            });
            services.AddScoped<MunityContext>(p =>
                p.GetRequiredService<IDbContextFactory<MunityContext>>()
                .CreateDbContext());
            databaseConfiguration = DatabaseConfigurations.SQLite;
            Logger.LogInfo($"Started with SQLite under: {Environment.CurrentDirectory}/{sqlDbLiteName}");
        }

        private void UpdateDatabase(IApplicationBuilder app)
        {
            using var serviceScope = app.ApplicationServices
                .GetRequiredService<IServiceScopeFactory>()
                .CreateScope();
            
            // Only migrate when its a MySQL Database. The SqlLite Database will be generated, when 
            // the Service is set up
            using var context = serviceScope.ServiceProvider.GetService<MunityContext>();
            if (databaseConfiguration == DatabaseConfigurations.MySql)
                context.Database.Migrate();

        }
    }
}
