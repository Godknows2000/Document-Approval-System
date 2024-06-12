using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using DocumentApprovalSystem.Data;
using DocumentApprovalSystem.Web.Pages;
using wCyber.Lib.DataSync;

namespace DocumentApprovalSystem.Web.Areas.Config.Pages.Department
{
    public class EditModel : SysPageModel
    {
        [BindProperty]
        public Data.Department Department { get; set; }
        public void OnGet(Guid Id)
        {
            Department = Db.Departments.First(c => c.Id == Id);
            PageTitle = "Edit title";
            Title = $"Edit title: {Department.Name}";
            BreadCrumb.Add(Department.Name, "./Details", new { Id });
            BreadCrumb.Add("Edit", ".", new { Id });
        }
        public async Task<IActionResult> OnPostAsync(Guid Id)
        {
            Department = Db.Departments.First(c => c.Id == Id);
            if (await TryUpdateModelAsync(Department, "", p => p.Name))
            {
                await Db.SaveChangesAsync();
                return RedirectToPage("./Details", new { Id });
            }
            return Page();
        }
    }
}
