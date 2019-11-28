using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace UnsignedArtists.Models
{
    public class UnsignedArtist
    {
        [Key]
        public int Id { get; set; }
        [Display(Name = "First")]
        public string firstName { get; set; }
        [Display(Name = "Last")]
        public string lastName { get; set; }
        [ForeignKey("UserId")]
        public string UserId { get; set; }
        public IdentityUser User { get; set; }
    }
}
