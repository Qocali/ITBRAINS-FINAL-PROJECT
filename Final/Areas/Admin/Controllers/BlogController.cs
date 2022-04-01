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
    [Authorize(Roles = "Admin")]
    public class BlogController : Controller
    {
        #region databasepart
        private readonly AppDbContext _db;
        private readonly IWebHostEnvironment _env;
        private int id;

        public BlogController(AppDbContext db, IWebHostEnvironment env)
        {
            _db = db;
            _env = env;
        }
        #endregion
        #region Index
        public async Task<IActionResult> Index(int page = 1)
        {
            ViewBag.Page = page;
            ViewBag.Pagecount = Math.Ceiling((decimal)_db.Blogs.Count() / 8);
            List<Blog> blogs = await _db.Blogs.OrderByDescending(x => x.Id).Skip((page - 1) * 8).Take(8).Include(x => x.BlogDetail).ToListAsync();


            return View(blogs);
        }
        #endregion
        #region Activate Blog or inverse
        public async Task<IActionResult> Active(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Blog dbblogs = await _db.Blogs.FirstOrDefaultAsync(x => x.Id == id);
            if (dbblogs == null)
            {
                return NotFound();
            }
            if (dbblogs.IsDeactive)
            {
                dbblogs.IsDeactive = false;
            }
            else
            {
                dbblogs.IsDeactive = true;
            }
            _db.SaveChanges();
            return RedirectToAction("Index");

        }
        #endregion
        #region Create new Blog
        public async Task<IActionResult> Create()
        {
            ViewBag.blog = await _db.Blogs.ToListAsync();
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Blog blog)
        {
            ViewBag.blog = await _db.Blogs.ToListAsync();
            if (!ModelState.IsValid)
            {
                return View();
            }

            if (blog.Photo == null)
            {
                ModelState.AddModelError("Photo", "Zehmet olmasa sekil elave edin");
                return View();
            }
            if (!blog.Photo.isImage())
            {
                ModelState.AddModelError("Photo", "zehmet olmasa sekil secin!");
                return View();

            }
            if (blog.Photo.isLower4mb())
            {
                ModelState.AddModelError("Photo", "Zehmet olmasa  4mb kecmeyin!");
                return View();
            }


            string folder = Path.Combine(_env.WebRootPath, "img", "blog");
            blog.Image = await blog.Photo.savefileAsync(folder);



            bool exist2 = _db.Blogs.Any(x => x.Creator == blog.Creator);
            if (exist2)
            {
                ModelState.AddModelError("Creator", "Bu Yaradici movcuddur");
                return View();
            }

            BlogDetail blogDetail = new BlogDetail();
            blogDetail.BlogId = blog.Id;
            blogDetail.Description = blog.BlogDetail.Description;
            blogDetail.Title = blog.BlogDetail.Title;
            blogDetail.SubTitle = blog.BlogDetail.SubTitle;
            blog.BlogDetail = blogDetail;
            await _db.Blogs.AddAsync(blog);
            await _db.SaveChangesAsync();

            return RedirectToAction("Index");
        }
        #endregion
        #region Update
        public IActionResult Update(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Blog dbblog = _db.Blogs.Include(x => x.BlogDetail).FirstOrDefault(x => x.Id == id);
            if(dbblog==null)
            {
                return NotFound();
            }
            return View(dbblog);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Update(Blog blog, int? id)
        {
           
            Blog dbblog = _db.Blogs.Include(x => x.BlogDetail).FirstOrDefault(x => x.Id == id);

            if (dbblog == null)
            {
                return NotFound();
            }

            if (blog.Photo != null)
            {
                if (!blog.Photo.isImage())
                {
                    ModelState.AddModelError("Photo", "Zehmet olmasa sekil secin!");
                    return View(dbblog);

                }
                if (blog.Photo.isLower4mb())
                {
                    ModelState.AddModelError("Photo", "Zehmet olmasa 4mb kecmeyin!");
                    return View(dbblog);
                }
                if (!ModelState.IsValid)
                {
                    return View(dbblog);
                }
                string folder = Path.Combine(_env.WebRootPath, "img", "blog");
                blog.Image = await blog.Photo.savefileAsync(folder);
                dbblog.Image = blog.Image;
            }
           
            bool exist2 = _db.Blogs.Any(x => x.Creator== blog.Creator && x.Id != id);
            if (exist2)
            {
                ModelState.AddModelError("Creator", "Bu Yaradici movcuddur");
                return View(dbblog);
            }

            
            dbblog.Creator =blog.Creator;
            dbblog.Idea = blog.Idea;
            dbblog.Time = blog.Time;
            dbblog.BlogDetail.BlogId = blog.Id;
            dbblog.BlogDetail.Description = blog.BlogDetail.Description;
            dbblog.BlogDetail.SubTitle = blog.BlogDetail.SubTitle;
            dbblog.BlogDetail.Title = blog.BlogDetail.Title;
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");


        }
        #endregion
    }
}
