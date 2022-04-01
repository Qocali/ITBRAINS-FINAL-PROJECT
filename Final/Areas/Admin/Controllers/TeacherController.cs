using Final.DAL;
using Final.Extentions;
using Final.Models;
using Final.ViewModels;
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
    [Authorize(Roles = "Admin")]
    public class TeacherController : Controller
    {
        #region DbPart
        private readonly AppDbContext _db;
        private readonly IWebHostEnvironment _env;
        private int id;

        public TeacherController(AppDbContext db, IWebHostEnvironment env)
        {
            _db = db;
            _env = env;
        }
        #endregion
        #region Index
        public async Task<IActionResult> Index(int page = 1)
        {
            ViewBag.Page = page;
            ViewBag.Pagecount = Math.Ceiling((decimal)_db.Teacher.Count() / 8);
            List<Teacher> Teachers = await _db.Teacher.OrderByDescending(x => x.Id).Skip((page - 1) * 8).Take(8).Include(x => x.Profession).Include(x => x.Teacherdetails).ToListAsync();


            return View(Teachers);
        }
        #endregion
        #region Activate or Deactivate
        public async Task<IActionResult> Active(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Teacher dbteacher = _db.Teacher.FirstOrDefault(x => x.Id == id);
            if (dbteacher == null)
            {
                return NotFound();
            }
            if (dbteacher.IsDeactive)
            {
                dbteacher.IsDeactive = false;
            }
            else
            {
                dbteacher.IsDeactive = true;
            }
            _db.SaveChanges();
            return RedirectToAction("Index");

        }
        #endregion
        #region Create New Teacher
        public async Task<IActionResult> Create()
        {
            ViewBag.profes = await _db.Professions.ToListAsync();
            ViewBag.skill = await _db.TeacherSkills.ToListAsync();
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int? ProfesId, Teacher teacher, int[] skillid)
        {
            ViewBag.profes = await _db.Professions.ToListAsync();
            ViewBag.skill = await _db.TeacherSkills.ToListAsync();
            if (!ModelState.IsValid)
            {
                return View();
            }
            if (teacher.Photo == null)
            {
                ModelState.AddModelError("Photo", "Zehmet olmasa sekil elave edin!");
                return View();
            }
            if (!teacher.Photo.isImage())
            {
                ModelState.AddModelError("Photo", "Zehmet olmasa sekil elave et!");
                return View();

            }
            if (teacher.Photo.isLower4mb())
            {
                ModelState.AddModelError("Photo", "Zehmet olmasa 4mb kecmeyin!");
                return View();
            }


            string folder = Path.Combine(_env.WebRootPath, "img", "teacher");
            teacher.Image = await teacher.Photo.savefileAsync(folder);


            bool exist2 = _db.Teacher.Any(x => x.Name == teacher.Name);
            if (exist2)
            {
                ModelState.AddModelError("Name", "Bu ad movcuddur");
                return View();
            }
            if (ProfesId == null)
            {
                return NotFound();
            }
            List<TeacherSkillsmany> teacherSkillsmany = new List<TeacherSkillsmany>();
            foreach (int item in skillid)
            {
                TeacherSkillsmany teacherskillsmany = new TeacherSkillsmany();
                teacherskillsmany.TeacherId = teacher.Id;
                teacherskillsmany.TeacherSkillsId = item;
                teacherSkillsmany.Add(teacherskillsmany);
            }
            TeacherDetails teacherDetails = new TeacherDetails();
            teacherDetails.Hobbies = teacher.Teacherdetails.Hobbies;
            teacherDetails.Experience = teacher.Teacherdetails.Experience;
            teacherDetails.Description = teacher.Teacherdetails.Description;
            teacherDetails.Degree = teacher.Teacherdetails.Degree;
            teacherDetails.Email = teacher.Teacherdetails.Email;
            teacherDetails.Faculty = teacher.Teacherdetails.Faculty;
            teacherDetails.PhoneNumber = teacher.Teacherdetails.PhoneNumber;
            teacherDetails.Scype = teacher.Teacherdetails.Scype;
            teacherDetails.SubTitle = teacher.Teacherdetails.SubTitle;
            teacherDetails.Title = teacher.Teacherdetails.Title;
            teacherDetails.TeacherId = teacher.Id;
            teacher.Teacherdetails = teacherDetails;
            teacher.ProfessionId = (int)ProfesId;
            teacher.TeacherSkillsmanys = teacherSkillsmany;
            teacher.TeacherSkillsmanys = teacherSkillsmany;
            SocialMedia socialMedia = new SocialMedia();
            socialMedia.Facebook = teacher.SocialMedia.Facebook;
            socialMedia.Twitter = teacher.SocialMedia.Twitter;
            socialMedia.Pinterest = teacher.SocialMedia.Pinterest;
            socialMedia.Vimeo = teacher.SocialMedia.Vimeo;
            socialMedia.TeacherId = teacher.Id;
            teacher.SocialMedia = socialMedia;
            await _db.Teacher.AddAsync(teacher);

            await _db.SaveChangesAsync();

            return RedirectToAction("Index");
        }
        #endregion

        #region Edit Teacher Information
        public async Task<IActionResult> Update(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            ViewBag.profes = await _db.Professions.ToListAsync();
            ViewBag.skill = await _db.TeacherSkills.Include(x=>x.TeacherSkillsmanys).ToListAsync();
            Teacher dbteacher = _db.Teacher.Include(x => x.Profession).Include(x=>x.SocialMedia).Include(x => x.Teacherdetails).FirstOrDefault(x => x.Id == id);
            if (dbteacher == null)
            {
                return NotFound();
            }
            return View(dbteacher);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Update(Teacher teacher, int? id, int[] skillid, int? ProfesId)
        {
            ViewBag.profes = await _db.Professions.ToListAsync();
            ViewBag.skill = await _db.TeacherSkills.ToListAsync();
            Teacher dbteacher = _db.Teacher.Include(x => x.Profession).Include(x=>x.SocialMedia).Include(x => x.Teacherdetails).FirstOrDefault(x => x.Id == id);

            if (dbteacher == null)
            {
                return NotFound();
            }

            if (teacher.Photo != null)
            {
                if (!teacher.Photo.isImage())
                {
                    ModelState.AddModelError("Photo", "Zehmet olmasa sekil secin!");
                    return View(dbteacher);

                }
                if (teacher.Photo.isLower4mb())
                {
                    ModelState.AddModelError("Photo", "Zehmet olmasa 4mb kecmeyin!");
                    return View(dbteacher);
                }
                if (!ModelState.IsValid)
                {
                    return View(dbteacher);
                }
                string folder = Path.Combine(_env.WebRootPath, "img", "teacher");
                teacher.Image = await teacher.Photo.savefileAsync(folder);
                dbteacher.Image = teacher.Image;
            }
           
            bool exist2 = _db.Teacher.Any(x => x.Name == teacher.Name && x.Id != id);
            if (exist2)
            {
                ModelState.AddModelError("Name", "Bu ad movcuddur");
                return View(dbteacher);
            }
            List<TeacherSkillsmany> teacherSkillsmany = new List<TeacherSkillsmany>();
            foreach (int item in skillid)
            {
                TeacherSkillsmany teacherskillsmany = new TeacherSkillsmany();
                teacherskillsmany.TeacherId = teacher.Id;
                teacherskillsmany.TeacherSkillsId = item;
                teacherSkillsmany.Add(teacherskillsmany);
            }

            dbteacher.Teacherdetails.Hobbies = teacher.Teacherdetails.Hobbies;
            dbteacher.Teacherdetails.Experience = teacher.Teacherdetails.Experience;
            dbteacher.Teacherdetails.Description = teacher.Teacherdetails.Description;
            dbteacher.Teacherdetails.Degree = teacher.Teacherdetails.Degree;
            dbteacher.Teacherdetails.Email = teacher.Teacherdetails.Email;
            dbteacher.Teacherdetails.Faculty = teacher.Teacherdetails.Faculty;
            dbteacher.Teacherdetails.PhoneNumber = teacher.Teacherdetails.PhoneNumber;
            dbteacher.Teacherdetails.Scype = teacher.Teacherdetails.Scype;
            dbteacher.Teacherdetails.SubTitle = teacher.Teacherdetails.SubTitle;
            dbteacher.Teacherdetails.Title = teacher.Teacherdetails.Title;
            dbteacher.SocialMedia.Facebook = teacher.SocialMedia.Facebook;
            dbteacher.SocialMedia.Twitter = teacher.SocialMedia.Twitter;
            dbteacher.SocialMedia.Vimeo = teacher.SocialMedia.Vimeo;
            dbteacher.SocialMedia.Pinterest = teacher.SocialMedia.Pinterest;
            dbteacher.SocialMedia.TeacherId = teacher.Id;
            dbteacher.Name = teacher.Name;
            dbteacher.Teacherdetails.TeacherId = teacher.Id;
            dbteacher.TeacherSkillsmanys = teacherSkillsmany;
            dbteacher.ProfessionId = (int)ProfesId;
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");


        }
        #endregion

    }


}
