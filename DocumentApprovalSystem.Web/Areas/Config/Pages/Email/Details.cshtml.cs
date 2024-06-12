using DocumentApprovalSystem.Data;
using DocumentApprovalSystem.Web.Pages;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using wCyber.Helpers.Identity;
using wCyber.Helpers.Web;

namespace DocumentApprovalSystem.Web.Areas.Config.Pages.Email;

public class DetailsModel : SysPageModel
{
    public EmailConfig EmailConfig { get; set; }
    public string ErrorMessage { get; private set; }
    public bool? IsTestSuccessful { get; private set; }

    public async Task OnGetAsync(Guid Id)
    {
        EmailConfig = await Db.EmailConfigs.FirstAsync(c => c.Id == Id);
        Title = $"Email config: {EmailConfig.Name}";
        PageTitle = EmailConfig.Name;
        ActionBar.Add("Edit..", "./Edit", new { Id }, null, PageActionBar.PageActionType.SECONDARY, "fa fa-pen-to-square");
        BreadCrumb.Add(EmailConfig.Name, ".", new { Id });
    }

    public async Task<IActionResult> OnGetStatusAsync(Guid ID)
    {
        EmailConfig = await Db.EmailConfigs.FirstOrDefaultAsync(c => c.Id == ID);
        EmailConfig.IsActive = !EmailConfig.IsActive;
        await Db.SaveChangesAsync();
        return RedirectToPage("./Details", new { ID });
    }

    public async Task<IActionResult> OnGetSendTest(Guid ID)
    {
        EmailConfig = await Db.EmailConfigs.FirstOrDefaultAsync(c => c.Id == ID);
        Title = $"Email config details: {EmailConfig.Name}";
        var emailSender = new EmailSender(EmailConfig.GetOptions());
        try
        {
            await emailSender.SendEmailAsync(CurrentUser.Email, "Test message", "Test message from PropMngr!");
            IsTestSuccessful = true;
        }
        catch (Exception ex)
        {
            ErrorMessage = ex.Message;
            IsTestSuccessful = false;
        }
        return Page();
    }
}
