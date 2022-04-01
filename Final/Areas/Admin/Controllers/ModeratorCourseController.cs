using Final.DAL;
using Final.Extentions;
using Final.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using static Final.Extentions.Helper;

namespace Final.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Mederator")]
    public class ModeratorCourseController : Controller
    {
        #region DbContext
        private readonly AppDbContext _db;
        private readonly IWebHostEnvironment _env;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        private readonly RoleManager<IdentityRole> _roleManager;


        public ModeratorCourseController(AppDbContext db, IWebHostEnvironment env, UserManager<AppUser> userManager,
                                 RoleManager<IdentityRole> roleManager,
                                 SignInManager<AppUser> signInManager)
        {
            _db = db;
            _env = env;

            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
        }
        #endregion

        #region Index
        public async Task<IActionResult> Index()
        {
            string userid= _userManager.GetUserId(User);
            List<Course> courses = await _db.Courses.Where(x=>x.AppUserId== userid).OrderByDescending(x => x.Id).Include(x => x.CourseDetail).Include(x => x.CourseCategories).ToListAsync();
            return View(courses);
        }
        #endregion

        #region Edit seletion Course
        public async Task<IActionResult> Update(int? id)
        {

            if (id == null)
            {
                return NotFound();
            }
            ViewBag.CourseCatigories = await _db.CourseCategories.ToListAsync();

            Course dbcourse = _db.Courses.Include(x => x.CourseCategories).Include(x => x.CourseDetail).FirstOrDefault(x => x.Id == id);
            if (dbcourse == null)
            {
                return NotFound();
            }
            return View(dbcourse);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Update(Course course, int? id, int? CourseId)
        {
            ViewBag.CourseCatigories = await _db.CourseCategories.ToListAsync();

            Course dbcourse = _db.Courses.Include(x => x.CourseCategories).Include(x => x.CourseDetail).FirstOrDefault(x => x.Id == id);


            if (dbcourse == null)
            {
                return NotFound();
            }
            if (course.Photo != null)
            {
                if (!course.Photo.isImage())
                {
                    ModelState.AddModelError("Photo", "Zehmet olmasa sekil secin");
                    return View(dbcourse);
                }
                if (course.Photo.isLower4mb())
                {
                    ModelState.AddModelError("Photo", "Zehmet olmasa 4mb kecmeyin!");
                    return View(dbcourse);
                }
                if (!ModelState.IsValid)
                {

                    return View(dbcourse);
                }
                string folder = Path.Combine(_env.WebRootPath, "img", "course");
                course.Image = await course.Photo.savefileAsync(folder);
                dbcourse.Image = course.Image;
            }
            
            bool exist2 = _db.Courses.Any(x => x.Name == course.Name && x.Id != id);
            if (exist2)
            {
                ModelState.AddModelError("Name", "Bu ad movcuddur");
                return View(dbcourse);
            }
            dbcourse.Name = course.Name;
            dbcourse.CourseDetail.ASSESMENTS = course.CourseDetail.ASSESMENTS;
            dbcourse.CourseDetail.Description = course.CourseDetail.Description;
            dbcourse.CourseDetail.LANGUAGE = course.CourseDetail.LANGUAGE;
            dbcourse.CourseDetail.SKILLLEVEL = course.CourseDetail.SKILLLEVEL;
            dbcourse.CourseDetail.STUDENTS = course.CourseDetail.STUDENTS;
            dbcourse.CourseDetail.Title = course.CourseDetail.Title;
            dbcourse.CourseDetail.SubTitle = course.CourseDetail.SubTitle;
            dbcourse.CourseDetail.DURATION = course.CourseDetail.DURATION;
            dbcourse.CourseDetail.STARTS = course.CourseDetail.STARTS;
            dbcourse.CourseCategoriesId = (int)CourseId;
            dbcourse.CourseDetail.CourseId = course.Id;
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");


        }
        #endregion

    }
}
