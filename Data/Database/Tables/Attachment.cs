using System;
using System.Collections.Generic;

namespace Meeting_App.Data.Database.Tables
{
    public partial class Attachment
    {
        public int Id { get; set; }
        public int? TaskId { get; set; }
        public int? TaskDetailId { get; set; }
        public string SourceName { get; set; }
        public string AttachmentName { get; set; }
        public bool? RecordStatus { get; set; }

        public virtual tbl_Task Task { get; set; }
        public virtual tbl_TaskDetails TaskDetail { get; set; }
    }
}
