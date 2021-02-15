using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Net.Mail;
using System.Net;

namespace SeminaryLibrary.Libraries
{
    public class EmailLibrary
    {
        internal static void ReserveBooks(List<string> BooksBooked, SemCurrentUser cu, int count, string ToAddress, string FromAddress, string mailSrvUser, string mailSrvPass, string mailSrvIP)
        {
            MailMessage msg = new MailMessage();
            msg.From = new MailAddress(FromAddress, "Seminary Online Booking");
            msg.To.Add(new MailAddress(ToAddress, cu.UsernameFormatted));
            msg.ReplyToList.Add( new MailAddress(cu.Email));
            msg.Subject = String.Format("{0} Books reserved", count);

            StringBuilder body = new StringBuilder();
            body.AppendLine(String.Format("<h2>Booking by: {0} on {1}</h2>", cu.UsernameFormatted, DateTime.Now.ToShortDateString()));
            body.AppendLine(String.Format("<h3>{0} Books booked : </h3>", count));
            body.AppendLine(String.Format("<p></p>", count));
            body.AppendLine(String.Format("<ul>"));
            foreach (string book in BooksBooked)
            {
                body.AppendLine(String.Format("<li>{0}</li>", book));
            }

            body.AppendLine(String.Format("</ul>"));

            body.AppendLine(String.Format("<br /><br />", count));
            body.AppendLine(String.Format("<h3>Details on client: </h3>"));

            body.AppendLine(String.Format("<ul>"));
            body.AppendLine(String.Format("<li>Full Name : {0}</li>", cu.UsernameFormatted));
            body.AppendLine(String.Format("<li>Email : {0}</li>", cu.Email));
            body.AppendLine(String.Format("<li>Seminary Id : {0}</li>", cu.UserId));
            body.AppendLine(String.Format("<li>Client Class : {0}</li>", cu.SeminaryYr));


            body.AppendLine(String.Format("</ul>"));

            msg.Body = body.ToString();

            msg.IsBodyHtml = true;

            SmtpClient smtp = new SmtpClient(mailSrvIP);
            smtp.Credentials = new NetworkCredential(mailSrvUser, mailSrvPass);
            try
            {
                smtp.Send(msg);
            }

            catch
            {
            }
        }
    }
}