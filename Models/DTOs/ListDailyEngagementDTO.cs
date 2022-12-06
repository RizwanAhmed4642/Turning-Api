using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Meeting_App.Models.DTOs
{
    public class ListDailyEngagementDTO
    {
        public int Id { get; set; }
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
}
