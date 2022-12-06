using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Meeting_App.Models.DTOs.AdminWing
{
    public class LeavesExpiredDTO
    {
        public int Id { get; set; }
        public string Barcode { get; set; }
        public string FileNumber { get; set; }
        public string EmployeeName { get; set; }
        public string FatherName { get; set; }
        public string CNIC { get; set; }
        public string StatusName { get; set; }
        public string Designation_Name { get; set; }
        public string LeaveTypeName { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public int? Officer_Id { get; set; }
        public string SignedBy { get; set; }
        public int? DateDiff { get; set; }
    }
}
