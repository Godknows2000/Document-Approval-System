using DocumentApprovalSystem.Lib;
using System.ComponentModel.DataAnnotations.Schema;

namespace DocumentApprovalSystem.Data;

partial class Employee : INotesContainer, IAttachmentsContainer
{
    [NotMapped]
    public string Name => $" {FirstName} {Surname}";

    [NotMapped]
    public string Initials => (!Name.Trim().Contains(' ', StringComparison.CurrentCulture) ?
     Name[..Math.Min(2, Name.Length)] :
     new string(Name.Trim().Split(" ").Select(c => c[0]).ToArray())).ToUpper();
    [NotMapped]
    public User User => IdNavigation;

    [NotMapped]
    public string FullName => $"{Title?.Name} {FirstName} {Surname}";
    [NotMapped]
    public EmployeeStatus Status
    {
        get => (EmployeeStatus)StatusId;
        set => StatusId = (int)value;
    }
    [NotMapped]
    public bool IsUploaded => Status == EmployeeStatus.UPLOADED;

    [NotMapped]
    public EmployeeProfileStatus ProfileStatus
    {
        get => (EmployeeProfileStatus)ProfileStatusId;
        set => ProfileStatusId = (int)value;
    }
    [NotMapped]
    public bool IsProfilePending => ProfileStatus == EmployeeProfileStatus.PENDING;
    [NotMapped]
    public bool IsProfileInReview => ProfileStatus == EmployeeProfileStatus.AWAITING_REVIEW;
    [NotMapped]
    public bool IsRejected => ProfileStatus == EmployeeProfileStatus.REJECTED;

}
