using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Meeting_App.Models.DTOs.AdminWing
{
  
    public class VPReportDTO
    {

        public int? DesignationID { get; set; }
        public string DesignationName { get; set; }
        public int? TotalSanctioned { get; set; }
        public int? TotalWorking { get; set; }
        public string CadreName { get; set; }
        public int? BPS { get; set; }
        public string HFMISCode { get; set; }
        public string HfFullName { get; set; }
        public string HfTypeName { get; set; }
        public string Adhoc { get; set; }
        public string Contract { get; set; }
        public string DailyWages { get; set; }
        public string Regular { get; set; }
        public string PHFMC { get; set; }
        public int? TotalVacant { get; set; }
        public int? TotalProfile { get; set; }
    }
}
