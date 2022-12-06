using System;
using System.Collections.Generic;

namespace Meeting_App.Data.Database.Tables
{
    public partial class tbl_MeetingVenue
    {
        public tbl_MeetingVenue()
        {
            EventCalender = new HashSet<EventCalender>();
            tbl_Conference = new HashSet<tbl_Conference>();
            tbl_DailyEngagement = new HashSet<tbl_DailyEngagement>();
        }

        public int Id { get; set; }
        public string Venue { get; set; }
        public int? OderBy { get; set; }
        public bool? RecordStatus { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreationDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdationDate { get; set; }

        public virtual ICollection<EventCalender> EventCalender { get; set; }
        public virtual ICollection<tbl_Conference> tbl_Conference { get; set; }
        public virtual ICollection<tbl_DailyEngagement> tbl_DailyEngagement { get; set; }
    }
}
