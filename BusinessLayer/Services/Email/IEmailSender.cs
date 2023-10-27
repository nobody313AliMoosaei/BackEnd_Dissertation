using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Services.Email
{
    public interface IEmailSender
    {
        Task<bool> SendEmailAsync(string To, string Title, string Body);
    }
}
