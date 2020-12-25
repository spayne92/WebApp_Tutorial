using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using DutchTreat.Data;

namespace DutchTreat
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = BuildWebHost(args);
            SeedDb(host);
            host.Run();

            // Old one-line technique using obsolete IHostBuilder.
            //CreateHostBuilder(args).Build().Run();
        }

        private static void SeedDb(IWebHost host)
        {
            // Creates scope for lifetime of request with instance of context outside of web server, since no request being made. 
            // No web server request means no context instance exists, we have to create one for running once at startup.
            var scopeFactory = host.Services.GetService<IServiceScopeFactory>();
            using (var scope = scopeFactory.CreateScope())
            {
                // Previously just host.Services.GetService<DutchSeeder>(); and seeder.Seed().
                // But required scoped context to be wrapped around it and used.
                var seeder = scope.ServiceProvider.GetService<DutchSeeder>();
                seeder.Seed();
            }
        }

        private static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                   .ConfigureAppConfiguration(SetupConfiguration)
                   .UseStartup<Startup>()
                   .Build();

        // Obsolete HostBuilder method. Used this from scratch project somehow.
        //public static IHostBuilder CreateHostBuilder(string[] args) =>
        //    Host.CreateDefaultBuilder(args)
        //        .ConfigureAppConfiguration(SetupConfiguration)
        //        .ConfigureWebHostDefaults(webBuilder =>
        //        {
        //            webBuilder.UseStartup<Startup>();
        //        });

        private static void SetupConfiguration(WebHostBuilderContext ctx, IConfigurationBuilder builder)
        {
            // Removing the default configuration options.
            builder.Sources.Clear();

            // Adds multiple configs from multiple sources to central store.
            // Treats order of lines as hierarchy for conflicting config settings.
            // JSON line adds main config as not optional and reload on change.
            builder.AddJsonFile("config.json", false, true)
                .AddXmlFile("config.xml", true) // optional
                .AddEnvironmentVariables();
        }
    }
}
