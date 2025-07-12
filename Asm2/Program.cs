using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Sinks.MSSqlServer;

namespace Asm2
{
    public class Program
    {
        //[Obsolete]
        public static void Main(string[] args)
        {
            try
            {
                var configuration = new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json")
                    .Build();

                var sinkOptions = new MSSqlServerSinkOptions
                {
                    TableName = "Logs",
                    AutoCreateSqlTable = true
                };

                Log.Logger = new LoggerConfiguration()
                    .WriteTo.File("Logs/log.txt"
                                , rollingInterval: RollingInterval.Day
                                , outputTemplate: "{TimeStamp} [{level}] - Message: {Message}{NewLine}{Exception}")
                    .WriteTo.MSSqlServer(configuration.GetConnectionString("MedicalDbConnection"), sinkOptions)
                    .CreateLogger();

                CreateHostBuilder(args).Build().Run();
            }
            catch (Exception)
            {

                throw;
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
