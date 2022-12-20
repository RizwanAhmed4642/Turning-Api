using System;
using System.Collections.Generic;

namespace Meeting_App.Data.Database.Tables
{
    public partial class Schedule
    {
        public long Id { get; set; }
        public int? TrainingId { get; set; }
        public int? VenueId { get; set; }
        public DateTime? StartDateTime { get; set; }
        public DateTime? EndDateTime { get; set; }
    }
}
