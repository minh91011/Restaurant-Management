using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Mail;
using System.Net;

namespace RestaurantManagement.Pages.Sendmail
{
    public class IndexModel : PageModel
    {
        private const string sendingEmail = "sonnvbhe161489@fpt.edu.vn";
        private const string sendingEmailPassword = "ehcy ilad zkyt zhpm"; // mail password, if use 2 factor auth, this will be app password
        private const string smtpServer = "smtp.gmail.com"; // smtp server for gmail
        string OTPcode; // OTP code ..duh

        public void OnGet()
        {
        }

        public async Task  OnPost()
        {
            try
            {
                using (SmtpClient smtpClient = new SmtpClient(smtpServer))
                {
                    smtpClient.Port = 587;
                    smtpClient.Credentials = new NetworkCredential(sendingEmail, sendingEmailPassword);
                    smtpClient.EnableSsl = true;

                    MailMessage mail = new MailMessage();
                    mail.From = new MailAddress(sendingEmail);

                    // Create OTP code
                    Random random = new Random();
                    for(int i = 0; i < 5; i++)
                    {
                        OTPcode += random.Next(10);
                    }


                    //Create mail to send

                    mail.To.Add(Request.Form["Email"]);
                    mail.Subject = "Sending code for restaurant manager";
                    mail.Body = "Your OTP code " + OTPcode;

                    await  smtpClient.SendMailAsync(mail);
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Error: {ex.Message}");
            }

        }
    }
}
