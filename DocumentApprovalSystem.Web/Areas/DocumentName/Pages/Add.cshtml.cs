using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using DocumentApprovalSystem.Data;
using DocumentApprovalSystem.Web.Pages;

namespace DocumentApprovalSystem.Web.Areas.DocumentName.Pages;
public class AddModel : SysPageModel
{
    [BindProperty]
    public Data.DocType DocType { get; set; } = null!;

    public void OnGet()
    {
        Title = PageTitle = "Add new..";
        BreadCrumb?.Add("Add");
    }

    public async Task<IActionResult> OnPostAsync()
    {
        DocType.Id = Guid.NewGuid();
        DocType.CreationDate= DateTime.Now;
        Db.DocTypes.Add(DocType);
        await Db.SaveChangesAsync();
        return RedirectToPage("./Details", new { DocType.Id });
    }
}
