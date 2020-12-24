using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace DutchTreat
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration(SetupConfiguration)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });

        private static void SetupConfiguration(HostBuilderContext ctx, IConfigurationBuilder builder)
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
