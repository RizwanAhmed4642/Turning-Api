using System;
using System.Collections.Generic;

namespace Meeting_App.Data.Database.Tables
{
    public partial class tbl_ConferenceAttendance
    {
        public int Id { get; set; }
        public int? ConferenceId { get; set; }
        public string ParticipantId { get; set; }
        public int? FingerPrintId { get; set; }
        public DateTime? Datetime { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedByUserId { get; set; }
    }
}
