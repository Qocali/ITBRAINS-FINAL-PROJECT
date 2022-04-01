using Final.DAL;
using Final.Extentions;
using Final.Models;
using Final.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
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
    [Authorize(Roles = " Admin")]
    public class CourseController : Controller
    {
        #region DbContext
        private readonly AppDbContext _db;
        private readonly IWebHostEnvironment _env;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        private readonly RoleManager<IdentityRole> _roleManager;


        public CourseController(AppDbContext db, IWebHostEnvironment env, UserManager<AppUser> userManager,
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
        public async Task<IActionResult> Index(int page = 1)
        {
            ViewBag.Page = page;
            ViewBag.Pagecount = Math.Ceiling((decimal)_db.Courses.Count() / 8);
            
            List<Course> courses = await _db.Courses.OrderByDescending(x => x.Id).Skip((page - 1) * 8).Take(8).ToListAsync();
            if(User.IsInRole("Mederator"))
            {
                return RedirectToAction("Index", "ModeratorCourse");
            }


            return View(courses);
        }
        #endregion

        #region Activate or inverse
        public async Task<IActionResult> Active(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Course dbcourse = _db.Courses.FirstOrDefault(x => x.Id == id);
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
        

        #region Create new Course
        public async Task<IActionResult> Create()
        {
            List<ModeratorVM> moderators = new List<ModeratorVM>();
            var items = await _userManager.GetUsersInRoleAsync("Mederator");
            foreach (var item in items)
            {
                moderators.Add(new ModeratorVM { Id = item.Id, Name = item.Name, Surname = item.Surname });
            }
            ViewBag.moderators = moderators;
            ViewBag.CourseCatigories = await _db.CourseCategories.ToListAsync();
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Course course, int? id, int CourseId, string modeId)
        {
            
            ViewBag.CourseCatigories = await _db.CourseCategories.ToListAsync();
            if (!ModelState.IsValid)
            {
                return View();
            }
            bool exist = _db.Courses.Any(x => x.Name == course.Name);
            if (exist)
            {
                ModelState.AddModelError("Name", "Bu ad artiq movcuddur");
                return View();
            }
            if (course.Photo == null)
            {
                ModelState.AddModelError("Photo", "Zehmet olmasa sekil elave edin");
                return View();
            }
            if (!course.Photo.isImage())
            {
                ModelState.AddModelError("Photo", "Zehmet olmasa sekil secin!");
                return View();

            }
            if (course.Photo.isLower4mb())
            {
                ModelState.AddModelError("Photo", "Zehmet olamsa 4mb kecmeyin!");
                return View();
            }
            if (!ModelState.IsValid)
            {
                return View();
            }
            
            string folder = Path.Combine(_env.WebRootPath, "img", "course");
            course.Image = await course.Photo.savefileAsync(folder);
            CourseDetail courseDetail = new CourseDetail();
            courseDetail.ASSESMENTS = course.CourseDetail.ASSESMENTS;
            courseDetail.Description = course.CourseDetail.Description;
            courseDetail.LANGUAGE = course.CourseDetail.LANGUAGE;
            courseDetail.CourseId = course.Id;
            courseDetail.SKILLLEVEL = course.CourseDetail.SKILLLEVEL;
            courseDetail.Title = course.CourseDetail.Title;
            courseDetail.SubTitle = course.CourseDetail.SubTitle;
            courseDetail.STUDENTS = course.CourseDetail.STUDENTS;
            course.CourseDetail = courseDetail;

            course.CourseCategoriesId = CourseId;
            
            List<ModeratorVM> moderators = new List<ModeratorVM>();
            var items = await _userManager.GetUsersInRoleAsync(" Mederator");
            foreach (var item in items)
            {
                moderators.Add(new ModeratorVM { Id = item.Id, Name = item.Name, Surname = item.Surname });
            }
            ViewBag.moderators = moderators;
            course.AppUserId = modeId;


           

            await _db.Courses.AddAsync(course);
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
            ViewBag.CourseCatigories = await _db.CourseCategories.ToListAsync();
            List<ModeratorVM> moderators = new List<ModeratorVM>();
            var items = await _userManager.GetUsersInRoleAsync("Mederator");
            foreach (var item in items)
            {
                moderators.Add(new ModeratorVM { Id = item.Id, Name = item.Name, Surname = item.Surname });
            }
            ViewBag.moderators = moderators;
            Course dbcourse = _db.Courses.Include(x => x.CourseCategories).Include(x => x.CourseDetail).FirstOrDefault(x => x.Id == id);
            if (dbcourse == null)
            {
                return NotFound();
            }
            return View(dbcourse);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Update(Course course, int? id, int? CourseId, string modeId)
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
                    ModelState.AddModelError("Photo", "Zehmet olmasa 4mb kecmeyin !");
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
                ModelState.AddModelError("Name", "Bu kurs artiq movcuddur");
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
            List<ModeratorVM> moderators = new List<ModeratorVM>();
            var items = await _userManager.GetUsersInRoleAsync(" Mederator");
            foreach (var item in items)
            {
                moderators.Add(new ModeratorVM { Id = item.Id, Name = item.Name, Surname = item.Surname });
            }
            ViewBag.moderators = moderators;
            dbcourse.AppUserId = modeId;
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");


        }
        #endregion

        




    }
}