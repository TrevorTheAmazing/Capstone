using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using test.Data;
using test.Models;
using System.Security.Claims;
using Capstone;

namespace test.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UploadController : ControllerBase
    {
        //member variables
        //the hosting environment
        private IHostingEnvironment hostingEnvironment;//tlc
        private readonly ApplicationDbContext _context;//tlc
        //private readonly UserManager<IdentityUser> userManager;//tlc

        //constructor
        public UploadController(IHostingEnvironment hostingEnvironment, ApplicationDbContext context)
        {
            _context = context;//tlc
            this.hostingEnvironment = hostingEnvironment;
            //this.userManager = userManager;//tlc
        }

        //member methods
        [HttpPost]
        public async Task<HttpResponseMessage> Post(IList<IFormFile> files)//tlc
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
                //submission.originalFilename = filename;

                //set the path
                string uploadFilename = this.GetPathAndFilename(filename);

                //create the file on the local filesystem
                using (FileStream output = System.IO.File.Create(uploadFilename))
                    await source.CopyToAsync(output);

                //add the filename(actually the guid) to a list
                newFiles.Add(uploadFilename);

                //create a new submission for each file
                CreateNewSubmission(uploadFilename);
            }

            //send the list of new filenames to be processed to the brain
            Brain.ProcessUploads(newFiles);

            //respond to the request
            return new HttpResponseMessage()
            {
                Content = new StringContent("POST: Test message")
            };
        }

        private string EnsureCorrectFilename(string filename)//tlc
        {
            if (filename.Contains("\\"))
                filename = filename.Substring(filename.LastIndexOf("\\") + 1);

            return filename;
        }

        private string GetPathAndFilename(string filename)//tlc
        {
            return this.hostingEnvironment.WebRootPath + "\\mp3\\" + filename;
        }

        private void CreateNewSubmission(string uploadFilename)
        {
            string userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            Submission submission = new Submission();

            submission.filepath = uploadFilename;
            submission.UserId = userId;
            submission.originalFilename = Path.GetFileName(uploadFilename);
            _context.Add(submission);
            _context.SaveChanges();
        }
    }
}
