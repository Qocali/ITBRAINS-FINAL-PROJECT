using Final.DAL;
using Final.Models;
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

namespace EduHomeFinal.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class NoticeBoardController : Controller
    {
        #region DbContext
        private readonly AppDbContext _db;
        private readonly IWebHostEnvironment _env;


        public NoticeBoardController(AppDbContext db, IWebHostEnvironment env)
        {
            _db = db;
            _env = env;
        }
        #endregion

        #region Index
        public async Task<IActionResult> Index()
        {
            List<Board> boards = await _db.Boards.ToListAsync();
            return View(boards);
        }
        #endregion

        #region Create
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Board Board)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            Board board = new Board();
            board.Time = Board.Time;
            board.Title =Board.Title;
            await _db.Boards.AddAsync(Board);
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        #endregion

        #region Delete
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();
            Board board = await _db.Boards.FirstOrDefaultAsync(x => x.Id == id);
            if (board == null)
                return NotFound();
            _db.Boards.Remove(board);
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        #endregion
    }
}
