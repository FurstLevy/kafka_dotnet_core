using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using System;

namespace DotNetKafka.Producer
{
    public class Program
    {
        public static IConfiguration Configuration =>
            new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", false, true)
                .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", true)
                .AddEnvironmentVariables()
                .Build();

        public static void Main(string[] args)
        {
            ILogger<Program> logger = null;

            try
            {
                var t = Configuration;
                var host = CreateHostBuilder(args).Build();
                logger = host.Services.GetRequiredService<ILogger<Program>>();

                Log.Logger = new LoggerConfiguration()
                    .ReadFrom.Configuration(Configuration)
                    .Enrich.WithEnvironmentUserName()
                    .CreateLogger();

                logger.LogInformation(
                    $"Subindo API; ASPNETCORE_ENVIRONMENT = {Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")};");
                Log.Information(
                    $"Subindo API; ASPNETCORE_ENVIRONMENT = {Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")};");
                host.Run();
            }
            catch (Exception ex)
            {
                logger.LogCritical(ex, ex.Message);
            }

            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                    webBuilder.UseConfiguration(Configuration);
                    webBuilder.ConfigureLogging(logging =>
                    {
                        logging.ClearProviders();
                        logging.AddDebug();
                        logging.AddConsole();
                        logging.AddSerilog();
                    });
                });
    }
}
