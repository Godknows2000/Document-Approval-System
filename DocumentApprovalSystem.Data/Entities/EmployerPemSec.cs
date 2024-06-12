using System;
using System.Collections.Generic;

namespace DocumentApprovalSystem.Data;

public partial class EmployerPemSec
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Email { get; set; }

    public string Phone { get; set; } = null!;

    public DateTime? CreationDate { get; set; }

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
