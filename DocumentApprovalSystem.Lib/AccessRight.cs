using System;
using System.Collections.Generic;
using System.Text;

namespace DocumentApprovalSystem.Lib
{
    public enum AccessRight
    {
        MANAGE_USERS = 1 << 5,
        MANAGE_APPLICATIONS = 1 << 1,
        MANAGE_LEAVES = 33,
    }
}
