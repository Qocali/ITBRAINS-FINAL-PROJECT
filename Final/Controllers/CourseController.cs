using Final.DAL;
using Final.Models;
using Final.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Final.Controllers
{
    public class CourseController : Controller
    {
        #region Databasepart
        private readonly AppDbContext _db;
        public CourseController(AppDbContext db)
        {
            _db = db;
        }
        #endregion
        #region Index
        public async Task<IActionResult> Index(int page=1)
        {
            ViewBag.Page = page;
            ViewBag.Pagecount = Math.Ceiling((decimal)_db.Courses.Count() / 6);
            List<Course> Course = await _db.Courses.Include(x => x.CourseDetail).OrderByDescending(x => x.Id).Skip((page - 1) * 6).Where(x => !x.IsDeactive).Take(6).ToListAsync();
           
            ViewBag.Categories = await _db.CourseCategories.Where(x => !x.IsDeactive).ToListAsync();
            return View(Course);
        }
        #endregion
        #region Detail
        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            ViewBag.Course = await _db.Courses.FirstOrDefaultAsync(x => x.Id == id);
            CourseDetail courseDetails = await _db.CourseDetails.Include(x => x.Course).FirstOrDefaultAsync(x => x.CourseId == id);

            return View(courseDetails);
        }
        #endregion
        #region Searchpart
        public async Task<IActionResult> SearchCourse(string search)
        {

            List<Course> courses = await _db.Courses.Where(x => x.Name.Contains(search)).Include(x=>x.CourseDetail).Take(3).ToListAsync();
            if (search == null)
            {
                List<Course> courses1 = await _db.Courses.Include(x => x.CourseDetail).Take(5).ToListAsync();
                return PartialView("_SearchCoursePartrial", courses1);
            }
            return PartialView("_SearchCoursePartrial", courses);
        }
        #endregion
        #region Searchcatogorypart
        public async Task<IActionResult> SearchCourseCategory(int id)
        {

            List<Course> courses = await _db.Courses.Where(x => x.CourseCategoriesId==id).Include(x => x.CourseDetail).Take(3).ToListAsync();
            
            return View(courses);
        }
        #endregion
    }
}
