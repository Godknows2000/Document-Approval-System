using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using DocumentApprovalSystem.Data;
using Microsoft.EntityFrameworkCore;
using DocumentApprovalSystem.Web.Pages;
using wCyber.Helpers.Identity;
using DocumentApprovalSystem.Lib;
using DocumentApprovalSystem.Web.Models;

namespace DocumentApprovalSystem.Web.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class ForgotPasswordModel : SysPageModel
    {
        private readonly UserManager<User> _userManager;
        private readonly IEmailSender _emailSender;
        public ForgotPasswordModel(UserManager<User> userManager, IEmailSender emailSender)
        {
            _userManager = userManager;
            _emailSender = emailSender;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                EmailSender EmailSender = null;
                var SelectedUser = await Db.Users
                    .FirstOrDefaultAsync(c => c.Email == Input.Email);
                if (SelectedUser == null)
                {
                    return Page();
                }

                var config = await Db.EmailConfigs.FirstOrDefaultAsync();//(c => c.IsActive && (c.TargetId & (int)EmailConfigTarget.LOGIN_ACCOUNT_MANAGEMENT) == (int)EmailConfigTarget.LOGIN_ACCOUNT_MANAGEMENT);
                if (config != null) EmailSender = new EmailSender(config.GetOptions());


                await SelectedUser.SendActivationEmail(EmailSender, HttpContext, Url, sendAsync: true, IsReset: true);
                SelectedUser.ActivationDate = null;
                await Db.SaveChangesAsync();
                return RedirectToPage("./ForgotPasswordConfirmation");
            }
            return Page();
        }
    }
}
