using System;
using System.Collections.Generic;

namespace DocumentApprovalSystem.Data;

public partial class Title
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public DateTime? CreationDate { get; set; }

    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();
}
