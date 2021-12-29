using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatPath
{
    public class Program
    {
        public static void Main(string[] args)
        {
            initLogging();
            Log.Information("ChatPath baþladý");
            CreateHostBuilder(args).Build().Run();
        }
        public static void initLogging()
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .WriteTo.File("logs\\ChatPath.log", rollingInterval: RollingInterval.Day)
                .WriteTo.File("logs\\ChatPathError.log", rollingInterval: RollingInterval.Day,restrictedToMinimumLevel:Serilog.Events.LogEventLevel.Error)
                .WriteTo.File("logs\\ChatPathFatal.log", rollingInterval: RollingInterval.Day,restrictedToMinimumLevel:Serilog.Events.LogEventLevel.Fatal)
                .CreateLogger();

        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
