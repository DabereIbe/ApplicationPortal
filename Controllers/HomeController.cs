using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ApplicationPortal.Models;
using ApplicationPortal.Data;
using ApplicationPortal.Models.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Authorization;

namespace ApplicationPortal.Controllers
{
    
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly AppDbContext _db;
        private readonly IEmailSender _emailSender;

        public HomeController(ILogger<HomeController> logger, AppDbContext db, UserManager<IdentityUser> userManager, IWebHostEnvironment webHostEnvironment, IEmailSender emailSender)
        {
            _logger = logger;
            _db = db;
            _userManager = userManager;
            _webHostEnvironment = webHostEnvironment;
            _emailSender = emailSender;
        }

        public IActionResult Index()
        {
            HomeViewModel vm = new HomeViewModel
            {
                Ranks = _db.Ranks,
                Jobs = _db.Jobs.Include(x => x.Rank)
            };
            return View(vm);
        }
        public async Task<IActionResult> Apply(int jobId, string email)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Redirect($"~/Identity/Account/Login?jobId={jobId}");
            }
            ApplicationViewModel vm = new ApplicationViewModel
            {
                Jobs = _db.Jobs.FirstOrDefault(x => x.Id == jobId),
                Applicants = new Applicants(),
                IdentityUser = await _userManager.FindByEmailAsync(email)
            };
            return View(vm);
        }

        [HttpPost]
        public IActionResult Apply(ApplicationViewModel viewModel, IFormFile file)
        {
            var webRootPath = _webHostEnvironment.WebRootPath;
            var uploadDir = webRootPath + @"\cv\";
            var filename = Path.GetFileName(file.FileName);
            var filePath = uploadDir + filename;
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                file.CopyTo(fileStream);
            }
            Applicants applicants = new Applicants
            {
                FirstName = viewModel.Applicants.FirstName,
                LastName = viewModel.Applicants.LastName,
                Email = viewModel.IdentityUser.Email,
                YearsOfExperience = viewModel.Applicants.YearsOfExperience,
                LinkedIn = viewModel.Applicants.LinkedIn,
                GitHub = viewModel.Applicants.GitHub,
                CV = filename
            };
            _db.Applicants.Add(applicants);
            _db.SaveChanges();
            _emailSender.SendEmailAsync(viewModel.IdentityUser.Email,
             "Thanks For Applying", $"{applicants.FirstName}, Thank you for indicating interest in our company. We are processing your application and will get back to you with response");
            return View("Success");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
