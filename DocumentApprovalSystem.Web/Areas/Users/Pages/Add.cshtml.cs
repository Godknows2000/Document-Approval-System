using DocumentApprovalSystem.Data;
using DocumentApprovalSystem.Lib;
using DocumentApprovalSystem.Web.Auth;
using DocumentApprovalSystem.Web.Pages;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DocumentApprovalSystem.Web.Areas.Users.Pages;
[AccessRight(AccessRight.MANAGE_USERS)]
public class AddModel : SysPageModel
{

    [BindProperty]
    public User NewUser { get; set; } = null!;

    public void OnGet(Guid? Id)
    {
        Title = PageTitle = "Add new user..";
        BreadCrumb.Add("Add");

    }

    public async Task<IActionResult> OnPostAsync(Guid? Id)
    {
        NewUser.LoginId = NewUser.Email.ToLower().Trim();
        NewUser.Email = NewUser.LoginId;
        var duplicateUser = Db.Users.FirstOrDefault(c => c.LoginId == NewUser.LoginId);
        if (duplicateUser != null)
        {
            Title = PageTitle = "Add new user..";
            ModelState.AddModelError($"{nameof(NewUser)}.{nameof(NewUser.LoginId)}", "A user with the same Login Id already exists!");
            return Page();
        }
        NewUser.Id = Guid.NewGuid();
        NewUser.CreationDate = DateTime.Now;
        NewUser.SecurityStamp = Guid.NewGuid().ToString();
        NewUser.CreatorId = CurrentUser.Id;
        Db.Users.Add(NewUser);
        await Db.SaveChangesAsync();
        return RedirectToPage("./Details", new { NewUser.Id });
    }
}