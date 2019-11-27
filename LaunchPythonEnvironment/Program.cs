using System;
using System.Diagnostics;

namespace LaunchPythonEnvironment
{
    //public static class Program
    public class Program
    {
        private string predictionDirectory;// = @"C:\Users\Trevor\Dropbox\dcc\capstone\Capstone\MLClassifier\mLprojData\Predict";
        private string pythonScriptToExecute;
        private string pythonExePath;
        public string errors = "";
        public string results = "";
        ProcessStartInfo psi;
        //public bool PythonEnvironmentIsPrepared;


        public Program()
        {
            this.predictionDirectory = @"C:\Users\Trevor\Dropbox\dcc\capstone\Capstone\MLClassifier\mLprojData\Predict";
            this.pythonExePath = @"C:\Users\Trevor\AppData\Local\Programs\Python\Python37\python.exe";
            this.pythonScriptToExecute = @"C:\Users\Trevor\Dropbox\dcc\capstone\Capstone\MLClassifier\mLproj.py";
            this.psi = new ProcessStartInfo();
        }

        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            //if (PreparePythonEnvironment())
            //{
            //    PythonEnvironmentIsPrepared = true;

            //}
        }

        public bool PreparePythonEnvironment()
        {
            //create process info
            //var psi = new ProcessStartInfo();

            //!//

            //psi.FileName = @"C:\Users\Trevor\AppData\Local\Programs\Python\Python37\python.exe";
            psi.FileName = this.pythonExePath;

            //provide script and arguments
            //var script = @"C:\Users\Trevor\Dropbox\dcc\capstone\Capstone\MLClassifier\mLproj.py";
            var script = this.pythonScriptToExecute;

            psi.Arguments = $"\"{script}\"\"{predictionDirectory}\"";

            //process configuration
            psi.UseShellExecute = false;
            psi.CreateNoWindow = true;
            psi.RedirectStandardOutput = true;
            psi.RedirectStandardError = true;

            //PythonEnvironmentIsPrepared = true;

            ////execute process and get output
            //var errors = "";
            //var results = "";

            //using (var process = Process.Start(psi))
            //{
            //    errors = process.StandardError.ReadToEnd();
            //    results = process.StandardOutput.ReadToEnd();

            //    Console.WriteLine("ERRORS: " + errors);

            //    foreach (var result in results)
            //    {
            //        Console.WriteLine(result);
            //    }
            //}
            //Console.ReadLine();
            ////return output

            ////return results;
            return true;
        }

        public string PredictUploadedFiles()
        {
            //execute process and get output
            this.errors = "";
            this.results = "";

            using (var process = Process.Start(psi))
            {
                errors = process.StandardError.ReadToEnd();
                results = process.StandardOutput.ReadToEnd();

                Console.WriteLine("ERRORS: " + errors);

                foreach (var result in results)
                {
                    Console.WriteLine(result);
                }
            }
            //Console.ReadLine();
            
            //return output
            return results;
            //return "yay";
        }
    }
}
