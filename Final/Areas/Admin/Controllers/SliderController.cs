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

namespace EduHomeFinal.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class SliderController : Controller
    {
        #region DbContext
        private readonly AppDbContext _db;
        private readonly IWebHostEnvironment _env;
        private int id;

        public SliderController(AppDbContext db, IWebHostEnvironment env)
        {
            _db = db;
            _env = env;
        }
        #endregion

        #region Index
        public async Task<IActionResult> Index()
        {
            List<Slider> slider = await _db.sliders.OrderByDescending(x => x.Id).ToListAsync();
            return View(slider);
        }
        #endregion

        #region Create
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Slider slider)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            if (slider.Photo == null)
            {
                ModelState.AddModelError("Photo","Zehmet olmasa sekil elave edin!");
                return View();
            }
            if (slider.Photo2 == null)
            {
                ModelState.AddModelError("Photo2", "Zehmet olmasa sekil elave edin!!");
                return View();
            }
            if (!slider.Photo.isImage())
            {
                ModelState.AddModelError("Photo","Zehmet olmasa sekil secin!");
                return View();
            }
            if (slider.Photo.isLower4mb())
            {
                ModelState.AddModelError("Photo","Zehmet olmasa  4Mb kecmeyin!");
            }
            string folder = Path.Combine(_env.WebRootPath, "img", "slider");
            slider.Image = await slider.Photo.savefileAsync(folder);
            if (slider.Photo2 != null)
            {
                if (!slider.Photo2.isImage())
                {
                    ModelState.AddModelError("Photo2", "Zehmet olmasa sekil secin!");
                    return View();
                }
                if (slider.Photo2.isLower4mb())
                {
                    ModelState.AddModelError("Photo2", "Zehmet olmasa  4Mb kecmeyin!");
                    return View();
                }
                
                string folder2 = Path.Combine(_env.WebRootPath, "img", "slider");
                slider.BackgroundImage = await slider.Photo2.savefileAsync(folder2);
            }
            

            Slider Slider = new Slider();
            Slider.Id = slider.Id;
            Slider.Image = slider.Image;
            Slider.Title = slider.Title;
            Slider.SubTitle = slider.SubTitle;
            Slider.Description = slider.Description;
            await _db.sliders.AddAsync(slider);
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        #endregion

        #region Update
        public async Task<IActionResult> Update(int?id)
        {
            Slider dbSlider = await _db.sliders.FirstOrDefaultAsync(x=>x.Id==id);
            if (dbSlider == null)
            {
                return NotFound();
            }
            return View(dbSlider);

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(Slider slider,int?id)
        {
            Slider dbSlider = await _db.sliders.FirstOrDefaultAsync(x => x.Id == id);
            if (id == null)
            {
                return NotFound();
            }
            if (dbSlider==null)
            {
                return NotFound();
            }
            if (!ModelState.IsValid)
            {
                return View(dbSlider);
            }
            if (slider.Photo!=null)
            {
                if (!slider.Photo.isImage())
                {
                    ModelState.AddModelError("Photo", "Zehmet olmasa sekil secin !");
                    return View(dbSlider);
                }
                if (slider.Photo.isLower4mb())
                {
                    ModelState.AddModelError("Photo", "Zehmet olmasa  4Mb kecmeyin!");
                    return View(dbSlider);
                }
                
                string folder = Path.Combine(_env.WebRootPath, "img", "slider");
                slider.Image = await slider.Photo.savefileAsync(folder);
                dbSlider.Image = slider.Image;
            }
           
            if (slider.Photo2 != null)
            {
                if (!slider.Photo2.isImage())
                {
                    ModelState.AddModelError("Photo2", "Zehmet olmasa sekil secin!");
                    return View(dbSlider);
                }
                if (slider.Photo2.isLower4mb())
                {
                    ModelState.AddModelError("Photo2", "Zehmet olmasa  4Mb kecmeyin!");
                    return View(dbSlider);
                }
                
                string folder = Path.Combine(_env.WebRootPath, "img", "slider");
                slider.BackgroundImage= await slider.Photo2.savefileAsync(folder);
                dbSlider.BackgroundImage = slider.BackgroundImage;
            }
            else
            {
                ModelState.AddModelError("Photo2", "Zehmet olmasa sekil elave edin");
                return View();
            }

            dbSlider.Title = slider.Title;
            dbSlider.SubTitle = slider.SubTitle;
            dbSlider.Description = slider.Description;
            dbSlider.Id = slider.Id;
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        #endregion

        #region Delete
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Slider slide = _db.sliders.FirstOrDefault(x => x.Id == id);

            if (slide == null)
            {
                return NotFound();
            }
            string path = Path.Combine(_env.WebRootPath, "img", slide.Image);
            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }
            _db.Remove(slide);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
        #endregion

    }
}
