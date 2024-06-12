using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using DocumentApprovalSystem.Data;
using DocumentApprovalSystem.Web.Pages;
using wCyber.Helpers.Web;

namespace DocumentApprovalSystem.Web.Areas.Config.Pages.Titles
{
    public class DetailsModel : SysPageModel
    {
        public Data.Title MyTitle { get; set; }
        public void OnGet(int Id)
        {
            MyTitle = Db.Titles.FirstOrDefault(c => c.Id == Id);
            PageTitle = Title = "";
            ActionBar.Add("Edit..", "./Edit", new { Id }, null, PageActionBar.PageActionType.SECONDARY, "fa fa-pen-to-square");
            BreadCrumb.Add(MyTitle.Name, ".", new { Id });
        }
    }
}
