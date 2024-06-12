using System;
using System.Collections.Generic;
using System.Text;

namespace DocumentApprovalSystem.Lib
{
    public enum DocStatus : int
    {
        AWAITING_EMPLOYER_APPROVAL = 1 << 1,
        APPROVED = 1 << 2,
        REJECTED = 1 << 3,
        CURRENT = 1 << 4,
        CLOSED = 1 << 5,
        CANCELED = 1 << 6,
        AWAITING_APPROVAL = 1 << 7,
    }
}
