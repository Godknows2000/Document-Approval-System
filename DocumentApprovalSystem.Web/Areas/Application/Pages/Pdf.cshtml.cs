
using DocumentApprovalSystem.Data;
using DocumentApprovalSystem.Lib;
using DocumentApprovalSystem.Web.Auth;
using DocumentApprovalSystem.Web.Pages;
using Microsoft.EntityFrameworkCore;

namespace DocumentApprovalSystem.Web.Areas.Application.Pages;
[AccessRight(AccessRight.MANAGE_LEAVES)]

public class PdfModel : SysPageModel
{
    public DocRequest DocRequest { get; private set; } = null!;
    public async Task OnGet(Guid id)
    {
        Title = PageTitle = "Document Approval Request";
        DocRequest = await Db.DocRequests
            .Include(c => c.Employee.Title)
            .Include(c => c.DocType)
            .Include(c => c.Department)
            .Include(c => c.Creator).FirstAsync(c => c.Id == id);
    }
}
