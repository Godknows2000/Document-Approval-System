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

public class RejectModel : SysPageModel
{
    public DocRequest DocRequest { get; set; }
    [BindProperty]
    public string Comments { get; set; }
    public async Task OnGet(Guid id)
    {
        DocRequest = await Db.DocRequests
          .Include(c => c.Employee.Title)
          .Include(c => c.Creator).FirstAsync(c => c.Id == id);
        Title = PageTitle = "Reject Document Approval Request";
    }

    public async Task<IActionResult> OnPost(Guid id)
    {
        //Add logic
        DocRequest = await Db.DocRequests.Include(c => c.Employee.Title).Include(c => c.Employee.IdNavigation).Include(c => c.Creator).FirstAsync(c => c.Id == id);
        DocRequest.Status = User.IsTechSupport() ? DocStatus.REJECTED : DocStatus.AWAITING_APPROVAL;
        DocRequest.AddNotes(new()
        {
            CreationDate = DateTime.Now,
            Creator = CurrentUser?.Name,
            CreatorId = CurrentUserId,
            StatusId = (int)DocRequest.StatusId,
            Status = DocRequest.Status.Humanize(),
            Text = $"{DocRequest.Number}-Rejected by {CurrentUser?.Name} on {DateTime.Now: dddd dd MMM yyy HH:mm}. {Comments}",
        });
        EmailSender EmailSender = null;
        var config = await Db.EmailConfigs.FirstOrDefaultAsync();
        if (config != null) EmailSender = new EmailSender(config.GetOptions());
        var Content = $"We are sorry to inform you that Ministry of ICT has rejected TO Approve your Document Request application for {DocRequest.DocType?.Name}.";
        DocRequest.SendLeaveEmail(EmailSender, HttpContext, true, Content);

        await Db.SaveChangesAsync();
        return RedirectToPage("./Details", new { id });
    }
}

