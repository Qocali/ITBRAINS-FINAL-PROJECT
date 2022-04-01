using Final.DAL;
using Final.Models;
using Final.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Final.Controllers
{
    public class TeacherController : Controller
    {
        #region DbPart
        private readonly AppDbContext _db;
        public TeacherController(AppDbContext db)
        {
            _db = db;
        }
        #endregion
        #region Index
        public async Task<IActionResult> Index(int page = 1)
        {
            ViewBag.Page = page;
            ViewBag.Pagecount = Math.Ceiling((decimal)_db.Teacher.Count() / 6);
            List<Teacher> teachers = await _db.Teacher.Skip((page - 1) * 6).Include(x => x.SocialMedia).Include(x => x.Profession).Where(x=>!x.IsDeactive).Take(6).ToListAsync();
            return View(teachers);
        }
        #endregion
        #region Detail Teacher
        [Authorize(Roles = " Admin,Member,Mederator")]
        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Teacher teacher =await _db.Teacher.Include(x => x.Profession).Include(x => x.Teacherdetails).Include(x => x.SocialMedia).Include(x => x.TeacherSkillsmanys).ThenInclude(x => x.TeacherSkills).FirstOrDefaultAsync(x => x.Id == id);

            return View(teacher);
        }
        #endregion
        #region TeacherSearch
        public async Task<IActionResult> TeacherSearch(string keyword)
        {
           
            List<Teacher> teachers = await _db.Teacher.Where(x => x.Name.Contains(keyword)).Include(x=>x.SocialMedia).Include(x=>x.Profession).ToListAsync();
            if (keyword == null)
            {
                List<Teacher> teachers1 = await _db.Teacher.Include(x => x.SocialMedia).Include(x => x.Profession).ToListAsync();
                return PartialView("_SearchTeacherPartial", teachers1);
            }
            return PartialView("_SearchTeacherPartial", teachers);
        }
        #endregion




    }
}
