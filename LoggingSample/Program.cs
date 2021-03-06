#define snippet_LogProgram
#define FirstProgram 

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
//using TodoApi.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
//using Microsoft.Extensions.Logging.AzureAppServices;
using Microsoft.Extensions.Logging.Debug;
using Microsoft.Extensions.Hosting;

namespace LoggingSample
{
#if FirstProgram
    public class Program
    {

#if LogFromMain
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            var todoRepository = host.Services.GetRequiredService<ITodoRepository>();
            todoRepository.Add(new Core.Model.TodoItem() { Name = "Feed the dog" });
            todoRepository.Add(new Core.Model.TodoItem() { Name = "Walk the dog" });

            var logger = host.Services.GetRequiredService<ILogger<Program>>();
            logger.LogInformation("Seeded the database.");

            IMyService myService = host.Services.GetRequiredService<IMyService>();
            myService.WriteLog("Logged from MyService.");

            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureLogging(logging =>
                {
                    logging.ClearProviders();
                    logging.AddConsole();
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
#elif AzLogOptions
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            var todoRepository = host.Services.GetRequiredService<ITodoRepository>();
            todoRepository.Add(new Core.Model.TodoItem() { Name = "Feed the dog" });
            todoRepository.Add(new Core.Model.TodoItem() { Name = "Walk the dog" });

            var logger = host.Services.GetRequiredService<ILogger<Program>>();
            logger.LogInformation("Seeded the database.");

            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureLogging(logging => logging.AddAzureWebAppDiagnostics())
                    .ConfigureServices(serviceCollection => serviceCollection
                        .Configure<AzureFileLoggerOptions>(options =>
                        {
                            options.FileName = "azure-diagnostics-";
                            options.FileSizeLimit = 50 * 1024;
                            options.RetainedFileCountLimit = 5;
                        }).Configure<AzureBlobLoggerOptions>(options =>
                        {
                            options.BlobName = "log.txt";
                        })
                    )
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
#elif Scopes
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            var todoRepository = host.Services.GetRequiredService<ITodoRepository>();
            todoRepository.Add(new Core.Model.TodoItem() { Name = "Feed the dog" });
            todoRepository.Add(new Core.Model.TodoItem() { Name = "Walk the dog" });

            var logger = host.Services.GetRequiredService<ILogger<Program>>();
            logger.LogInformation("Seeded the database.");

            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureLogging((hostingContext, logging) =>
                {
                    logging.ClearProviders();
                    logging.AddConsole(options => options.IncludeScopes = true);
                    logging.AddDebug();
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
#elif FilterInCode
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureLogging(logging =>
                    logging.AddFilter("System", LogLevel.Debug)
                           .AddFilter<DebugLoggerProvider>("Microsoft", LogLevel.Trace))
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
#elif MinLevel
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureLogging(logging => logging.SetMinimumLevel(LogLevel.Warning))
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
#elif FilterFunction
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureLogging(logBuilder =>
                {
                    logBuilder.AddFilter((provider, category, logLevel) =>
                    {
                        if (provider == "Microsoft.Extensions.Logging.Console.ConsoleLoggerProvider" &&
                            category == "TodoApi.Controllers.TodoController")
                        {
                            return false;
                        }
                        return true;
                    });
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
#elif snippet_LogProgram
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            var logger = host.Services.GetRequiredService<ILogger<Program>>();
            logger.LogInformation("Host created.");

            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
#else
        // Do nothing.
#endif
    }

#else
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
#endif
}
