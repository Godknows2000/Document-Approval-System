using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using DocumentApprovalSystem.Data;
using DocumentApprovalSystem.Web.Pages;
using wCyber.Helpers.Web;

namespace DocumentApprovalSystem.Web.Areas.DocumentName.Pages
{
    public class DetailsModel : SysPageModel
    {
        public Data.DocType DocType { get; set; }
        public void OnGet(Guid Id)
        {
            DocType = Db.DocTypes.First(c => c.Id == Id);
            PageTitle = Title = "";
            ActionBar.Add("Edit..", "./Edit", new { Id }, null, PageActionBar.PageActionType.SECONDARY, "fa fa-pen-to-square");
            BreadCrumb?.Add(DocType.Name, ".", new { Id });
        }
    }
}
