using System;
using System.Collections.Generic;

namespace DocumentApprovalSystem.Data;

public partial class DocType
{
    public Guid Id { get; set; }

    public string? Name { get; set; }

    public DateTime? CreationDate { get; set; }

    public virtual ICollection<DocRequest> DocRequests { get; set; } = new List<DocRequest>();
}
