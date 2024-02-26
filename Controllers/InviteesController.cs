using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using ApplicationPortal.Data;
using ApplicationPortal.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ApplicationPortal.Controllers
{
    [Authorize(Roles = "Admin")]
    public class InviteesController : Controller
    {
        private readonly ILogger<InviteesController> _logger;
        private readonly AppDbContext _db;
        private readonly IEmailSender _emailSender;

        public InviteesController(ILogger<InviteesController> logger, AppDbContext db, IEmailSender emailSender)
        {
            _logger = logger;
            _db = db;
            _emailSender = emailSender;
        }

        public IActionResult Index()
        {
            // IEnumerable<Applicants> applicants = _db.Applicants.Where(x => x.Invited == true);
            var applicants = _db.Applicants.Where(x => x.Invited == true).ToList();
            return View(applicants);
        }

        public IActionResult Accept(int id)
        {
            var applicant = _db.Applicants.Find(id);
            _emailSender.SendEmailAsync(applicant.Email, "You're Hired", $"<p>Dear {applicant.FirstName}</p>,<br/> <p>We are pleased to tell you that you've been hired.</p>");
            _db.Applicants.Remove(applicant);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Reject(int id)
        {
            var applicant = _db.Applicants.Find(id);
            _db.Applicants.Remove(applicant);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}