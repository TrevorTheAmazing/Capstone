using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using NAudio;
using NAudio.Wave;

namespace APIFileUploader
{
    public static class Brain
    {
        //path to predictions
        static string newPath = @"C:\Users\Trevor\Dropbox\dcc\capstone\Capstone\MLClassifier\mLprojData\Predict\";
        //array of target labels
        static string[] targetLabels = new string[3] { "classical", "country", "metal" };

        public static void ProcessUploads(List<string> newFiles)
        {            
            foreach (var file in newFiles)
            {
                //if the file is an .mp3 (it should be...)
                if (Path.GetExtension(file)==".mp3")
                {
                    //... convert it to .wav
                    ConvertMp3ToWav(file);
                }
            }

            ClassifyConvertedUploads();

            //if the results are in accord with the target labels, email the results to the A/R rep
            //MailKit(predictions)
        }

        private static bool ConvertMp3ToWav(string mp3)
        {
            //get the filename without its extension
            string tempFilename = Path.GetFileNameWithoutExtension(mp3);// = "001066.mp3"
            try
            {
                //create a new mp3 reader
                using (Mp3FileReader reader = new Mp3FileReader(mp3))
                {
                    //create a new wav reader
                    using (WaveStream pcmStream = WaveFormatConversionStream.CreatePcmStream(reader))
                    {
                        //create a new .wav
                        WaveFileWriter.CreateWaveFile(newPath + tempFilename + ".wav", pcmStream);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("*** 3RR0R ***" + tempFilename + " *** 3RROR *** " + e.Message + "*** 3RROR ***");
            }

            return true;
        }

        public static void ClassifyConvertedUploads()
        {
            //prepare python stuff
            PythonEnvironment pythonEnvironment = new PythonEnvironment();

            //if the python environment is ready
            if (pythonEnvironment.PreparePythonEnvironment())
            {
                //make predictions for the uploaded files
                Console.WriteLine(pythonEnvironment.MakePredictions());
            }
        }

        public static void NewResultsFile(string newResultsFile)
        {
            List<string> resultsList = new List<string>();
            using (StreamReader reader = new StreamReader(newResultsFile))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    resultsList.Add(line);
                }
            }

            //foreach (var prediction in resultsList)
            for (int i = 0; i > resultsList.Count(); i++)
            {
                //string predictionString = prediction.Substring(0, prediction.IndexOf(" "));
                string predictionString = resultsList[i].Substring(0, resultsList[i].IndexOf(" "));
                if (targetLabels.Contains(predictionString))
                {
                    //do nothing
                }
                else
                {
                    resultsList.RemoveAt(i);
                }
            }

            //email the results
            bool mailIsReady = false;
            if (mailIsReady)
            {
                EmailResults(resultsList);
            }
        }

        public static void EmailResults(List<string> resultsList)
        {
            //email results now.
        }

    }
}
