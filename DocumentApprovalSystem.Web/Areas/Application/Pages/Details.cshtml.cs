using Humanizer;
using DocumentApprovalSystem.Data;
using DocumentApprovalSystem.Lib;
using DocumentApprovalSystem.Web.Auth;
using DocumentApprovalSystem.Web.Pages;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using System.Xml.Linq;

namespace DocumentApprovalSystem.Web.Areas.Application.Pages;
[AccessRight(AccessRight.MANAGE_LEAVES)]

public class DetailsModel : SysPageModel
{
    public DocRequest DocRequest { get; private set; } = null!;
    public bool PDF { get; private set; }
    [BindProperty]
    public string Comments { get; set; }
    [BindProperty]
    public string Signature { get; set; } = null!;
    [BindProperty]
    public string SecretarySignature { get; set; } = null!;
    [BindProperty]
    public Employee Employee { get; private set; }
    public async Task OnGet(Guid id, int? pdf)
    {
        Title = "Document Approval";
        DocRequest = await Db.DocRequests
            .Include(c => c.Employee.Title)
            .Include(c => c.DocType)
            .Include(c => c.Department)
            .Include(c => c.Creator).FirstAsync(c => c.Id == id);
        PDF = pdf == 1;
        Employee = await Db.Employees
           .Include(c => c.IdNavigation)
           .Include(c => c.Title)
           .Include(c => c.Department)
           .FirstAsync(c => c.Id == DocRequest.EmployeeId);
    }

    public async Task<IActionResult> OnPostAddNotesAsync(Guid Id)
    {
        DocRequest = await Db.DocRequests
            .Include(c => c.Employee.Title)
          .Include(c => c.Creator).FirstAsync(c => c.Id == Id);
        DocRequest.AddNotes(new Note
        {
            Text = Comments,
            CreationDate = DateTime.Now,
            StatusId = (int)DocRequest.StatusId,
            CreatorId = CurrentUserId,
            Creator = CurrentUser.Name,
            Status = DocRequest.Status.Humanize(),
        });
        var secretarySignature = SecretarySignature;
        DocRequest.SecretarySignatureUrl = secretarySignature;
        await Db.SaveChangesAsync();
        return RedirectToPage("./Details", new { Id });
    }
    public async Task<IActionResult> OnPostAddDocumentApprovalAsync(Guid Id)
    {
        DocRequest = await Db.DocRequests
            .Include(c => c.Employee.Title)
          .Include(c => c.Creator).FirstAsync(c => c.Id == Id);
        var secretarySignature = SecretarySignature;
        DocRequest.SecretarySignatureUrl = secretarySignature;
        await Db.SaveChangesAsync();
        return RedirectToPage("./Details", new { Id });
    }
    public async Task<IActionResult> OnPostSign(Guid id)
    {

        DocRequest = await Db.DocRequests
            .Include(c => c.Employee.Title)
            .FirstAsync(c => c.Id == id);
        var signature = Signature;
        DocRequest.SignatureUrl = signature;
        await Db.SaveChangesAsync();
        return RedirectToPage("./Details", new { id });
    }
    public IActionResult OnGetDownloadFile(Guid Id)
    {
        DocRequest = Db.DocRequests.Find(Id);
        var path = DocRequest.AttachmentsJson;
        var extension = path.Split('.').Last();
        var bytes = System.IO.File.ReadAllBytes(path);
        return File(bytes, System.Net.Mime.MediaTypeNames.Application.Octet, $"Document-Approval.{extension}");
    }

    public IActionResult OnGetDownload(Guid Id)
    {
        return File(GeneratePdf(Url.PageLink(pageName: "./Pdf", values: new { Id, pdf = 1 })), "application/pdf", $"{Id}.pdf");
    }

    public IActionResult OnGetPreview(Guid Id)
    {
        return new FileContentResult(GeneratePdf(Url.PageLink(pageName: "./Pdf", values: new { Id, pdf = 1 })), "application/pdf");
    }
}
