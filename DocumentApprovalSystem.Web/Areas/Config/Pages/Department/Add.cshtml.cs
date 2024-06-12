using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using DocumentApprovalSystem.Data;
using DocumentApprovalSystem.Web.Pages;

namespace DocumentApprovalSystem.Web.Areas.Config.Pages.Department;
public class AddModel : SysPageModel
{
    [BindProperty]
    public Data.Department Department { get; set; }

    public void OnGet()
    {
        Title = PageTitle = "Add new..";
        BreadCrumb.Add("Add");
    }

    public async Task<IActionResult> OnPostAsync()
    {
        Department.Id = Guid.NewGuid();
        Department.CreationDate = DateTime.Now;
        Db.Departments.Add(Department);
        await Db.SaveChangesAsync();
        return RedirectToPage("./Details", new { Department.Id });
    }
}
