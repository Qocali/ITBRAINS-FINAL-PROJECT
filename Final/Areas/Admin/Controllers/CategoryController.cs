using Final.DAL;
using Final.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Final.Areas.Admin.Controllers
{
    
        [Area("Admin")]
        [Authorize(Roles = "Admin")]
        public class CategoryController : Controller
        {
            #region DbContext
            private readonly AppDbContext _db;
            private readonly IWebHostEnvironment _env;

            public CategoryController(AppDbContext db, IWebHostEnvironment env)
            {
                _db = db;
                _env = env;
            }
            #endregion

            #region Index
            public async Task<IActionResult> Index()
            {

                List<CourseCategories> categories = await _db.CourseCategories.Include(x => x.Courses).ToListAsync();
                return View(categories);
            }
            #endregion

            #region Create
            public async Task<IActionResult> Create()
            {
                ViewBag.Categories = await _db.CourseCategories.Where(x=>!x.IsDeactive).ToListAsync();
                return View();
            }
            [HttpPost]
            [ValidateAntiForgeryToken]
            public async Task<IActionResult> Create(CourseCategories category)
            {
                ViewBag.Categories = await _db.CourseCategories.Where(x => !x.IsDeactive).ToListAsync();
                bool exist = _db.CourseCategories.Any(x => x.CourseName == category.CourseName);
                if (exist)
                {
                    ModelState.AddModelError("Name", "This Category is already exist!");
                    return View();
                }

                await _db.CourseCategories.AddAsync(category);
                await _db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
        #endregion

        #region Activate or inverse
        public async Task<IActionResult> Activity(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            CourseCategories dbcourse = _db.CourseCategories.FirstOrDefault(x => x.Id == id);
            if (dbcourse == null)
            {
                return NotFound();
            }
            if (dbcourse.IsDeactive)
            {
                dbcourse.IsDeactive = false;
            }
            else
            {
                dbcourse.IsDeactive = true;
            }
            _db.SaveChanges();
            return RedirectToAction("Index");

        }
        #endregion
        #region Update
        
        public async Task<IActionResult> Update(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            ViewBag.Categories = await _db.CourseCategories.Where(x => !x.IsDeactive).ToListAsync();
            
            CourseCategories dbcategories = _db.CourseCategories.FirstOrDefault(x => x.Id == id);
            if (dbcategories == null)
            {
                return NotFound();
            }
            return View(dbcategories);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(CourseCategories category, int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            ViewBag.Categories = await _db.CourseCategories.Where(x => !x.IsDeactive).ToListAsync();
            CourseCategories dbcategories = _db.CourseCategories.FirstOrDefault(x=>x.Id==id);
            bool exist = _db.CourseCategories.Any(x => x.CourseName == category.CourseName);
            if (exist)
            {
                ModelState.AddModelError("Name", "This Category is already exist!");
                return View(dbcategories);
            }

            dbcategories.CourseName = category.CourseName;
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
       
        #endregion



    }
}
