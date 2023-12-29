using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Services.Email
{
    public class EmailSender : IEmailSender
    {
        public async Task<bool> SendEmailAsync(string To, string Title, string Body)
        {
            try
            {
                string From = "tickettinggroup@yahoo.com";
                SmtpClient client = new SmtpClient("smtp.mail.yahoo.com", 25);
                client.Port = 587;
                client.Host = "smtp.mail.yahoo.com";
                client.EnableSsl = true;
                client.Timeout = 1000000;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;

                client.Credentials = new NetworkCredential(From, "bhedkhjtszmogxsp");
                var message = new MailMessage(From, To, Title, Body);
                
                message.IsBodyHtml = true;
                message.BodyEncoding = UTF8Encoding.UTF8;
                message.DeliveryNotificationOptions = DeliveryNotificationOptions.OnSuccess;
                await client.SendMailAsync(message);
                return true;
            }
            catch (Exception ex)
            {
                var message = ex.Message;
                var data = ex.Data;
                var mes = ex.Source;

                return false;
            }
        }
    }
}
