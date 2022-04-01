using Final.Models;
using Final.ViewModels;
using Final.DAL;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Final.Controllers
{
    
    public class HomeController : Controller
    {
        #region DbPart
        private readonly AppDbContext _db;
        public HomeController(AppDbContext db)
        {
            _db = db;
        }
        #endregion
        #region Index
        public async Task<IActionResult> Index()
        {
            HomeVM homeVM = new HomeVM
            {
                Slider = await _db.sliders.ToListAsync(),
                //SliderImage = await _db.sliderimages.ToListAsync(),
                Course = await _db.Courses.Where(x => !x.IsDeactive).OrderByDescending(x => x.Id).Take(4).ToListAsync(),
                About = await _db.Abouts.FirstOrDefaultAsync(),
                Events = await _db.Events.Where(x => !x.IsDeactive).OrderByDescending(x => x.Id).Take(4).ToListAsync(),
                Blogs = await _db.Blogs.Where(x => !x.IsDeactive).OrderByDescending(x => x.Id).Take(4).ToListAsync()
            };
            return View(homeVM);
        }
        #endregion
        public async Task<IActionResult> SearchGlobal(string Search)
        {
            CourseVM courseVM = new CourseVM()
            {
                Events = await _db.Events.Where(x=>x.EventName.Contains(Search)).OrderByDescending(x => x.Id).Take(2).ToListAsync(),
                Courses = await _db.Courses.Where(x => x.Name.Contains(Search)).OrderByDescending(x => x.Id).Take(2).ToListAsync(),
                blogs = await _db.Blogs.Where(x => x.Creator.Contains(Search)).OrderByDescending(x => x.Id).Take(2).ToListAsync(),
                Teachers = await _db.Teacher.OrderByDescending(x => x.Id).Where(x => x.Name.Contains(Search) && !x.IsDeactive).Include(x => x.Teacherdetails).Include(x => x.Profession).Include(x => x.TeacherSkillsmanys).ThenInclude(x=>x.TeacherSkills).Include(x => x.SocialMedia).Take(2).ToListAsync(),
            };
            return PartialView("_SearchGlobalPartrial", courseVM);
        }
        #region Subscribed When Enter E-mail
        public async Task<ContentResult> SubScribe(string email)
        {

            Subscribe subscribe = new Subscribe();
            subscribe.Email = email;
            bool exist = _db.subscribes.Any(x => x.Email == email);
            if (exist)
            {

                return Content("Bu E-mail Movcuddur!!!");
            }

            await _db.subscribes.AddAsync(subscribe);
            await _db.SaveChangesAsync();
            return Content("E-mail Ugurla Subscribed olundu");
        }
        #endregion
    }
}
