using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using DocumentApprovalSystem.Web.Pages;
using DocumentApprovalSystem.Data;
using DocumentApprovalSystem.Lib;

namespace DocumentApprovalSystem.Web.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class LoginModel : SysPageModel
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly ILogger<LoginModel> _logger;

        public LoginModel(SignInManager<User> signInManager, UserManager<User> userManager, ILogger<LoginModel> logger)
        {
            _signInManager = signInManager;
            _logger = logger;
            _userManager = userManager;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public string ReturnUrl { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        public class InputModel
        {
            [Required]
            public string Email { get; set; }

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [Display(Name = "Keep me signed in")]
            public bool RememberMe { get; set; }
        }
        public async Task OnGetAsync(string returnUrl = null, string code = null)
        {
            BreadCrumb.Items.Clear();
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }

            returnUrl ??= Url.Content("~/Config");
            Title  = "Login";
            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            ReturnUrl = returnUrl;

            if (code != null)
            {
                var uid = new Guid(Convert.FromBase64String(code));
                var user = await Db.Users.FindAsync(uid);
                Input = new InputModel { Email = user.LoginId };
            }
        }

        #region snippet
        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");

            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(Input.Email, Input.Password, Input.RememberMe, lockoutOnFailure: false);
                if (!result.Succeeded && !Db.Users.Any())
                {
                    await CreateSuperAdmin();
                    result = await _signInManager.PasswordSignInAsync(Input.Email, Input.Password, Input.RememberMe, lockoutOnFailure: false);
                }
                if (result.Succeeded)
                {
                    var user = await _userManager.FindByNameAsync(Input.Email);
                    user.LastLoginDate = DateTime.Now;
                    //user.UserSession.Add(new UserSession
                    //{
                    //    ClientIpaddress = Request.Host.Host,
                    //    LoginDate = user.LastLoginDate.Value,
                    //});
                    await _userManager.UpdateAsync(user);
                    _logger.LogInformation("User logged in.");                   
                    return LocalRedirect(returnUrl);
                }
                if (result.RequiresTwoFactor)
                {
                    return RedirectToPage("./LoginWith2fa", new
                    {
                        ReturnUrl = returnUrl,
                        Input.RememberMe
                    });
                }
                if (result.IsLockedOut)
                {
                    _logger.LogWarning("User account locked out.");
                    return RedirectToPage("./Lockout");
                }
                else
                {
                    ModelState.AddModelError("Input.Email", "Invalid login attempt.");
                    return Page();
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
        #endregion

        async Task CreateSuperAdmin()
        {
            var user = new User
            {
                Id = Guid.NewGuid(),
                Name = "Administrator",
                Email = Input.Email.ToLower().Trim(),
                LoginId = Input.Email.ToLower().Trim(),
                CreationDate = DateTime.Now,
                SecurityStamp = Guid.NewGuid().ToString(),
                IsActive = true,
                IsEmailConfirmed = true,
                Role = UserRole.ADMIN,
                ActivationDate = DateTime.Now,
            };
            Db.Users.Add(user);
            user.PasswordHash = _userManager.PasswordHasher.HashPassword(user, Input.Password);
            await Db.SaveChangesAsync();
        }
    }
}
