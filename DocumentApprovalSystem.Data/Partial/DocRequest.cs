using DocumentApprovalSystem.Lib;
using System.ComponentModel.DataAnnotations.Schema;

namespace DocumentApprovalSystem.Data;

partial class DocRequest : INotesContainer, IAttachmentsContainer
{
    [NotMapped]
    public DocStatus Status { get => (DocStatus)(StatusId??0); set => StatusId = (int)value; }
    [NotMapped]
    public bool IsApproved => Status == DocStatus.APPROVED;
    [NotMapped]
    public bool IsAwaitingCreditProvider => Status == DocStatus.AWAITING_APPROVAL;
    [NotMapped]
    public bool IsCanceled => Status == DocStatus.CANCELED;
    [NotMapped]
    public bool IsClosed => Status == DocStatus.CLOSED;
    [NotMapped]
    public bool IsCurrent => Status == DocStatus.CURRENT;

}
