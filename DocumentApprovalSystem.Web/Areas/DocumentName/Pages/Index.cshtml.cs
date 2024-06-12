using Humanizer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using DocumentApprovalSystem.Data;
using DocumentApprovalSystem.Web.Pages;
using X.PagedList;

namespace DocumentApprovalSystem.Web.Areas.DocumentName.Pages
{
    public class IndexModel : SysListPageModel<Data.DocType>
    {
        public void OnGet(int? p, int? ps, string q)
        {
            var query = Db.DocTypes.AsQueryable();
            List = query.OrderBy(c => c.Id).ToPagedList(p ?? 1, ps ?? DefaultPageSize);
            PageTitle = "Title".ToQuantity(List.TotalItemCount);
            if (List.PageCount > 0) PageSubTitle = $"Page {List.PageNumber} of {List.PageCount}";
            ActionBar.Add("Add new..", "./Add", "fa-solid fa-plus");

        }
    }
}
