using Humanizer;
using DocumentApprovalSystem.Data;
using DocumentApprovalSystem.Lib;
using DocumentApprovalSystem.Web.Auth;
using DocumentApprovalSystem.Web.Models;
using DocumentApprovalSystem.Web.Pages;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using wCyber.Helpers.Identity;

namespace DocumentApprovalSystem.Web.Areas.Application.Pages;
[AccessRight(AccessRight.MANAGE_LEAVES)]
public class CanceDocumentApprovalSystemodel : SysPageModel
{
    public DocRequest DocRequest { get; set; }
    [BindProperty]
    public string Comments { get; set; }
    public async Task OnGet(Guid id)
    {
        DocRequest = await Db.DocRequests
          .Include(c => c.Employee.Title)
          .Include(c => c.Creator).FirstAsync(c => c.Id == id);
        Title = PageTitle = "Cancel this leave request";
    }

    public async Task<IActionResult> OnPost(Guid id)
    {
        //Add logic
        DocRequest = await Db.DocRequests.Include(c => c.Employee.Title).Include(c => c.Employee.IdNavigation).Include(c => c.Creator).FirstAsync(c => c.Id == id);
        DocRequest.Status = DocStatus.CANCELED;
        DocRequest.AddNotes(new()
        {
            CreationDate = DateTime.Now,
            Creator = CurrentUser.Name,
            CreatorId = CurrentUserId,
            StatusId = (int)DocRequest.StatusId,
            Status = DocRequest.Status.Humanize(),
            Text = $"{DocRequest.Number}-Canceled by {CurrentUser?.Name} on {DateTime.Now: dddd dd MMM yyy HH:mm}. {Comments}",
        });
        EmailSender EmailSender = null;
        var config = await Db.EmailConfigs.FirstOrDefaultAsync();
        if (config != null) EmailSender = new EmailSender(config.GetOptions());
        var Content = $"Please note that {CurrentUser?.Name} canceled your Document Approval request application for {DocRequest.DocType?.Name}. Cancellation reason: {Comments}. If this was not you, please contact the administrator as soon as you can";
        DocRequest.SendLeaveEmail(EmailSender, HttpContext, true, Content);

        await Db.SaveChangesAsync();
        return RedirectToPage("./Details", new { id });
    }

}
