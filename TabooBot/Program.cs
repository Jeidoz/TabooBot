using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;
using System.IO;

namespace TabooBot
{
    internal class Program
    {
        internal static IConfiguration Configuration { get; } = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appSettings.json", optional: false, reloadOnChange: true)
            .AddEnvironmentVariables()
            .Build();

        static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .WriteTo.File(Path.Combine("logs", "TabooBotLog.txt"), rollingInterval: RollingInterval.Day)
                .CreateLogger();

            try
            {
                CreateHostBuilder().Build().Run();
            }
            catch (Exception e)
            {
                Log.Logger.Fatal($"{e.Message}\n{e.StackTrace}");
                throw;
            }
            Log.CloseAndFlush();
        }

        private static IHostBuilder CreateHostBuilder()
        {
            return Host.CreateDefaultBuilder()
                .UseSystemd()
                .ConfigureServices(services =>
                {
                    services.AddLogging();
                    services.AddSingleton<IHostedService, TabooChatBot>();
                })
                .ConfigureLogging(configLogging => configLogging.AddSerilog(dispose: true));
        }
    }
}
