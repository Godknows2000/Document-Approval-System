using DocumentApprovalSystem.Data;
using DocumentApprovalSystem.Web.Pages;
using Microsoft.AspNetCore.Mvc;

namespace DocumentApprovalSystem.Web.Areas.Config.Pages.Email;

public class AddModel : SysPageModel
{

    [BindProperty]
    public EmailConfig NewEmailConfig { get; set; }

    public void OnGet()
    {
        Title = PageTitle = "Add new email config..";
        BreadCrumb.Add("Add");
    }

    public async Task<IActionResult> OnPost(int[] TargetIds)
    {
        NewEmailConfig.Id = Guid.NewGuid();
        NewEmailConfig.CreationDate = DateTime.Now;
        NewEmailConfig.CreatorId = CurrentUserId;
        NewEmailConfig.ComputeHash();
        if (TargetIds != null) NewEmailConfig.TargetId = TargetIds.Sum();
        Db.EmailConfigs.Add(NewEmailConfig);
        await Db.SaveChangesAsync();
        return RedirectToPage("./Details", new { NewEmailConfig.Id });
    }
}
