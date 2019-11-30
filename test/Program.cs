using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Capstone;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using test.Controllers;

namespace test
{
    public class Program
    {
        //variables
        public static FileSystemWatcher watcher;

        public static void Main(string[] args)
        {
            InitWatcherForResults();//tlc
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });

        static void InitWatcherForResults() //tlc
        {
            string directory = @"C:\Users\Trevor\Dropbox\dcc\capstone\Capstone\test\MLClassifier\mLprojData\Results\";
            watcher = new FileSystemWatcher();
            watcher.Path = directory;
            watcher.NotifyFilter = NotifyFilters.FileName;
            watcher.Created += new FileSystemEventHandler(OnCreated);
            watcher.EnableRaisingEvents = true;
        }

        private async static void OnCreated(object source, FileSystemEventArgs e) //tlc
        {
            Console.WriteLine("new file created - " + e.FullPath);
            Brain.NewResultsFile(e.FullPath);
        }
    }
}
