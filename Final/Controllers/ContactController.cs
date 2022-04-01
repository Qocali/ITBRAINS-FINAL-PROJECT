using Final.DAL;
using Final.Models;
using Microsoft.AspNetCore.Mvc;
using MailKit.Net.Smtp;
using MailKit;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace EduHomeFinal.Controllers
{
    public class ContactController : Controller
    {
        #region DbPart
        private readonly AppDbContext _db;
        public ContactController(AppDbContext db)
        {
            _db = db;
        }
        #endregion
        #region Index
        public IActionResult Index()
        {
            return View();
        }
        #endregion

        #region When You Are Want To Create Contact Used This Action
        public ContentResult Send(string message, string email, string subject, string name)
        {
            if (email == null)
            {
                return Content("empty e-mail please enter");
            }
            MimeMessage Message = new MimeMessage();
            Message.From.Add(new MailboxAddress("Devloper", email));
            Message.To.Add(MailboxAddress.Parse("babayevqocheli@gmail.com"));
            Message.Body = new TextPart("plain")
            {
                Text = message
            };
            Message.Subject = subject;
            string password = "Psg+1970";
            SmtpClient client = new SmtpClient();
            client.Connect("smtp.gmail.com", 465, true);
            client.Authenticate(email, password);
            client.Send(Message);

            return Content("Sending message to e-mail Succesfully");

        }
        #endregion
    }
}
