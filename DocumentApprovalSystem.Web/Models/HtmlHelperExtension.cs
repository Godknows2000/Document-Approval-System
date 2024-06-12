using Humanizer;
using Microsoft.AspNetCore.Html;
using DocumentApprovalSystem.Lib;
using System.Drawing;
using DocumentApprovalSystem.Data;

namespace DocumentApprovalSystem.Web;

public static class HtmlHelperExtension
{
    public static HtmlString ToStatusHtml(this bool value)
        => new($"<i class='fa fa-{(value ? "check-circle text-success" : "clock text-muted")}'></i>");
    public static HtmlString ToHtml(this bool value)
    {
        return value switch
        {
            true => new HtmlString($"<span class='badge bg-success'>ACTIVE</span>"),
            _ => new HtmlString($"<span class='badge bg-danger'>IN-ACTIVE</span>"),
        };
    }
    public static HtmlString AccountStatusHtml(this User user)
    {
        if (user.IsEmailConfirmed) return new HtmlString($"{user.IsActive.ToStatusHtml()} {(user.IsActive ? "ACTIVE" : "DEACTIVATED")}");
        else return new HtmlString("<i class='fa fa-user-clock text-secondary'></i> AWAITING ACTIVATION");
    }
    public static bool IsNull(this PointF value)
    {
        return float.IsNaN(value.X) || float.IsNaN(value.Y);
    }
    public static HtmlString ToHtml(this DocStatus value)
    {
        return value switch
        {
            DocStatus.REJECTED => new HtmlString($"<span class='badge bg-danger'>{value.Humanize()}</span>"),
            DocStatus.AWAITING_APPROVAL => new HtmlString($"<span class='badge bg-warning'>{value.Humanize()}</span>"),
            DocStatus.APPROVED => new HtmlString($"<span class='badge bg-success'>{value.Humanize()}</span>"),
            DocStatus.CURRENT => new HtmlString($"<span class='badge bg-primary'>{value.Humanize()}</span>"),
            DocStatus.CLOSED => new HtmlString($"<span class='badge bg-success'>{value.Humanize()}</span>"),
            DocStatus.CANCELED => new HtmlString($"<span class='badge bg-danger'>{value.Humanize()}</span>"),
            _ => new HtmlString($"<span class='badge bg-light'>{value.Humanize()}</span>"),
        };
    }
}
