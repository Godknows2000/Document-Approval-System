using System;
using System.Collections.Generic;

namespace DocumentApprovalSystem.Data;

public partial class EmployerSec
{
    public Guid Id { get; set; }

    public string Email { get; set; } = null!;

    public string Phone { get; set; } = null!;

    public string? Address { get; set; }

    public DateTime? CreationDate { get; set; }

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
