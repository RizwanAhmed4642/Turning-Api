using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Meeting_App.Models.DTOs
{
    public class DailyEngagementDTO
    {
        public int? Id { get; set; }
        public string Task { get; set; }
        public string WhereToGo { get; set; }
        public DateTime? WhenToGo { get; set; }
        public bool? RecordStatus { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreationDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdationDate { get; set; }
        public bool? IsDeleted { get; set; }

        public int? VenueId { get; set; }
        public string VenueIdName { get; set; }
        public int? EngagementSatusId { get; set; }
        public string EngagementStatusIdName { get; set; }

    }




    public class DailyEngagementFilterDTO
    {
        public Guid? userList { get; set; }
        public string Priority { get; set; }
        public string Division { get; set; }
        public string Status { get; set; }
        public bool? RecordStatus { get; set; }
        public int? DailyEngagementStatusId { get; set; }
        public string DailyEngagementStatus { get; set; }
        public bool ShowMyDailyEngagement { get; set; }

    }

    public class DailyEngagementCount
    {

        public int Total { get; set; }
        public int Active { get; set; }
        public int InActive { get; set; }
        public int Postponed { get; set; }
        public int Rescheduled { get; set; }
        public int Cancelled { get; set; }

        public int Archived { get; set; }
    }


}

