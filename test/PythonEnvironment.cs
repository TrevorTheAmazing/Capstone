using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Capstone
{
    public class PythonEnvironment
    {
        //member variables
        public string errors = "";
        public string results = "";
        private string pythonExePath;
        private string pythonScriptToExecute;
        ProcessStartInfo processStartInfo;

        //constructor
        public PythonEnvironment()
        {
            this.pythonExePath = @"C:\Users\Trevor\AppData\Local\Programs\Python\Python37\python.exe";
            this.pythonScriptToExecute = @"C:\Users\Trevor\Desktop\capstone\Capstone\test\MLClassifier\MLClassifier.py";
            this.processStartInfo = new ProcessStartInfo();
        }

        //member methods
        public bool PreparePythonEnvironment()
        {
            try
            {
                //create process info
                processStartInfo.FileName = this.pythonExePath;

                //provide script and arguments
                var script = this.pythonScriptToExecute;

                //processStartInfo.Arguments = $"\"{script}\"\"{predictionDirectory}\"";
                processStartInfo.Arguments = $"\"{script}";

                //process configuration
                processStartInfo.UseShellExecute = false;
                processStartInfo.CreateNoWindow = true;
                processStartInfo.RedirectStandardOutput = false;
                processStartInfo.RedirectStandardError = false;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }

            return true;
        }

        public string MakePredictions()
        {
            //execute process and get output
            this.errors = "";
            this.results = "";

            try
            {
                Process.Start(processStartInfo);
            }
            catch (Exception e)
            {
                Console.WriteLine("Errors happened: " + e.Message);
                //ErrorsHappened = true;
            }


            return results;
        }
    }
}
