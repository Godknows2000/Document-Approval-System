using DocumentApprovalSystem.Data;
using DocumentApprovalSystem.Lib;
using DocumentApprovalSystem.Web.Auth;
using DocumentApprovalSystem.Web.Pages;
using DocumentApprovalSystem.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Net.Mail;
using wCyber.Helpers.Identity;

namespace DocumentApprovalSystem.Web.Areas.Application.Pages;

public class AddModel : SysPageModel
{
    private readonly IWebHostEnvironment _environment;
    public AddModel(IWebHostEnvironment environment)
    {
        _environment = environment;
    }
    [BindProperty]
    public DocRequest DocRequest { get; set; } = new DocRequest();
    [BindProperty]
    public IFormFile Thumbnail { get; set; }

    [ViewData]
    public SelectList DocType { get; set; }
    [ViewData]
    public SelectList Departments { get; set; }
    public async Task OnGetAsync(Guid id)
    {
        Title = PageTitle = "New document approval request";
        DocType = new SelectList(await Db.DocTypes.OrderBy(c => c.Name).ToListAsync(), nameof(Data.DocType.Id), nameof(Data.DocType.Name));
        Departments = new SelectList(await Db.Departments.OrderBy(c => c.Name).ToListAsync(), nameof(Data.Department.Id), nameof(Data.Department.Name));
    }

    public async Task<IActionResult> OnPostAsync(Guid id)
    {
        DocRequest.Id = Guid.NewGuid();
        var thisMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
        var count = await Db.DocRequests.CountAsync(c => c.CreationDate > thisMonth);
        while (true)
        {
            count = (count + 1) % 100_000;
            DocRequest.Number = $"MOFICT{CurrentUserId.ToString()[0..2]}{thisMonth:yy}{thisMonth.Month:X}{count:000}".ToUpper();
            if (!await Db.DocRequests.AnyAsync(c => c.Number == DocRequest.Number)) break;
        }
        DocRequest.EmployeeId = CurrentUserId;
        DocRequest.CreationDate = DateTime.Now;
        DocRequest.CreatorId = CurrentUserId;
        DocRequest.Status = Lib.DocStatus.AWAITING_APPROVAL;

        await Db.DocRequests.AddAsync(DocRequest);
        var info = new FileInfo(Thumbnail.FileName);
        var file = Path.Combine(_environment.WebRootPath, "uploads", $"{DocRequest.Id}{info.Extension}");
        DocRequest.AttachmentsJson = file;
        using (var fileStream = new FileStream(file, FileMode.Create))
        {
            await Thumbnail.CopyToAsync(fileStream);
        }
        await Db.SaveChangesAsync();

        var url = Url.Page("/Details", pageHandler: null, values: new { DocRequest.Id, area = "Application" }, protocol: Request.Scheme);

        var employeeid = CurrentUserId;
        var Employee = await Db.Employees
            .Include(c => c.Department)
             .Include(c => c.IdNavigation)
             .Include(c => c.Title)
             .FirstOrDefaultAsync(c => c.Id == employeeid);
        EmailSender EmailSender = null!;
        var config = await Db.EmailConfigs.FirstOrDefaultAsync();
        if (config != null) EmailSender = new EmailSender(config.GetOptions());
        var Content = $"A document approval request application was made by {DocRequest?.Employee?.Title} {DocRequest?.Employee?.FirstName}. Please contact the administrator if this was not you.";
       
        var lr=await Db.DocRequests.Include(c => c.Department).Include(c=>c.Employee.IdNavigation).FirstOrDefaultAsync(c=>c.Id== DocRequest.Id);
        lr.SendLeaveEmail(EmailSender, HttpContext, true, Content, CurrentUser.Email, btnText: "View your Document Approval Request", url: url);

        return RedirectToPage("./Index");
    }
}
