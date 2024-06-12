using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using DocumentApprovalSystem.Data;
using DocumentApprovalSystem.Web.Pages;
using wCyber.Lib.DataSync;

namespace DocumentApprovalSystem.Web.Areas.DocumentName.Pages
{
    public class EditModel : SysPageModel
    {
        [BindProperty]
        public Data.DocType DocType { get; set; }
        public void OnGet(Guid Id)
        {
            DocType = Db.DocTypes.First(c => c.Id == Id);
            PageTitle = "Edit title";
            Title = $"Edit title: {DocType.Name}";
            BreadCrumb?.Add(DocType.Name, "./Details", new { Id });
            BreadCrumb?.Add("Edit", ".", new { Id });
        }
        public async Task<IActionResult> OnPostAsync(Guid Id)
        {
            DocType = Db.DocTypes.First(c => c.Id == Id);
            if (await TryUpdateModelAsync(DocType, "", p => p.Name))
            {
                await Db.SaveChangesAsync();
                return RedirectToPage("./Details", new { Id });
            }
            return Page();
        }
    }
}
