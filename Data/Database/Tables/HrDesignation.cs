using System;
using System.Collections.Generic;

namespace Meeting_App.Data.Database.Tables
{
    public partial class HrDesignation
    {
        public int Id { get; set; }
        public int Code { get; set; }
        public string Name { get; set; }
        public int? OrderBy { get; set; }
        public int? Cadre_Id { get; set; }
        public int? HrScale_Id { get; set; }
        public string Remarks { get; set; }
        public string Created_By { get; set; }
        public DateTime? Creation_Date { get; set; }
        public bool? IsActive { get; set; }
        public int? HrScale_Id2 { get; set; }
        public int? OD_Id { get; set; }
    }
}
