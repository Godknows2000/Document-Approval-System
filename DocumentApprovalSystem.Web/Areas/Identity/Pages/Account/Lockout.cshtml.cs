using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using DocumentApprovalSystem.Data;

namespace DocumentApprovalSystem.Web.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class LockoutModel : PageModel
    {
        private readonly SignInManager<User> _signInManager;

        public LockoutModel(SignInManager<User> signInManager )
        {
            _signInManager = signInManager;
        }

        public async Task OnGet()
        {
            if(User.Identity.IsAuthenticated)
                await _signInManager.SignOutAsync();

        }
    }
}
