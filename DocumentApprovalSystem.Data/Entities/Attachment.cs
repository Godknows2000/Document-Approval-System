using System;
using System.Collections.Generic;

namespace DocumentApprovalSystem.Data;

public partial class Attachment
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public Guid? UniqueId { get; set; }

    public string Container { get; set; } = null!;

    public string? Description { get; set; }

    public int Size { get; set; }

    public Guid CreatorId { get; set; }

    public DateTime CreationDate { get; set; }

    public string? NotesJson { get; set; }

    public string? Extension { get; set; }

    public virtual User Creator { get; set; } = null!;

    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();
}
