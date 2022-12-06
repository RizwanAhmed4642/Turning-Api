using System;
using System.Collections.Generic;

namespace Meeting_App.Data.Database.Tables
{
    public partial class tbl_UserFP
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string FP { get; set; }
        public DateTime? Datetime { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedByUserId { get; set; }
        public bool? IsActive { get; set; }
    }
}
