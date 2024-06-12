using DocumentApprovalSystem.Data;
using DocumentApprovalSystem.Lib;
using DocumentApprovalSystem.Web.Auth;
using DocumentApprovalSystem.Web.Pages;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DocumentApprovalSystem.Web.Areas.Users.Pages;

[AccessRight(AccessRight.MANAGE_USERS)]
public class EditModel : SysPageModel
{

    [BindProperty]
    public User SelectedUser { get; private set; }
    public async Task OnGetAsync(Guid ID)
    {
        SelectedUser = await Db.Users.FirstAsync(c => c.Id == ID);
        Title = $"Edit user: {SelectedUser.Name}";
        PageTitle = SelectedUser.Name;
        BreadCrumb.Add(SelectedUser.Name, "./Details", new { ID });
        BreadCrumb.Add("Edit", ".", new { ID });
    }

    public async Task<IActionResult> OnPostAsync(Guid ID)
    {
        SelectedUser = await Db.Users.FirstAsync(c => c.Id == ID);
        if (await TryUpdateModelAsync(SelectedUser, "", p => p.Name, p => p.Email, p => p.Role, p => p.IsActive, p => p.Mobile))
        {
            await Db.SaveChangesAsync();
            return RedirectToPage("./Details", new { SelectedUser.Id });
        }
        return Page();
    }
}