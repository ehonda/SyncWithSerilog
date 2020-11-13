using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using Serilog.Templates;
using System;

namespace SyncWithSerilog
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //var outputTemplate = "[{Timestamp:HH:mm:ss} {Level:u3}] - {Message} {Properties}{NewLine}{Exception}";
            //var expressionTemplate = new ExpressionTemplate(
            //    "{ {@t, @l, @m, @x, ..@p} }\n");

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                .Enrich.FromLogContext()
                .WriteTo.Console(
                    /*expressionTemplate*/
                    /*outputTemplate: outputTemplate*/)
                .WriteTo.File(
                    //expressionTemplate,
                    "log.txt",
                    rollingInterval: RollingInterval.Day/*,*/
                    /*outputTemplate: outputTemplate*/)
                .CreateLogger();

            try
            {
                Log.Information("Starting web host");
                CreateHostBuilder(args).Build().Run();
            }
            catch (Exception exception)
            {
                Log.Fatal(exception, "Host terminated unexpectedly");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseSerilog()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
