using System;
using System.Collections.Generic;

namespace Meeting_App.Data.Database.Tables
{
    public partial class tbl_MeetingStatus
    {
        public tbl_MeetingStatus()
        {
            EventCalender = new HashSet<EventCalender>();
            tbl_Conference = new HashSet<tbl_Conference>();
            tbl_DailyEngagement = new HashSet<tbl_DailyEngagement>();
        }

        public int Id { get; set; }
        public string Status { get; set; }
        public int? RecordStatus { get; set; }

        public virtual ICollection<EventCalender> EventCalender { get; set; }
        public virtual ICollection<tbl_Conference> tbl_Conference { get; set; }
        public virtual ICollection<tbl_DailyEngagement> tbl_DailyEngagement { get; set; }
    }
}
