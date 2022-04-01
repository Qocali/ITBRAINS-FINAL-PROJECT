using Final.DAL;
using Final.Extentions;
using Final.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Final.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SpeakerController : Controller
    {
        #region databasepart
        private readonly AppDbContext _db;
        private readonly IWebHostEnvironment _env;
        private int id;

        public SpeakerController(AppDbContext db, IWebHostEnvironment env)
        {
            _db = db;
            _env = env;
        }
        #endregion
        #region Index
        public async Task<IActionResult> Index()
        {
            List<Speaker> speakers = await _db.Speakers.ToListAsync();
            return View(speakers);
        }
        #endregion
        #region Create new Speaker
        public async Task<IActionResult> Create()
        {
            
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Speaker speaker)
        {

            if (!ModelState.IsValid)
            {
                return View();
            }
            bool exist = _db.Speakers.Any(x => x.Name == speaker.Name);
            if (exist)
            {
                ModelState.AddModelError("Name", "Bu ad movcuddur");
                return View();
            }
            if (speaker.Photo == null)
            {
                ModelState.AddModelError("Photo", "Zehmet olmasa sekil elave edin!");
                return View();
            }
            if (!speaker.Photo.isImage())
            {
                ModelState.AddModelError("Photo", "Zehmet olmasa sekil secin!");
                return View();

            }
            if (speaker.Photo.isLower4mb())
            {
                ModelState.AddModelError("Photo", "Zehmet olmasa 4mb kecmeyin!");
                return View();
            }
            if (!ModelState.IsValid)
            {
                return View();
            }

            string folder = Path.Combine(_env.WebRootPath, "img", "event");
            speaker.Image = await speaker.Photo.savefileAsync(folder);
            await _db.Speakers.AddAsync(speaker);
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");

        }

        #endregion
        #region Edit seletion Course
        public async Task<IActionResult> Update(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            
            Speaker dbspeaker = _db.Speakers.FirstOrDefault(x => x.Id == id);
            if (dbspeaker == null)
            {
                return NotFound();
            }
            return View(dbspeaker);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Update(Speaker speaker, int? id)
        {


            if (id == null)
            {
                return NotFound();
            }

            Speaker dbspeaker = _db.Speakers.FirstOrDefault(x => x.Id == id);
            if (dbspeaker == null)
            {
                return NotFound();
            }
            if (speaker.Photo != null)
            {
                if (!speaker.Photo.isImage())
                {
                    ModelState.AddModelError("Photo", "Zehmet olmasa sekil secin!");
                    return View(dbspeaker);
                }
                if (speaker.Photo.isLower4mb())
                {
                    ModelState.AddModelError("Photo", "Zehmet olmasa 4mb kecmeyin!");
                    return View(dbspeaker);
                }
                if (!ModelState.IsValid)
                {

                    return View(dbspeaker);
                }
                string folder = Path.Combine(_env.WebRootPath, "img", "event");
                speaker.Image = await speaker.Photo.savefileAsync(folder);
                dbspeaker.Image = speaker.Image;
            }
           
            bool exist2 = _db.Courses.Any(x => x.Name == speaker.Name && x.Id != id);
            if (exist2)
            {
                ModelState.AddModelError("Name", "Bu ad movcuddur");
                return View(dbspeaker);
            }

            
            dbspeaker.Name= speaker.Name;
            dbspeaker.Venue= speaker.Venue;
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");


        }
        #endregion



    }
}
