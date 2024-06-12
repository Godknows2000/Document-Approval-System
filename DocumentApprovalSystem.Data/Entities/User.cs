using System;
using System.Collections.Generic;

namespace DocumentApprovalSystem.Data;

public partial class User
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public string LoginId { get; set; } = null!;

    public string? PasswordHash { get; set; }

    public string Email { get; set; } = null!;

    public string? Mobile { get; set; }

    public int RoleId { get; set; }

    public bool IsActive { get; set; }

    public bool IsEmailConfirmed { get; set; }

    public string? SecurityStamp { get; set; }

    public DateTime? LastLoginDate { get; set; }

    public string? AuthenticatorKey { get; set; }

    public string? AuthRecoveryCodes { get; set; }

    public bool TwoFactorAuthEnabled { get; set; }

    public int AccessFailedCount { get; set; }

    public DateTime? LockoutExpiryDate { get; set; }

    public Guid? CreatorId { get; set; }

    public DateTime CreationDate { get; set; }

    public DateTime? ActivationDate { get; set; }

    public Guid? EmployerSecId { get; set; }

    public Guid? EmployerPemSecId { get; set; }

    public virtual ICollection<Attachment> Attachments { get; set; } = new List<Attachment>();

    public virtual User? Creator { get; set; }

    public virtual ICollection<DocRequest> DocRequests { get; set; } = new List<DocRequest>();

    public virtual ICollection<EmailConfig> EmailConfigs { get; set; } = new List<EmailConfig>();

    public virtual Employee? Employee { get; set; }

    public virtual EmployerPemSec? EmployerPemSec { get; set; }

    public virtual EmployerSec? EmployerSec { get; set; }

    public virtual ICollection<User> InverseCreator { get; set; } = new List<User>();
}
