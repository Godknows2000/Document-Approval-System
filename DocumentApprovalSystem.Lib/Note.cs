using System;
using System;
using System.Collections.Generic;
using System.Text;

namespace DocumentApprovalSystem.Lib
{
    public class Note
    {
        public string Creator { get; set; }
        public Guid CreatorId { get; set; }
        public string Text { get; set; }
        public DateTime CreationDate { get; set; }
        public string Status { get; set; }
        public int StatusId { get; set; }
    }
}
