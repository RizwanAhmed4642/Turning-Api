using System;
using System.Collections.Generic;

namespace Meeting_App.Data.Database.Tables
{
    public partial class tbl_TaskAssignee
    {
        public int Id { get; set; }
        public int? TaskId { get; set; }
        public Guid? TaskAssignToID { get; set; }

        public virtual tbl_Task Task { get; set; }
        public virtual AspNetUsers TaskAssignTo { get; set; }
    }
}
