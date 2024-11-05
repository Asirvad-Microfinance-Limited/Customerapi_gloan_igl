using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace CustomerAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        //public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
        //    WebHost.CreateDefaultBuilder(args)
        //        .UseStartup<Startup>();

        public static IWebHost BuildWebHost(string[] args) =>
           new WebHostBuilder()
               .UseKestrel()
               .UseContentRoot(Directory.GetCurrentDirectory())
               .ConfigureAppConfiguration((hostingContext, config) =>
               {
                   var env = hostingContext.HostingEnvironment;

                   // find the shared folder in the parent folder
                   var sharedFolder = Path.Combine(env.ContentRootPath, "..", "Shared");

                   //load the SharedSettings first, so that appsettings.json overrwrites it
                   config
                      .AddJsonFile(Path.Combine(sharedFolder, "SharedSettings.json"), optional: true)
                      .AddJsonFile("appsettings.json", optional: true)
                      .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

                   config.AddEnvironmentVariables();
               }).UseStartup<Startup>().Build();
    }
}
