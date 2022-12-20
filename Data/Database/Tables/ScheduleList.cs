using System;
using System.Collections.Generic;

namespace Meeting_App.Data.Database.Tables
{
    public partial class ScheduleList
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Venue { get; set; }
    }
}
