using System;
using System.Collections.Generic;

namespace Meeting_App.Data.Database.Tables
{
    public partial class tbl_ContactDesignation
    {
        public int Id { get; set; }
        public int? DepartmentId { get; set; }
        public string Designation { get; set; }
        public string CategoryCode { get; set; }
        public int? OderBy { get; set; }
        public bool? RecordStatus { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreationDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdationDate { get; set; }
        public int? CategoryId { get; set; }
    }
}
