using System;
using System.Collections.Generic;

namespace DocumentApprovalSystem.Data;

public partial class Employee
{
    public Guid Id { get; set; }

    public string AccountId { get; set; } = null!;

    public string FirstName { get; set; } = null!;

    public string Surname { get; set; } = null!;

    public int TitleId { get; set; }

    public DateTime DoB { get; set; }

    public string IdNumber { get; set; } = null!;

    public string? EcNumber { get; set; }

    public string Address { get; set; } = null!;

    public int ProfileStatusId { get; set; }

    public string? AttachmentsJson { get; set; }

    public int StatusId { get; set; }

    public DateTime CreationDate { get; set; }

    public string Position { get; set; } = null!;

    public string? NotesJson { get; set; }

    public Guid? ProfilePictureId { get; set; }

    public Guid? EmployeeId { get; set; }

    public Guid? DepartmentId { get; set; }

    public virtual Department? Department { get; set; }

    public virtual ICollection<DocRequest> DocRequests { get; set; } = new List<DocRequest>();

    public virtual User IdNavigation { get; set; } = null!;

    public virtual Attachment? ProfilePicture { get; set; }

    public virtual Title Title { get; set; } = null!;
}
