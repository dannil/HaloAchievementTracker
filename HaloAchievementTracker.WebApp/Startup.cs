using HaloAchievementTracker.Common;
using HaloAchievementTracker.Common.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace HaloAchievementTracker.WebApp
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();
            services.AddControllersWithViews();
            services.AddMemoryCache();

            // In production, the Angular files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/dist";
            });

            services.AddHttpClient<ISteamService, SteamService>(typeof(SteamService).Name, client =>
            {
                client.BaseAddress = new Uri("https://steamcommunity.com/profiles/");
            });

            services.AddHttpClient<IOpenXBLService, OpenXBLService>(typeof(OpenXBLService).Name, client =>
            {
                client.BaseAddress = new Uri("https://xbl.io/api/v2/");
                client.DefaultRequestHeaders.Add("X-Authorization", Configuration[Constants.CONFIGURATION_KEY_OPENXBL_API_KEY]);
                client.DefaultRequestHeaders.Add("Accept", "application/json");
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IHostApplicationLifetime lifetime, IOpenXBLService openXBLService)
        {
            app.UseCors(options =>
            {
                //var clientUrl = Configuration["ClientAppUrl"];
                options.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader();
            });

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
                    pattern: "{controller}/{action=Index}/{id?}");
            });

            app.UseSpa(spa =>
            {
                // To learn more about options for serving an Angular SPA from ASP.NET Core,
                // see https://go.microsoft.com/fwlink/?linkid=864501

                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    //spa.UseAngularCliServer(npmScript: "start");
                    spa.UseProxyToSpaDevelopmentServer(Configuration["ClientAppUrl"]);
                }
            });

            lifetime.ApplicationStarted.Register(OnApplicationStartedAsync(openXBLService).Wait);
        }

        private async Task<Action> OnApplicationStartedAsync(IOpenXBLService openXBLService)
        {
            await openXBLService.FindClubs("placeholder");
            return null;
        }
    }
}
