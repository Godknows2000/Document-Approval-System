using System;
using System.Collections.Generic;

namespace DocumentApprovalSystem.Data;

public partial class DocRequest
{
    public Guid Id { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public Guid? DocTypeId { get; set; }

    public string? RequestComments { get; set; }

    public DateTime CreationDate { get; set; }

    public Guid? DepartmentId { get; set; }

    public string? Number { get; set; }

    public Guid? EmployeeId { get; set; }

    public int? StatusId { get; set; }

    public Guid? CreatorId { get; set; }

    public string? NotesJson { get; set; }

    public string? AttachmentsJson { get; set; }

    public string? SignatureUrl { get; set; }

    public string? SecretarySignatureUrl { get; set; }

    public virtual User? Creator { get; set; }

    public virtual Department? Department { get; set; }

    public virtual DocType? DocType { get; set; }

    public virtual Employee? Employee { get; set; }
}
