using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Final.Extentions
{
    public static class Helper
    {
        public static async Task SendMessage(string messageSubject, string messageBody, string mailTo)
        {

            SmtpClient client = new SmtpClient("smtp.yandex.com", 587);
            client.UseDefaultCredentials = false;
            client.EnableSsl = true;
            client.Credentials = new NetworkCredential("qocheli.b@itbrains.az", "qloxoopawkzomvmc");
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            MailMessage message = new MailMessage("qocheli.b@itbrains.az", mailTo);
            message.Subject = messageSubject;
            message.Body = messageBody;
            message.BodyEncoding = System.Text.Encoding.UTF8;
            message.IsBodyHtml = true;


            await client.SendMailAsync(message);


        }
        public enum Role
        {
            Admin,
            Member,
            Mederator
        };
    }
}
