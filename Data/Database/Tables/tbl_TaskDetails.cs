using System;
using System.Collections.Generic;

namespace Meeting_App.Data.Database.Tables
{
    public partial class tbl_TaskDetails
    {
        public tbl_TaskDetails()
        {
            Attachment = new HashSet<Attachment>();
        }

        public int Id { get; set; }
        public int? TaskId { get; set; }
        public string Comments { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public bool? RecordStatus { get; set; }
        public bool? IsRead { get; set; }
        public string CommentFor { get; set; }
        public DateTime? ReadDateTime { get; set; }

        public virtual tbl_Task Task { get; set; }
        public virtual ICollection<Attachment> Attachment { get; set; }
    }
}
