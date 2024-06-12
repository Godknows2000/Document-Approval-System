using Humanizer;
using DocumentApprovalSystem.Data;
using DocumentApprovalSystem.Web.Pages;
using Microsoft.AspNetCore.Mvc;
using X.PagedList;

namespace DocumentApprovalSystem.Web.Areas.Config.Pages.Email;

public class IndexModel : SysListPageModel<EmailConfig>
{
    public void OnGet(int? p, int? ps)
    {
        Title  = "Email configs..";
        var query = Db.EmailConfigs.AsQueryable();
        List = query.OrderByDescending(c => c.CreationDate).ToPagedList(p ?? 1, ps ?? DefaultPageSize);
        PageTitle = "Email".ToQuantity(List.TotalItemCount);
        if (List.PageCount > 0) PageSubTitle = $"Page {List.PageNumber} of {List.PageCount}";
        ActionBar.Add("Add new..", "./Add", "fa-solid fa-plus");
    }
}
