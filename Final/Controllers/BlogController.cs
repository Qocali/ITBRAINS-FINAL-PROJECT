using Final.DAL;
using Final.Models;
using Final.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EduHomeFinal.Controllers
{
    public class BlogController : Controller
    {
        #region DbPart
        private readonly AppDbContext _db;
        public BlogController(AppDbContext db)
        {
            _db = db;
        }
        #endregion
        #region Index and Pagination Part
        public async Task<IActionResult> Index(int page = 1)
        {
            ViewBag.Page = page;
            ViewBag.Pagecount = Math.Ceiling((decimal)_db.Blogs.Count() /6);
            List<Blog> blogs = await _db.Blogs.OrderByDescending(x => x.Id).Skip((page - 1) * 6).Take(6).ToListAsync();
            return View(blogs);
        }
        #endregion
        #region Detail Part
        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Blog Blog = await _db.Blogs.Include(x => x.BlogDetail).FirstOrDefaultAsync(x => x.Id == id);

            return View(Blog);
        }
        #endregion
        
    }
}
