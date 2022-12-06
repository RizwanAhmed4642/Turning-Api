using System;
using System.Collections.Generic;

namespace Meeting_App.Data.Database.Tables
{
    public partial class tbl_ConferenceParticipant
    {
        public int Id { get; set; }
        public int? ConferenceId { get; set; }
        public Guid? ParticipantId { get; set; }

        public virtual tbl_Conference Conference { get; set; }
        public virtual AspNetUsers Participant { get; set; }
    }
}
