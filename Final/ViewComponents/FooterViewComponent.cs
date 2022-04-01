using Final.DAL;
using Final.Models;
using Final.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Final.ViewModels;

namespace Final.ViewComponents
{
    public class FooterViewComponent:ViewComponent
    {
        private readonly AppDbContext _db;
        public FooterViewComponent(AppDbContext db)
        {
            this._db = db;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            HomeVM homeVM = new HomeVM 
            {
             Bios= await _db.bios.FirstOrDefaultAsync()
        };
           
            return View(homeVM);
        }
    }
}
