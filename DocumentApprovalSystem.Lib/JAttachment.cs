using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DocumentApprovalSystem.Lib
{

    public partial class JAttachment
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int TypeId { get; set; }
        public DateTime CreationDate { get; set; }
        public string Extension { get; set; }

        [JsonIgnore]
        public bool IsImage => Extension != null;
    }
}