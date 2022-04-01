using Final.DAL;
using Final.Models;
using Final.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Final.ViewComponents
{
    public class HeaderViewComponent:ViewComponent
    {
        private readonly AppDbContext _db;
        public HeaderViewComponent(AppDbContext db)
        {
            this._db = db;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            Bio bio = await _db.bios.FirstOrDefaultAsync();
            return View(bio);
        }
    }

}

