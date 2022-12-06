using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Meeting_App.Models.DTOs.AdminWing
{
    public class PendancyResponseDTO
    {
        public PendancyResponseDTO()
        {
            pendancy = new List<PendencyDTO>();
        }

        public List<PendencyDTO> pendancy { get; set; }


    }
    public class PendencyDTO
    {
        //public string id { get; set; }
        public string OrderBy { get; set; }
        public string Code { get; set; }
        public string OfficerDesignation { get; set; }
        public string TodayUnderProcess { get; set; }
        public string TodayDispose { get; set; }
        public string UnderProcessGT7Days { get; set; }
        public string UnderProcessGT15Days { get; set; }
        public string UnderProcessGT30Days { get; set; }
        public string UnderProcessUntilToday { get; set; }

    }
}
