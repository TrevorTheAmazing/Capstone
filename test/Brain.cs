using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using NAudio;
using NAudio.Wave;
using Capstone.Private;
using test.Data;
using Capstone;
using Microsoft.AspNetCore.Identity;
using test.Models;
using Microsoft.EntityFrameworkCore;
using MimeKit;
using MailKit.Net.Smtp;

namespace Capstone
{
    public static class Brain
    {
        static string predictionsPath = Private.Private.predictionsPath;
        static string labelRepEmailAddress = Private.Private.labelRepEmailAddress;
        static string appMasterEmailAddress = Private.Private.appMasterEmailAddress;
        static string appMasterEmailPassword = Private.Private.appMasterEmailPassword;

        public static void ProcessUploads(List<string> newFiles)
        {
            foreach (var file in newFiles)
            {
                //if the file is an .mp3 (it should be...)
                if (Path.GetExtension(file) == ".mp3")
                {
                    //... convert it to .wav
                    ConvertMp3ToWav(file);
                }
            }

            //use machine learning to classify uploads based on genre
            ClassifyConvertedUploads();

            //done for now.
        }

        private static void ConvertMp3ToWav(string mp3)
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
                        WaveFileWriter.CreateWaveFile(predictionsPath + tempFilename + ".wav", pcmStream);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("*** 3RR0R ***" + tempFilename + " *** 3RROR *** " + e.Message + "*** 3RROR ***");
            }

            //return true;
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

        public static void ClearUploadsFromPredictionsDirectory(List<string> filesToDelete)
        {
            //remove the uploaded files after they have been classified
            for (int i = 0; i < filesToDelete.Count(); i++)
            {
                File.Delete(filesToDelete[i]);
            }
        }

        public static void NewResultsFile(string newResultsFile)
        {
            //0. process the new file into a list
            List<string> resultsList = new List<string>();

            resultsList = ProcessResults(newResultsFile, resultsList);

            //if there are results...
            if (resultsList.Count() > 0)
            {
                //...
                List<string> filesToDelete = new List<string>();

                for (int i = 0; i < resultsList.Count(); i++)
                {
                    string predictionString = resultsList[i].Substring(0, resultsList[i].IndexOf(" "));
                    string filepath = resultsList[i].Remove(0, resultsList[i].IndexOf("C:\\"));
                    filesToDelete.Add(filepath);
                    string filename = Path.GetFileNameWithoutExtension(filepath);

                    //... and the result label matches the preferred labels ...
                    if (Private.Private.targetLabels.Contains(predictionString))
                    {
                        //... notify the label.
                        EmailResults(filename);
                    }
                    else
                    {
                        //... track down the artist to tell them the bad news?
                    }
                }

                //remove the processed files from the predictions directory.
                ClearUploadsFromPredictionsDirectory(filesToDelete);
            }
        }

        public static List<string> ProcessResults(string newResultsFile, List<string> resultsList)
        {
            using (StreamReader reader = new StreamReader(newResultsFile))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    resultsList.Add(line);
                }
            }

            return resultsList;
        }

        public static void EmailResults(string filename)
        {
            //email results now.   
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Computer Dude", appMasterEmailAddress));
            message.To.Add(new MailboxAddress("Mr. Record Label Exec", labelRepEmailAddress));
            message.Subject = "*AWESOME NEW SONG ALERT*" + filename;

            message.Body = new TextPart("plain")
            {
                Text = @"Hey Mister, somebody just uploaded a song that appears to be not unlike that in which you are in search of."
            };

            using (var client = new SmtpClient())
            {
                client.Connect("smtp.gmail.com", 587);


                // Note: since we don't have an OAuth2 token, disable
                // the XOAUTH2 authentication mechanism.
                client.AuthenticationMechanisms.Remove("XOAUTH2");

                // Note: only needed if the SMTP server requires authentication
                client.Authenticate(appMasterEmailAddress, appMasterEmailPassword);

                client.Send(message);
                client.Disconnect(true);
            }
        }
    }
}
