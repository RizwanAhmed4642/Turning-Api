using System;
using System.Collections.Generic;

namespace Meeting_App.Data.Database.Tables
{
    public partial class tbl_TaskCc
    {
        public int Id { get; set; }
        public int? TaskId { get; set; }
        public Guid? TaskCcID { get; set; }

        public virtual tbl_Task Task { get; set; }
        public virtual AspNetUsers TaskCc { get; set; }
    }
}
