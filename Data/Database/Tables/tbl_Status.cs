using System;
using System.Collections.Generic;

namespace Meeting_App.Data.Database.Tables
{
    public partial class tbl_Status
    {
        public tbl_Status()
        {
            tbl_Task = new HashSet<tbl_Task>();
        }

        public int Id { get; set; }
        public string Status { get; set; }
        public int? RecordStatus { get; set; }

        public virtual ICollection<tbl_Task> tbl_Task { get; set; }
    }
}
