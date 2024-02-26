using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationPortal.Models
{
    public class Applicants
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Years of Experience(0 if none)")]
        public int YearsOfExperience { get; set; }

        [Required]
        [Url]
        public string LinkedIn { get; set; }

        [Required]
        [Url]
        public string GitHub { get; set; }

        [Required]
        public string CV { get; set; }

        public bool Invited { get; set; }
    }
}