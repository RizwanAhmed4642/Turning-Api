using System;
using System.Collections.Generic;

namespace Meeting_App.Data.Database.Tables
{
    public partial class MeetingAttachment
    {
        public int Id { get; set; }
        public int? MeetingId { get; set; }
        public string SourceName { get; set; }
        public string AttachmentName { get; set; }
        public bool? RecordStatus { get; set; }

        public virtual EventCalender Meeting { get; set; }
    }
}
