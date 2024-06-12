using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using DocumentApprovalSystem.Lib;
using wCyber.Helpers.Web;
using DocumentApprovalSystem.Web;

namespace DocumentApprovalSystem.Web.Auth;

public class AccessRightAttribute : AuthorizeAttribute, IAuthorizationFilter
{
    readonly AccessRight[] AccessRights;
    public AccessRightAttribute(params AccessRight[] rights)
    {
        AccessRights = rights;
    }
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        if ((AccessRights?.Length ?? 0) == 0) return;
        var rights = (AccessRight)(int.Parse(context.HttpContext.User.FindFirst(Claims.UserRightsClaim)?.Value ?? "0"));
        foreach (var item in AccessRights)
        {
            if (rights.HasFlag(item)) return;
        }
        var reqRights = (AccessRight)AccessRights.Sum(c => (int)c);
        if (context.HttpContext.Request.Path.Value.ToLower().Contains("/api/")) context.Result = new UnauthorizedResult();
        else context.Result = new LocalRedirectResult($"/Auth/AccessDenied?page={context.HttpContext.Request.Path}{context.HttpContext.Request.QueryString}&rights={reqRights.ToEnumString()}");
    }
}
