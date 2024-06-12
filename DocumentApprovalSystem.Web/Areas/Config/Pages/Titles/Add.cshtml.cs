using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using DocumentApprovalSystem.Data;
using DocumentApprovalSystem.Web.Pages;

namespace DocumentApprovalSystem.Web.Areas.Config.Pages.Titles;
public class AddModel : SysPageModel
{
    [BindProperty]
    public Data.Title MyTitle { get; set; }

    public void OnGet()
    {
        Title = PageTitle = "Add new..";
        BreadCrumb.Add("Add");
    }

    public async Task<IActionResult> OnPostAsync()
    {
        MyTitle.Id = Db.Titles.Count() + 1;
        MyTitle.CreationDate = DateTime.Now;
        Db.Titles.Add(MyTitle);
        await Db.SaveChangesAsync();
        return RedirectToPage("./Details", new { MyTitle.Id });
    }
}
