using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Cors;

namespace APIFileUploader.Controllers
{
    [Route("api/[controller]")]
    [EnableCors("CorsPolicy")]
    [ApiController]
    public class UploadController : ControllerBase
    {
        //member variables
        //the hosting environment
        private IHostingEnvironment hostingEnvironment;

        //constructor
        public UploadController(IHostingEnvironment hostingEnvironment)
        {
            this.hostingEnvironment = hostingEnvironment;
        }

        //member methods
        [HttpPost]
        public async Task<HttpResponseMessage> Post(IList<IFormFile> files)
        {
            //use list in case of multiple files
            List<string> newFiles = new List<string>();

            //for each file submitted
            foreach (IFormFile source in files)
            {
                //grab the filename
                string filename = ContentDispositionHeaderValue.Parse(source.ContentDisposition).FileName.Trim('"');

                //validate the filename
                filename = this.EnsureCorrectFilename(filename);

                //set the path
                string uploadFilename = this.GetPathAndFilename(filename);

                //create the file on the local filesystem
                //using (FileStream output = System.IO.File.Create(this.GetPathAndFilename(filename)))
                using (FileStream output = System.IO.File.Create(uploadFilename))
                    await source.CopyToAsync(output);

                //add the filename to a list
                newFiles.Add(uploadFilename);
            }

            //send the list of new filenames to be processed to the brain
            Brain.ProcessUploads(newFiles);

            //respond to the request
            return new HttpResponseMessage()
            {
                Content = new StringContent("POST: Test message")
            };
        }

        private string EnsureCorrectFilename(string filename)
        {
            if (filename.Contains("\\"))
                filename = filename.Substring(filename.LastIndexOf("\\") + 1);

            return filename;
        }

        private string GetPathAndFilename(string filename)
        {
            return this.hostingEnvironment.WebRootPath + "\\mp3\\" + filename;
            //return "GetPathAndFilename()";
        }

        //[HttpGet]
        //public ActionResult<IEnumerable<string>> Get()
        //{
        //    return new string[] { "upload1", "upload2" };
        //}
    }
}