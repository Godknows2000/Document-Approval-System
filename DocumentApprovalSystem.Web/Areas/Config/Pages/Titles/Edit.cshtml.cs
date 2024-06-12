using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using DocumentApprovalSystem.Data;
using DocumentApprovalSystem.Web.Pages;
using wCyber.Lib.DataSync;

namespace DocumentApprovalSystem.Web.Areas.Config.Pages.Titles
{
    public class EditModel : SysPageModel
    {
        [BindProperty]
        public Data.Title MyTitle { get; set; }
        public void OnGet(int Id)
        {
            MyTitle = Db.Titles.First(c => c.Id == Id);
            PageTitle = "Edit title";
            Title = $"Edit title: {MyTitle.Name}";
            BreadCrumb.Add(MyTitle.Name, "./Details", new { Id });
            BreadCrumb.Add("Edit", ".", new { Id });
        }
        public async Task<IActionResult> OnPostAsync(int Id)
        {
            MyTitle = Db.Titles.FirstOrDefault(c => c.Id == Id);
            if (await TryUpdateModelAsync(MyTitle, "", p => p.Name))
            {
                await Db.SaveChangesAsync();
                return RedirectToPage("./Details", new { Id });
            }
            return Page();
        }
    }
}
