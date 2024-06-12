
using DocumentApprovalSystem.Lib;
using System.ComponentModel.DataAnnotations.Schema;
using wCyber.Helpers.Identity;

namespace DocumentApprovalSystem.Data;

partial class EmailConfig
{
    [NotMapped]
    public EmailConfigTarget Target
    {
        get => (EmailConfigTarget)TargetId;
        set => TargetId = (int)value;
    }

    [NotMapped]
    public string Password { get; set; }

    public string ComputeHash() => Hash = Password?.GetHash(Id.ToString());
    public string DecodePassword() => Hash?.GetPassword(Id.ToString());
}
