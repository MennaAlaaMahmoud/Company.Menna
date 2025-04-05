using System.Net;
using System.Net.Mail;

namespace Company.Menna.PL.Helpers
{
    public static class EmailSettings
    {
       public static bool SendEmail(Email email )
        {
            // Mail Service : Gmail
            // SMTP
            try
            {
                var client = new SmtpClient("smtp.gmail.com.", 587);
                client.EnableSsl = true;
                client.Credentials = new NetworkCredential("menaalaa232203@gmail.com", "fpaputzxpzkueeth");
                client.Send("menaalaa232203@gmail.com", email.To, email.Subject, email.Body);


            }
            catch (Exception e)
            {
                return false;
            }

            return true;
        }

    }
}
