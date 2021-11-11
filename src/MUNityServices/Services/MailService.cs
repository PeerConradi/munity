using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace MUNity.Services
{
    public class MailService : IMailService
    {
        public void SendMail(string target, string title, string content)
        {
            var client = new SmtpClient("smtp.mailtrap.io", 2525)
            {
                Credentials = new NetworkCredential("056044a17ce7ee", "b8506b064d1b43"),
                EnableSsl = true
            };
            client.Send("no-reply@munity.org", target, title, content);
        }

    }
}
