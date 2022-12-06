using System;
using System.Collections.Generic;

namespace Meeting_App.Data.Database.Tables
{
    public partial class tbl_ConferenceAttachment
    {
        public int Id { get; set; }
        public int? ConferenceId { get; set; }
        public string SourceName { get; set; }
        public string AttachmentName { get; set; }
        public bool? RecordStatus { get; set; }

        public virtual tbl_Conference Conference { get; set; }
    }
}
