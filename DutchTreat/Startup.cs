using System.Reflection;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using DutchTreat.Data;
using DutchTreat.Data.Entities;
using DutchTreat.Services;

namespace DutchTreat
{
    public class Startup
    {
        private readonly IConfiguration _config;

        public Startup(IConfiguration config)
        {
            _config = config;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            // Creates identity system for user and role types. 
            services.AddIdentity<StoreUser, IdentityRole>(cfg =>
            {
                // Configuring the identity system.
                cfg.User.RequireUniqueEmail = true;
            })
                // Create EF implementation for the identity in the specified context.
                .AddEntityFrameworkStores<DutchContext>();
                // Can have separate data stores for the project vs identity, but not doing so for this app.
            
            // Creates DbContext as scoped service to be injected where needed.
            services.AddDbContext<DutchContext>(cfg =>
            {
                // Using MySQL required different setup process. Documented externally.
                cfg.UseMySql(_config.GetConnectionString("DutchConnectionString"));
            });

            // Sets up dependency injection to inject given class in place of interface.
            // Reconstructs everytime one is needed.
            services.AddTransient<IMailService, NullMailService>();
            // Support for real mail service needed

            // Registers DutchSeeder with DependencyInjection service layer.
            services.AddTransient<DutchSeeder>();

            // Add ability to inject AutoMapper into controllers of given assembly/project.
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            // Looks for profiles created for mappings within the given assembly.

            // Reuses single instance through scope and then deconstructs.
            services.AddScoped<IDutchRepository, DutchRepository>();

            // Adds MVC service dependencies (req. for MapControllerRoute).
            services.AddControllersWithViews()
                // Enforces compatibility for features like documentation attribute tags.
                .SetCompatibilityVersion(CompatibilityVersion.Version_3_0)
                // Allows child entities to have references to parents and still be included when querying them.
                //      The better option is to not have self-referencing loops. Need to fix models after tutorial.
                .AddNewtonsoftJson(opt => opt.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);
                // Newstonsoft has to be referenced and included specifically since being deprecated from main MVC.
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            /*---- Organizing Order of Middleware for Application ----*/

            // Checks project ASPNETCORE_ENVIRONMENT property for matching string value.
            if (env.IsEnvironment("Development"))
            {
                // Generates more useful exception page for developers.
                app.UseDeveloperExceptionPage();
            }
            else
            {
                /* -- NEVER GOT REDIRECT TO WORK */
                // Add user facing error page. 
                app.UseExceptionHandler("/error");
            }

            // Static file serving, but only from wwwroot directory - which had to be created.
            app.UseStaticFiles();

            // Required Nuget install of OdeToCode.UseNodeModules for .NET
            // Serves files from node_modules directory, installed JS modules like JQuery. 
            app.UseNodeModules();

            // Turns on generic routing from .NET Core
            app.UseRouting();

            // Places authentiaction in routing pipleline before it gets to end points.
            app.UseAuthentication(); // Identifies user.
            app.UseAuthorization();  // Verifies what user has acces to.

            // "Buys into" MVC. Makes controller and view endpoints connect.
            app.UseEndpoints(cfg =>
            {
                // Create endpoint for finding controllers using given semantics.
                cfg.MapControllerRoute("Fallback",
                    "{controller}/{action}/{id?}",  // Pattern match for controller actions.
                    new { controller = "App", action = "Index" }); // Default route if pattern fails.
            });
        }
    }
}
