using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace DutchTreat
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            // Adds MVC service dependencies (req. for MapControllerRoute).
            services.AddControllersWithViews();
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
