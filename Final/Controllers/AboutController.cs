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
    public class AboutController : Controller
    {
        #region DbPart
        private readonly AppDbContext _db;
        public AboutController(AppDbContext db)
        {
            _db = db;
        }
        #endregion

        #region Index
        public async Task<IActionResult> Index()
        {
            TeacherVM teacherVM = new TeacherVM
            {
                About = await _db.Abouts.FirstOrDefaultAsync(),
                Teacher= await _db.Teacher.OrderByDescending(x=>x.Id).Take(4).Include(x => x.Profession).Include(x => x.SocialMedia).ToListAsync()
        };
            return View(teacherVM);
        }
        #endregion
    }
}
