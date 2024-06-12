using DocumentApprovalSystem.Data;
using DocumentApprovalSystem.Web.Pages;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DocumentApprovalSystem.Web.Areas.Account.Pages;
public class IndexModel : SysPageModel
{
    public Employee Employee { get; private set; }
    public bool HasEcNumber => !string.IsNullOrEmpty(Employee.EcNumber);
    public bool DocsUploaded { get; private set; }
    //public List<Loan> Loans { get; private set; }
    public async Task OnGetAsync(Guid? id)
    {
        var employeeid = id ?? CurrentUserId;
        Employee = await Db.Employees
            .Include(c => c.IdNavigation)
            .Include(c => c.Title)
            .FirstAsync(c => c.Id == employeeid);
        var attachments = Employee.GetAttachments();
        //DocsUploaded = Employee.AttachmentTypes.All(c => attachments.Any(x => x.TypeId == c.Id));
        if (Employee.ProfileStatusId != (int)Lib.EmployeeProfileStatus.APPROVED && Employee.ProfileStatusId == (int)Lib.EmployeeProfileStatus.PENDING && DocsUploaded && HasEcNumber)
        {
            Employee.ProfileStatus = Lib.EmployeeProfileStatus.AWAITING_REVIEW;
            await Db.SaveChangesAsync();
        }
        BreadCrumb.Items.Clear();
    }

    public async Task<IActionResult> OnGetApprove(Guid id)
    {
        Employee = await Db.Employees
            .Include(c => c.IdNavigation)
            .Include(c => c.Title)
            .FirstAsync(c => c.Id == id);
        Employee.ProfileStatus = Lib.EmployeeProfileStatus.APPROVED;
        await Db.SaveChangesAsync();
        return RedirectToPage("/Employees/Index", new { area = "Employers" });
    }
}
