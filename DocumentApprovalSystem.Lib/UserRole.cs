using System;
using System.Collections.Generic;
using System.Text;

namespace DocumentApprovalSystem.Lib
{
    public enum UserRole : int
    {
        USER = 1 << 0,
        ADMIN = 1 << 31,
    }
}
