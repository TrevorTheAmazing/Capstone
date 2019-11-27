using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace APIFileUploader
{
    public class Program
    {
        //variables
        static FileSystemWatcher _watcher;

        //methods
        public static void Main(string[] args)
        {
            Init();//tlc
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();

        static void Init() //tlc
        {
            string directory = @"C:\Users\Trevor\Dropbox\dcc\capstone\Capstone\MLClassifier\mLprojData\Results\";
            Program._watcher = new FileSystemWatcher(directory);
            Program._watcher.Changed += new FileSystemEventHandler(Program._watcher_changed);
            Program._watcher.EnableRaisingEvents = true;
            Program._watcher.IncludeSubdirectories = true;
        }

        private static void _watcher_changed(object sender, FileSystemEventArgs e) //tlc
        {
            Console.WriteLine("CHANGED, NAME: " + e.Name);
            Console.WriteLine("CHANGED, FULLPATH: " + e.FullPath);
            Brain.NewResultsFile(e.FullPath + e.Name);


        }

       
    }
}
