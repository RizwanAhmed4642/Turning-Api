using System;
using System.Collections.Generic;

namespace Meeting_App.Data.Database.Tables
{
    public partial class tbl_DailyEngagment
    {
        public int Id { get; set; }
        public string Task { get; set; }
        public string WhereToGo { get; set; }
        public DateTime? WhenToGo { get; set; }
        public bool? RecordStatus { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreationDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdationDate { get; set; }
        public bool? IsDeleted { get; set; }
    }
}
