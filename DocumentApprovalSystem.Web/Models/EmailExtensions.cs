using DocumentApprovalSystem.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using wCyber.Helpers.Identity;

namespace DocumentApprovalSystem.Web.Models
{
    public static class EmailExtensions
    {
        public static async Task SendActivationEmail(this User user, EmailSender emailSender, HttpContext context, IUrlHelper Url, bool sendAsync = true, bool IsReset = false)
        {
            if (emailSender == null) return;

            var _userManager = context.RequestServices.GetService<UserManager<User>>();
            var code = await _userManager.GeneratePasswordResetTokenAsync(user);
            var url = Url.Page(
                "/Account/ResetPassword",
                pageHandler: null,
                values: new { code, email = user.LoginId, area = "Identity" },
                protocol: context.Request.Scheme);

            var HostEnvironment = context.RequestServices.GetService<IWebHostEnvironment>();
            string Body = File.ReadAllText(Path.Combine(HostEnvironment.WebRootPath, "EmailTemplates", "AccountActivation.html"));
            Body = Body.Replace("[GREETING]", $"Dear {user.Name}");
            Body = Body.Replace("[URL]", url);
            Body = Body.Replace("[USERNAME]", user.Email);
            if (sendAsync)
            {
                new Task(async () =>
                {
                    try
                    {
                        await emailSender.SendEmailAsync(user.Email, "Zengeza 1 High School account activation", Body);
                    }
                    catch (Exception E)
                    {

                        Console.WriteLine(E.StackTrace);
                    }
                }).Start();
            }
            else
            {
                emailSender.SendEmail(user.Email, "wCyber Leave Portal account activation", Body);
            }
        }



        public static void SendLeaveEmail(this DocRequest DocRequest, EmailSender emailSender, HttpContext context, bool sendAsync = true, string Content = null, string To = null, string btnText = null, string url = null)
        {
            var Customer = DocRequest.Employee?.FirstName;
            string Subject = "Application acknowledgement";
            string to = DocRequest.Employee?.IdNavigation.Email ?? To;
            var HostEnvironment = context.RequestServices.GetService<IWebHostEnvironment>();
            string Body = System.IO.File.ReadAllText(System.IO.Path.Combine(HostEnvironment.WebRootPath, "EmailTemplates", "EmailNotification.html"));
            Body = Body.Replace("[NAME]", Customer);
            Body = Body.Replace("[BODY]", Content);
            Body = Body.Replace("[BUTTON]", "view your application");
            Body = Body.Replace("[HEADER]", Subject);
            if (sendAsync)
            {
                new Task(async () =>
                {
                    try
                    {
                        await emailSender.SendEmailAsync(to, Subject, Body);
                    }
                    catch (Exception E)
                    {

                        Console.WriteLine(E.StackTrace);
                    }
                }).Start();
            }
            else
            {
                emailSender.SendEmail(to, Subject, Body);
            }
        }
    }
}
