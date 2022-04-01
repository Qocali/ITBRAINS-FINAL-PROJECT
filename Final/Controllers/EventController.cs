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
    public class EventController : Controller
    {
        #region DbPart
        private readonly AppDbContext _db;
        public EventController(AppDbContext db)
        {
            _db = db;
        }
        #endregion
        #region Index
        public async Task<IActionResult> Index(int page=1)
        {

            ViewBag.Page = page;
            ViewBag.Pagecount = Math.Ceiling((decimal)_db.Events.Count() /6);
            List<Events> events = await _db.Events.OrderByDescending(x => x.Id).Skip((page - 1) * 6).Where(x => !x.IsDeactive).Take(6).ToListAsync();
            
            return View(events);
        }
        #endregion

        #region Detail Event
        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Events events = await _db.Events.Include(x => x.EventSpeaker).ThenInclude(x => x.Speaker).FirstOrDefaultAsync(x => x.Id == id);
            return View(events);
        }
        #endregion
        #region EventSearch
        public async Task<IActionResult> Searchevent(string search)
        {

            List<Events> events= await _db.Events.Where(x => x.EventName.Contains(search)).Include(x => x.EventSpeaker).ThenInclude(x=>x.Speaker).ToListAsync();
            if (search == null)
            {
                List<Events> events1 = await _db.Events.Include(x => x.EventSpeaker).ThenInclude(x => x.Speaker).ToListAsync();
                return PartialView("_SearchEventPartial", events1);
            }
            return PartialView("_SearchEventPartial", events);
        }
        #endregion

    }
}
