using DocumentApprovalSystem.Data;
using DocumentApprovalSystem.Web.Models;
using DocumentApprovalSystem.Web.Pages;
using wCyber.Helpers.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using wCyber.Helpers.Identity;

namespace DocumentApprovalSystem.Web.Areas.Users.Pages;
public class DetailsModel : SysPageModel
{
    public User SelectedUser { get; private set; }

    public bool IsCurrentUser => SelectedUser.Id == CurrentUser.Id;

    public async Task OnGetAsync(Guid? ID)
    {
        if (ID == null) ID = CurrentUserId;
        SelectedUser = await Db.Users
            .FirstAsync(c => c.Id == ID);
        Title = $"User details: {SelectedUser.Name}";
        PageTitle = SelectedUser.Name;
        ActionBar.Add("Edit..", "./Edit", new { ID }, icon: "fa fa-plus");
        if (User.IsTechSupport())
        {
            if (!IsCurrentUser && SelectedUser.IsEmailConfirmed)
            {
                if (SelectedUser.IsActive)
                {
                    ActionBar.Add("Deactivate..", "./Edit", new { ID }, handler: "Status", PageActionBar.PageActionType.REJECT, icon: "fa fa-lock");
                }
                else
                {
                    ActionBar.Add("Activate..", "./Edit", new { ID }, handler: "Status", PageActionBar.PageActionType.PRIMARY, icon: "fa fa-lock-open");
                }
            }
        }
        BreadCrumb.Add(SelectedUser.Name, ".", new { ID });
    }

    public async Task<IActionResult> OnGetStatusAsync(Guid ID)
    {
        //if (!User.IsAdmin()) return Unauthorized();
        SelectedUser = await Db.Users.FindAsync(ID);
        SelectedUser.IsActive = !SelectedUser.IsActive;
        await Db.SaveChangesAsync();
        return RedirectToPage("./Details", new { ID });
    }

    public async Task<IActionResult> OnGetSendActivationAsync(Guid? Id)
    {
        if (Id == null) Id = CurrentUser.Id;
        EmailSender EmailSender = null;
        var config = await Db.EmailConfigs.FirstOrDefaultAsync();//(c => c.IsActive && (c.TargetId & (int)EmailConfigTarget.LOGIN_ACCOUNT_MANAGEMENT) == (int)EmailConfigTarget.LOGIN_ACCOUNT_MANAGEMENT);
        if (config != null) EmailSender = new EmailSender(config.GetOptions());

        SelectedUser = await Db.Users
            .FirstAsync(c => c.Id == Id);
        await SelectedUser.SendActivationEmail(EmailSender, HttpContext, Url, true);
        return RedirectToPage("./Details", new { Id });
    }


}