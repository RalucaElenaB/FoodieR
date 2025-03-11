using FoodieR.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;
using System.Net;

namespace FoodieR.Controllers;

public class ContactController : Controller
{
    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    public IActionResult SendMessage(ContactFormModel model)
    {
        if (ModelState.IsValid)
        {
            try
            {
                var fromAddress = new MailAddress("your-email@example.com", "FoodieR Support");
                var toAddress = new MailAddress("support@foodier.com", "Support Team");
                const string fromPassword = "your-email-password";
                string subject = model.Subject;
                string body = $"Name: {model.Name}\nEmail: {model.Email}\nMessage: {model.Message}";

                var smtp = new SmtpClient
                {
                    Host = "smtp.gmail.com",
                    Port = 587,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
                };

                using (var message = new MailMessage(fromAddress, toAddress)
                {
                    Subject = subject,
                    Body = body
                })
                {
                    smtp.Send(message);
                }

                TempData["Success"] = "Your message has been sent successfully!";
            }
            catch
            {
                TempData["Error"] = "An error occurred while sending the message. Please try again later.";
            }
        }
        return RedirectToAction("Index");
    }
}
