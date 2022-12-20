using System;
using System.Collections.Generic;

namespace Meeting_App.Data.Database.Tables
{
    public partial class ScheduleTraining
    {
        public long Id { get; set; }
        public int? TrainingId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool? IsDone { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime? CreationDate { get; set; }
        public Guid? UpdatedBy { get; set; }
        public DateTime? UpdationDate { get; set; }
        public int? VenueId { get; set; }
    }
}
