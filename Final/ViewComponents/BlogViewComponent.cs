using Final.DAL;
using Final.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Final.ViewComponents
{
    public class BlogViewComponent: ViewComponent
    {
        private readonly AppDbContext _db;
        public BlogViewComponent(AppDbContext db)
        {
            _db = db;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            List<Blog> blogs = await _db.Blogs.OrderByDescending(x => x.Id).Take(3).ToListAsync();
            return View(blogs);
        }
    }
}
