using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace test.Models
{
    public class Submission
    {
        [Key]
        public int SubmissionId { get; set; }

        public string filepath { get; set; }
        public string originalFilename { get; set; }
        public string guid { get; set; }
        public string labelledGenre { get; set; }

        [ForeignKey("UserId")]
        public string UserId { get; set; }
        public IdentityUser User { get; set; }
    }
}
