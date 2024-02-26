using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ApplicationPortal.Data;
using ApplicationPortal.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ApplicationPortal.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ApplicationsController : Controller
    {
        private readonly ILogger<ApplicationsController> _logger;
        private readonly AppDbContext _db;
        private readonly IEmailSender _emailSender;
        private readonly IWebHostEnvironment webHost;

        public ApplicationsController(ILogger<ApplicationsController> logger, AppDbContext db, IEmailSender emailSender, IWebHostEnvironment webHost)
        {
            _logger = logger;
            _db = db;
            _emailSender = emailSender;
            this.webHost = webHost;
        }

        public IActionResult Index()
        {
            IEnumerable<Applicants> applicants = _db.Applicants.Where(x => x.Invited == false);
            return View(applicants);
        }

        public IActionResult Applicant(int id)
        {
            var applicant = _db.Applicants.FirstOrDefault(x => x.Id == id && x.Invited == false);
            return View(applicant);
        }

        public IActionResult RejectApplicant(int id)
        {
            var applicant = _db.Applicants.Find(id);
            if (applicant != null)
            {
                _db.Applicants.Remove(applicant);
                _db.SaveChanges();
                _emailSender.SendEmailAsync(applicant.Email, "We're Sorry", $"<p>Dear {applicant.FirstName}</p>,<br/> <p>We are sorry to say that we do not think you will be a good fit for our company.</p>");
            }
            else
            {
                return NotFound();
            }
            return RedirectToAction("Index");
        }

        public IActionResult InviteApplicant(int id)
        {
            var applicant = _db.Applicants.Find(id);
            if (applicant != null)
            {
                applicant.Invited = true;
                _db.SaveChanges();
                _emailSender.SendEmailAsync(applicant.Email, "You're Invited", $"<p>Dear {applicant.FirstName}</p>,<br/> <p>Congrats, You have been shortlisted for an interview with us.</p>");
            }
            else
            {
                return NotFound();
            }
            return RedirectToAction("Index");
        }

        public IActionResult ViewCV(string pdfFile)
        {
            var webRootPath = webHost.WebRootPath;
            var pdfDir = webRootPath + @"\cv\";
            string filePath = Path.Combine(webRootPath, pdfDir, pdfFile);
            byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);
            // MemoryStream memory = new MemoryStream();
            // using (FileStream stream = new FileStream(Path.Combine(pdfDir + pdfFile), FileMode.Open))
            // {
            //     stream.CopyTo(memory);
            // }
            return View(fileBytes);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}