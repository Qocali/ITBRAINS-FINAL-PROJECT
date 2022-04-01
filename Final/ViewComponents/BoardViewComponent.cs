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
    public class BoardViewComponent : ViewComponent
    {
        private readonly AppDbContext _db;
        public BoardViewComponent(AppDbContext db)
        {
            _db = db;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            List<Board> boards = await _db.Boards.ToListAsync();
            return View(boards);


        }
    }
}
