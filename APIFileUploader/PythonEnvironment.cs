using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace APIFileUploader
{


    //constructor
    public class PythonEnvironment
    {
        //member variables
        private string predictionDirectory;// = @"C:\Users\Trevor\Dropbox\dcc\capstone\Capstone\MLClassifier\mLprojData\Predict";
        private string pythonScriptToExecute;
        private string pythonExePath;
        public string errors = "";
        public string results = "";
        public bool PredictionsWereMade;
        ProcessStartInfo processStartInfo;
        //public bool PythonEnvironmentIsPrepared;

        public PythonEnvironment()
        {
            this.predictionDirectory = @"C:\Users\Trevor\Dropbox\dcc\capstone\Capstone\MLClassifier\mLprojData\Predict";
            this.pythonExePath = @"C:\Users\Trevor\AppData\Local\Programs\Python\Python37\python.exe";
            this.pythonScriptToExecute = @"C:\Users\Trevor\Dropbox\dcc\capstone\Capstone\MLClassifier\MLClassifier.py";
            this.processStartInfo = new ProcessStartInfo();
            this.PredictionsWereMade = false;
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
                processStartInfo.RedirectStandardError = true;
            }
            catch(Exception e)
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
            bool ErrorsHappened = false;

            try
            {
                using (var process = Process.Start(processStartInfo))
                {
                    //results = process.StandardOutput.ReadToEnd();
                    errors = process.StandardError.ReadToEnd();
                    

                    Console.WriteLine("ERRORS: " + errors);

                    //foreach (var result in results)
                    //{
                    //    Console.WriteLine(result);
                    //    Console.ReadLine();
                    //}
                }
            }
            catch(Exception e)
            {
                Console.WriteLine("Errors happened: " + e.Message);
                ErrorsHappened = true;
            }
            finally
            {
                if (!ErrorsHappened)
                {
                    PredictionsWereMade = true;
                }
            }


            return results;
        }
    }
}
