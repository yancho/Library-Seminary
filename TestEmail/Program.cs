using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;

using System.Net;
using System.Configuration;

namespace TestEmail
{
    class Program
    {
        static void Main(string[] args)
        {
            string FromAddress = ConfigurationManager.AppSettings["libraryFromAddress"];
            
            string ToAddress =       ConfigurationManager.AppSettings["libraryToAddress"];

            string mailSrvUser =          ConfigurationManager.AppSettings["mailSrvUser"];
            
            string mailSrvPass = ConfigurationManager.AppSettings["mailSrvPass"];

            string mailSrvIP = ConfigurationManager.AppSettings["mailSrvIP"];

            MailMessage msg = new MailMessage();
            msg.From = new MailAddress(FromAddress, "Seminary Online Booking");
            msg.To.Add(new MailAddress(ToAddress, "test name"));
            msg.ReplyToList.Add(new MailAddress(ToAddress));
            msg.Subject = String.Format("Testing Mailserver");

            StringBuilder body = new StringBuilder();
            body.AppendLine(String.Format("<h2>Test {0} </h2>",DateTime.Now.ToLongDateString()));
            body.AppendLine(String.Format("<h2>Test From {0} </h2>", System.Security.Principal.WindowsIdentity.GetCurrent().Name.ToString()));


            msg.Body = body.ToString();

            msg.IsBodyHtml = true;

            SmtpClient smtp = new SmtpClient(mailSrvIP);
            smtp.Credentials = new NetworkCredential(mailSrvUser, mailSrvPass);

            try
            {
                smtp.Send(msg);
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            
        }
    }
}
