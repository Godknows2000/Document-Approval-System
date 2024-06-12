using System;
using System.Collections.Generic;

namespace DocumentApprovalSystem.Data;

public partial class EmailConfig
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public bool IsActive { get; set; }

    public int TargetId { get; set; }

    public string SenderId { get; set; } = null!;

    public string? Username { get; set; }

    public string? SenderDisplayName { get; set; }

    public string Hash { get; set; } = null!;

    public string Host { get; set; } = null!;

    public int Port { get; set; }

    public bool EnableSsl { get; set; }

    public Guid CreatorId { get; set; }

    public DateTime CreationDate { get; set; }

    public virtual User Creator { get; set; } = null!;
}
