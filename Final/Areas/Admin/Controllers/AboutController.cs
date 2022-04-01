using Final.DAL;
using Final.Extentions;
using Final.Models;
using Microsoft.AspNetCore.Authorization;
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
    [Authorize(Roles ="Admin")]
    public class AboutController : Controller
    {
        #region Databasepart
        private readonly AppDbContext _db;
        private readonly IWebHostEnvironment _env;
        private int id;

        public AboutController(AppDbContext db, IWebHostEnvironment env)
        {
            _db = db;
            _env = env;
        }
        #endregion
        public async Task<IActionResult> Index(int page=1)
        {
            ViewBag.Page = page;
            ViewBag.Pagecount = Math.Ceiling((decimal)_db.Abouts.Count() / 8);
            List<About> abouts = await _db.Abouts.OrderByDescending(x => x.Id).Skip((page - 1) * 8).Take(8).ToListAsync();


            return View(abouts);
        }
        #region Activate or inverse
        public async Task<IActionResult> Active(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            About dbabout = _db.Abouts.FirstOrDefault(x => x.Id == id);
            if (dbabout == null)
            {
                return NotFound();
            }
            if (dbabout.IsDeactive)
            {
                dbabout.IsDeactive = false;
            }
            else
            {
                dbabout.IsDeactive = true;
            }
            _db.SaveChanges();
            return RedirectToAction("Index");

        }
        #endregion
        #region Create new About
        public async Task<IActionResult> Create()
        {
           
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(About about)
        {

           
            if (!ModelState.IsValid)
            {
                return View();
            }
            if (about.Photo==null)
            {
                ModelState.AddModelError("Photo", "Zehmet olmasa sekil elave edin");
                return View();
            }
            if (!about.Photo.isImage())
            {
                ModelState.AddModelError("Photo", "zehmet olmasa sekil secin!");
                return View();

            }
            if (about.Photo.isLower4mb())
            {
                ModelState.AddModelError("Photo", "zehmet olmasa  4mb kecmeyin!");
                return View();
            }
            if (!ModelState.IsValid)
            {
                return View();
            }
            
            string folder = Path.Combine(_env.WebRootPath, "img", "about");
            about.Image = await about.Photo.savefileAsync(folder);
            await _db.Abouts.AddAsync(about);
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");

        }

        #endregion
        #region Edit seletion About
        public async Task<IActionResult> Update(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            About dbabout = _db.Abouts.FirstOrDefault(x => x.Id == id);
            if(dbabout==null)
            {
                return NotFound();
            }

            return View(dbabout);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Update(About about, int? id)
        {
            About dbabout = _db.Abouts.FirstOrDefault(x => x.Id == id);


            if (dbabout == null)
            {
                return NotFound();
            }

            if (about.Photo != null)
            {
                if (!about.Photo.isImage())
                {
                    ModelState.AddModelError("Photo", "Zehmet olmasa sekil secin!");
                    return View(dbabout);

                }
                if (about.Photo.isLower4mb())
                {
                    ModelState.AddModelError("Photo", "Zehmet olmasa 4mb kecmeyin!");
                    return View(dbabout);
                }
                if (!ModelState.IsValid)
                {
                    return View(dbabout);
                }
                string folder = Path.Combine(_env.WebRootPath, "img", "about");
                about.Image = await about.Photo.savefileAsync(folder);
                dbabout.Image = about.Image;
            }
           
            dbabout.Title = about.Title;
            dbabout.SubTitle = about.SubTitle;
            dbabout.Description = about.Description;
            
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");


        }
        #endregion
    }
}
