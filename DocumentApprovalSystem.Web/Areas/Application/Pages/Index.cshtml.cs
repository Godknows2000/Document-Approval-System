using DocumentApprovalSystem.Data;
using DocumentApprovalSystem.Lib;
using DocumentApprovalSystem.Web.Auth;
using DocumentApprovalSystem.Web.Pages;
using Microsoft.EntityFrameworkCore;
using X.PagedList;

namespace DocumentApprovalSystem.Web.Areas.Application.Pages;
[AccessRight(AccessRight.MANAGE_LEAVES)]

public class IndexModel : SysListPageModel<DocRequest>
{
    public Employee Employee { get; private set; }
    public DocRequest DocRequest { get; private set; }
    public async Task OnGetAsync(int? p, int? ps, string q)
    {
        Employee = await Db.Employees
            .Include(c => c.Title)
            .Include(c => c.Department)
            .FirstOrDefaultAsync(c => c.Id == CurrentUserId);
        // BreadCrumb.Items.Clear();
        var query = Db.DocRequests
            .Include(c => c.Employee.Title)
            .Include(c => c.Department)
            .Include(c => c.DocType)
            .AsQueryable();
        SearchPlaceholder = "Search documents..";
        if (!string.IsNullOrWhiteSpace(q))
        {
            QueryString = q;
            q = q.Trim().ToLower();
            query = query.Where(c => c.Number.ToLower().Contains(q));
        }
        //if (CurrentEmployerId.HasValue)
        //{
        //    query = query.Where(c => c.Employee.EmployerId == CurrentEmployerId);
        //}
        if (User.IsEmployee()) query = query.Where(c => c.EmployeeId == CurrentUserId);
        Title = PageTitle = "Documents approval request applications";
        if (User.IsEmployee() && !(Employee.IsProfileInReview || Employee.IsProfilePending))
        {
            ActionBar.Add("New application..", "./Index", new { area = "Application", pick = true }, icon: "fa fa-plus");
        }
        List = query.OrderByDescending(c => c.CreationDate).ToPagedList(p ?? 1, ps ?? DefaultPageSize);
    }
}
