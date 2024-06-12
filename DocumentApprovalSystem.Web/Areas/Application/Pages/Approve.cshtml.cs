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
public class ApproveModel : SysPageModel
{
    public DocRequest DocRequest { get; set; } = null!;
    [BindProperty]
    public string? Comments { get; set; }
    [BindProperty]
    public IFormFile? ApprovedThumbnail { get; set; }
    [BindProperty]
    public string? SecretarySignature { get; set; }
    public async Task OnGet(Guid id)
    {
        DocRequest = await Db.DocRequests
          .Include(c => c.Employee.Title)
          .Include(c => c.DocType)
          .Include(c => c.Department)
          .Include(c => c.Creator).FirstAsync(c => c.Id == id);
        Title = PageTitle = "Approve Document Request";
    }

    public async Task<IActionResult> OnPost(Guid id)
    {
        //Add logic
        DocRequest = await Db.DocRequests.Include(c => c.Employee.Title).Include(c => c.Employee.IdNavigation).Include(c => c.Creator).FirstAsync(c => c.Id == id);
        DocRequest.Status = User.IsTechSupport() ? DocStatus.APPROVED : DocStatus.AWAITING_APPROVAL ;
        DocRequest.AddNotes(new()
        {
            CreationDate = DateTime.Now,
            Creator = CurrentUser.Name,
            CreatorId = CurrentUserId,
            StatusId = (int)DocRequest.StatusId!,
            Status = DocRequest.Status.Humanize(),
            Text = $"{DocRequest.Number}-Approved by {CurrentUser?.Name} on {DateTime.Now: dddd dd MMM yyy HH:mm}. {Comments}",
        });
        var info = new FileInfo(ApprovedThumbnail.FileName);
        var file = Path.Combine(BaseFilePath, $"ADOC{DocRequest.Id}{info.Extension}");
        DocRequest.AttachmentsJson = file;
        using (var fileStream = new FileStream(file, FileMode.Create))
        {
            await ApprovedThumbnail.CopyToAsync(fileStream);
        }

        EmailSender EmailSender = null!;
        var config = await Db.EmailConfigs.FirstOrDefaultAsync();
        if (config != null) EmailSender = new EmailSender(config.GetOptions());
        var Content = $"We are happy to inform you that Ministry of ICT has approved your Document Signing Request application for your {DocRequest.DocType?.Name}.";
        DocRequest.SendLeaveEmail(EmailSender, HttpContext, true, Content);

        await Db.SaveChangesAsync();
        return RedirectToPage("./Details", new { id });
    }
    public async Task<IActionResult> OnPostSign(Guid id)
    {
        DocRequest = await Db.DocRequests
            .Include(c => c.Employee.Title)
            .FirstOrDefaultAsync(c => c.Id == id) ?? throw new Exception("Document request not found.");

        if (string.IsNullOrWhiteSpace(SecretarySignature))
        {
            ModelState.AddModelError("SecretarySignature", "Secretary signature is required.");
            return Page();
        }

        DocRequest.SecretarySignatureUrl = SecretarySignature;
        await Db.SaveChangesAsync();
        return RedirectToPage("./Details", new { id });
    }
}
