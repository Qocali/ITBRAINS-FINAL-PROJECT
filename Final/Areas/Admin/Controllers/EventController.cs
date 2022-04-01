using Final.DAL;
using Final.Extentions;
using Final.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MailKit;
using MimeKit;
using System.Web.Helpers;
using Microsoft.AspNetCore.Authorization;

namespace Final.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class EventController : Controller
    {
        #region DbContext
        private readonly AppDbContext _db;
        private readonly IWebHostEnvironment _env;
        private int id;

        public EventController(AppDbContext db, IWebHostEnvironment env)
        {
            _db = db;
            _env = env;
        }
        #endregion
        #region Index
        public async Task<IActionResult> Index(int page = 1)
        {
            ViewBag.Page = page;
            ViewBag.Pagecount = Math.Ceiling((decimal)_db.Events.Count() / 8);
            List<Events> events = await _db.Events.OrderByDescending(x => x.Id).Skip((page - 1) * 8).Take(8).Include(x => x.EventSpeaker).ThenInclude(x => x.Speaker).ToListAsync();


            return View(events);
        }
        #endregion
        #region Activate
        public async Task<IActionResult> Active(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Events dbevent = _db.Events.FirstOrDefault(x => x.Id == id);
            if (dbevent == null)
            {
                return NotFound();
            }
            if (dbevent.IsDeactive)
            {
                dbevent.IsDeactive = false;
            }
            else
            {
                dbevent.IsDeactive = true;
            }
            _db.SaveChanges();
            return RedirectToAction("Index");

        }
        #endregion
        #region Create
        public async Task<IActionResult> Create()
        {
            ViewBag.Speaker = _db.Speakers.Where(x => !x.IsDeactive).ToList();
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Events events, int[] speakerid)
        {

            ViewBag.Speaker = _db.Speakers.Where(x => !x.IsDeactive).ToList();
            if (!ModelState.IsValid)
            {
                return View();
            }
            bool exist = _db.Events.Any(x => x.EventName == events.EventName);
            if (exist)
            {
                ModelState.AddModelError("EventName", "Bu event movcuddur");
                return View();
            }
            if (events.Photo == null)
            {
                ModelState.AddModelError("Photo", "Zehmet olmasa sekil elave edin");
                return View();
            }
            if (!events.Photo.isImage())
            {
                ModelState.AddModelError("Photo", "Zehmet olmasa Sekil secin!");
                return View();

            }
            if (events.Photo.isLower4mb())
            {
                ModelState.AddModelError("Photo", "Zehmetr olmasa 4mb kecmeyin!");
                return View();
            }
            if (!ModelState.IsValid)
            {
                return View();
            }
           

            string folder = Path.Combine(_env.WebRootPath, "img", "event");
            events.Image = await events.Photo.savefileAsync(folder);
            List<EventSpeaker> EventSpeaker = new List<EventSpeaker>();
            foreach (int item in speakerid)
            {
                EventSpeaker Eventspeaker = new EventSpeaker();
                Eventspeaker.EventId = events.Id;
                Eventspeaker.SpeakerId = item;
                EventSpeaker.Add(Eventspeaker);
            }
            events.EventSpeaker = EventSpeaker;
            await _db.Events.AddAsync(events);
            await _db.SaveChangesAsync();
            string subject = "New Event";
            var message = $"<p>Tedbirin adi : {events.EventName}</p></br> <p> Vaxti : {events.Start.ToString("dd mm HH-")}{events.Finish.ToString("HH")}</p></br> <p> unvani : {events.Venue}</p>";
            foreach (Subscribe subscribe in _db.subscribes)
            {
                await Helper.SendMessage(subject, message, subscribe.Email);
            }
            return RedirectToAction("Index");
        }
        #endregion
        #region Update
        public async Task<IActionResult> Update(int? id)
            
        {
            if (id == null)
            {
                return NotFound();
            }
            ViewBag.Speaker = await _db.Speakers.Include(x=>x.EventSpeaker).Where(x => !x.IsDeactive).ToListAsync();
            Events dbevent =await _db.Events.Include(x => x.EventSpeaker).ThenInclude(x => x.Speaker).FirstOrDefaultAsync(x => x.Id == id);
            if (dbevent == null)
            {
                return NotFound();
            }
            return View(dbevent);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Update(Events events, int? id, int[] speakerid)
        {
            ViewBag.Speaker = _db.Speakers.Include(x=>x.EventSpeaker).Where(x=>!x.IsDeactive).ToList();
            Events dbevent = _db.Events.Include(x => x.EventSpeaker).ThenInclude(x => x.Speaker).FirstOrDefault(x => x.Id == id);

            if (dbevent == null)
            {
                return NotFound();
            }

            if (events.Photo != null)
            {
                if (!events.Photo.isImage())
                {
                    ModelState.AddModelError("Photo", "Zehmet olmasa sekil secin!");
                    return View(dbevent);

                }
                if (events.Photo.isLower4mb())
                {
                    ModelState.AddModelError("Photo", "Zehmet olmasa 4mb kecmeyin!");
                    return View(dbevent);
                }
                if (!ModelState.IsValid)
                {
                    return View(dbevent);
                }
                string folder = Path.Combine(_env.WebRootPath, "img", "event");
                events.Image = await events.Photo.savefileAsync(folder);
                dbevent.Image = events.Image;
            }
           
            bool exist2 = _db.Events.Any(x => x.EventName == events.EventName && x.Id != id);
            if (exist2)
            {
                ModelState.AddModelError("EventName", "Bu ad movcuddur");
                return View(dbevent);
            }

            List<EventSpeaker> eventSpeakers = new List<EventSpeaker>();
            foreach (int item in speakerid)
            {
                EventSpeaker EventSpeaker = new EventSpeaker();
                EventSpeaker.EventId = events.Id;
                EventSpeaker.SpeakerId = item;
                eventSpeakers.Add(EventSpeaker);
            }
            dbevent.EventName = events.EventName;
            dbevent.Start = events.Start;
            dbevent.Venue = events.Venue;
            dbevent.Date = events.Date;
            dbevent.Finish = events.Finish;
            dbevent.EventSpeaker = eventSpeakers;
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");


        }
        #endregion

        #region Send message to subscribe email
        public IActionResult Sendmail()
        {
            MimeMessage message = new MimeMessage();
            message.From.Add(new MailboxAddress("Devloper", "babayevqocheli@gmail.com"));
            foreach (Subscribe item in _db.subscribes)
            {
                message.To.Add(MailboxAddress.Parse(item.Email));
                message.Body = new TextPart("plain")
                {
                    Text = @"a body for email an email...you can write any thing..."
                };
                message.Subject = "Yeni tEdbir var istirak etmek istermisin?";
                string email = "babayevqocheli@gmail.com";
                string password = "Psg+1970";
                SmtpClient client = new SmtpClient();
                client.Connect("smtp.gmail.com", 465, true);
                client.Authenticate(email, password);
                client.Send(message);


            }

            return RedirectToAction("Index");
        }
        #endregion
    }
}
