using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Meeting_App.Models.DTOs.AdminWing
{
  
    public class GetCrrSummary
    {

        public int? OrderBy { get; set; }
        public int? Code { get; set; }
        public string OfficerDesignation { get; set; }
        public int? TodayUnderProcess { get; set; }
        public int? LT7Days { get; set; }
        public int? GT7Days { get; set; }
        public int? GT15Days { get; set; }
        public int? GT30Days { get; set; }
    }
}
