using System;
using System.Collections.Generic;
using System.Text;

namespace DocumentApprovalSystem.Lib
{
    public enum EmployeeProfileStatus : int
    {
        PENDING = 0,
        AWAITING_REVIEW = 1,
        APPROVED = 2,
        REJECTED = 3,
    }
}
