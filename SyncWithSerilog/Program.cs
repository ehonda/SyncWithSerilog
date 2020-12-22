using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.Elasticsearch;
using SyncWithSerilog.Logging.FormatProviders;
using System;

namespace SyncWithSerilog
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Serilog.Debugging.SelfLog.Enable(Console.Error);

            var outputTemplate = 
                "[{Timestamp:HH:mm:ss} {Level:u3}] " +
                "{Message:lj}{NewLine}{Exception}";

            //"{Message:lj} {@Article}{NewLine}{Exception}";
            //var expressionTemplate = new ExpressionTemplate(
            //    "{ {@t, @l, @m, @x, ..@p} }\n");

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                .Enrich.FromLogContext()
                .WriteTo.Console(
                    outputTemplate: outputTemplate,
                    formatProvider: new EventFormatter())
                .WriteTo.File(
                    ".logs/log.txt",
                    rollingInterval: RollingInterval.Day,
                    outputTemplate: outputTemplate,
                    formatProvider: new EventFormatter())
                .WriteTo.Elasticsearch(
                    new ElasticsearchSinkOptions(new Uri("http://localhost:9200"))
                    {
                        AutoRegisterTemplate = true,
                        BufferBaseFilename = ".logs/elasticbuffer",
                        //FormatProvider = new EventFormatter(),
                        IndexFormat = "article2-{0:yyyy.MM.dd}"
                    })
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
