using System;
using System.Collections.Generic;

namespace Meeting_App.Data.Database.Tables
{
    public partial class ContactDepartment
    {
        public int Id { get; set; }
        public int? CompanyId { get; set; }
        public string DepartmentName { get; set; }
        public bool? RecordStatus { get; set; }
        public int? OderBy { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreationDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdationDate { get; set; }
    }
}
