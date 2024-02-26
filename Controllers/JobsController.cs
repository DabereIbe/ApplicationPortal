using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using ApplicationPortal.Data;
using ApplicationPortal.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;

namespace ApplicationPortal.Controllers
{
    [Authorize(Roles = "Admin")]
    // [Route("[controller]")]
    public class JobsController : Controller
    {
        private readonly ILogger<JobsController> _logger;
        private readonly AppDbContext _db;

        public JobsController(ILogger<JobsController> logger, AppDbContext db)
        {
            _logger = logger;
            _db = db;
        }
        public List<SelectListItem> GetRanks()
        {
            List<SelectListItem> listOfRanks = new List<SelectListItem>();
            var ranks = _db.Ranks.ToList();
            foreach (var item in ranks)
            {
                listOfRanks.Add(new SelectListItem{Value = item.Id.ToString(), Text = item.Name});
            }
            return listOfRanks;
        }

        public IActionResult Index()
        {
            var rank = _db.Ranks.Where(x => x.Name == "Junior" || x.Name == "Intermediate" || x.Name == "Senior").FirstOrDefault();
            // switch (rank.Name)
            // {
            //     case "Junior":
            //     _db.Ranks.AddRange(new Rank {Name = "Intermediate"}, new Rank {Name = "Senior"});
            //     break;
            //     case "Intermediate":
            //     _db.Ranks.AddRange(new Rank {Name = "Junior"}, new Rank {Name = "Senior"});
            //     break;
            //     case "Senior":
            //     _db.Ranks.AddRange(new Rank {Name = "Junior"}, new Rank {Name = "Intermediate"});
            //     break;
            //     default :
            //     _db.Ranks.AddRange(new Rank {Name = "Junior"}, new Rank {Name = "Intermediate"}, new Rank {Name = "Senior"});
            //     break;
            // }
            if (rank == null)
            {
                _db.Ranks.AddRange(new Rank {Name = "Junior"}, new Rank {Name = "Intermediate"}, new Rank {Name = "Senior"});
                // _db.Ranks.Add(new Rank {Name = "Junior"});
                // _db.Ranks.Add(new Rank {Name = "Intermediate"});
                // _db.Ranks.Add(new Rank {Name = "Senior"});
                _db.SaveChanges();
            }
            IEnumerable<Jobs> jobs = _db.Jobs;
            return View(jobs);
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Ranks = GetRanks();
            Jobs jobs = new Jobs();
            return View(jobs);
        }

        [HttpPost]
        public IActionResult Create(Jobs job)
        {
            _db.Jobs.Add(job);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            ViewBag.Ranks = GetRanks();
            return View(_db.Jobs.Find(id));
        }

        [HttpPost]
        public IActionResult Edit(Jobs job)
        {
            _db.Jobs.Update(job);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            var job = _db.Jobs.Find(id);
            if (job != null)
            {
                _db.Jobs.Remove(job);
                _db.SaveChanges();
            }
            else
            {
                return NotFound();
            }
            return RedirectToAction("Index");
        }
    }
}