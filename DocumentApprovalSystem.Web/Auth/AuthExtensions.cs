using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using DocumentApprovalSystem.Lib;
using DocumentApprovalSystem.Data;
using System.Net;
using System.Security.Claims;
using DocumentApprovalSystem.Web;
using wCyber.Lib;

namespace DocumentApprovalSystem.Web;

public static class AuthExtensions
{
    public static async Task InitUser(this CookieSigningInContext context)
    {
        var db = context.HttpContext.RequestServices.GetService<DocumentApprovalDbContext>();
        var userId = Guid.Parse(context.Principal.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        var rights = 0;
        var user = await db.Users
            .Include(c => c.Employee)
            .FirstOrDefaultAsync(c => c.Id == userId);
        var claims = new List<Claim>();

        if (user.Employee != null)
        {
            claims.Add(new Claim(Claims.UserType, ((int)UserType.EMPLOYEE).ToString()));
            rights |= (int)AccessRight.MANAGE_LEAVES;
        }


        claims.Add(new Claim(Claims.UserType, ((int)UserType.TECH_SUPPORT).ToString()));

        if (user.RoleId == (int)Lib.UserRole.ADMIN) rights = Enum.GetValues(typeof(AccessRight)).Cast<int>().Aggregate(0, (s, f) => s | f);

        claims.Add(new Claim(Claims.UserRightsClaim, rights.ToString()));

        context.Principal.AddIdentity(new ClaimsIdentity(claims));
    }

    public static UserType GetUserType(this ClaimsIdentity user)
        => (UserType)int.Parse(user.FindFirst(Claims.UserType)?.Value ?? "0");

    public static UserType GetUserType(this ClaimsPrincipal user)
        => (UserType)int.Parse(user.FindFirst(Claims.UserType)?.Value ?? "0");

    public static bool IsTechSupport(this ClaimsIdentity user)
        => user.GetUserType() == UserType.TECH_SUPPORT;
    public static bool IsTechSupport(this ClaimsPrincipal user)
        => user.GetUserType() == UserType.TECH_SUPPORT;

    public static bool IsEmployee(this ClaimsIdentity user)
    => user.GetUserType() == UserType.EMPLOYEE;
    public static bool IsEmployee(this ClaimsPrincipal user)
        => user.GetUserType() == UserType.EMPLOYEE;
    public static string GetActivationLink(this User user, HttpRequest Request)
        => Request.Scheme + "://" + Request.Host + "/Identity/Account/Activate?code=" + WebUtility.UrlEncode(Convert.ToBase64String(user.Id.ToByteArray()));



}
