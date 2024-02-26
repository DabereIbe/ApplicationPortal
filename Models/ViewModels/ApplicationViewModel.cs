using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace ApplicationPortal.Models.ViewModels
{
    public class ApplicationViewModel
    {
        public Jobs Jobs { get; set; }
        public Applicants Applicants { get; set; }
        public IdentityUser IdentityUser { get; set; }
    }
}