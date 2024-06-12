using System;
using System.Collections.Generic;

namespace DocumentApprovalSystem.Data;

public partial class Department
{
    public Guid Id { get; set; }

    public string? Name { get; set; }

    public DateTime? CreationDate { get; set; }

    public virtual ICollection<DocRequest> DocRequests { get; set; } = new List<DocRequest>();

    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();
}
