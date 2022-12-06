using System;
using System.Collections.Generic;

namespace Meeting_App.Data.Database.Tables
{
    public partial class EventParticipant
    {
        public int Id { get; set; }
        public int? EventId { get; set; }
        public Guid? ParticipantId { get; set; }
        public int? ParticipantContactID { get; set; }

        public virtual EventCalender Event { get; set; }
        public virtual AspNetUsers Participant { get; set; }
    }
}
