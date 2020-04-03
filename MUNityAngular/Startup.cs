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
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using MongoDB.Driver;
using MUNityAngular.DataHandlers;
using Microsoft.Extensions.Options;
using MUNityAngular.DataHandlers.EntityFramework;

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


            services.AddSignalR(options =>
            {
                options.MaximumReceiveMessageSize = 68000;
                options.ClientTimeoutInterval = new System.TimeSpan(2,0,0);
            }).AddJsonProtocol(options => {
                options.PayloadSerializerOptions.PropertyNamingPolicy = null;
                
            });

            var mySqlConnectionString = Configuration.GetValue<string>("MySqlSettings:ConnectionString");

            // Add Entity Framework MariaDB Database
            services.AddDbContextPool<DataHandlers.EntityFramework.MunityContext>(options =>
                options.UseMySql(mySqlConnectionString, mySqlOptions => {
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
            services.AddScoped<Services.InstallationService>();
            services.AddScoped<Services.AuthService>();
            services.AddScoped<Services.ResolutionService>();
            services.AddScoped<Services.PresenceService>();
            services.AddScoped<Services.ConferenceService>();
            services.AddSingleton<Services.SpeakerlistService>();

            // Swagger for Documentation
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "MUNity-API", Version = "v0.4" });

                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });

            //This should make "AnNewtonsoftJson" obsolete
            services.AddMvc().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.PropertyNamingPolicy = null;
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

            app.UseAuthentication();

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
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=index}/{id?}");
                endpoints.MapHub<Hubs.TestHub>("/signalrtest");
                endpoints.MapHub<Hubs.ResolutionHub>("/resasocket");
                endpoints.MapHub<Hubs.SpeakerListHub>("/slsocket");
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

            UpdateDatabase(app);
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
            }
        }
    }
}
