using System;
using System.Collections.Generic;

namespace Meeting_App.Data.Database.Tables
{
    public partial class tbl_MeetingOrganizedBy
    {
        public int Id { get; set; }
        public int? EventId { get; set; }
        public int? OrganizerId { get; set; }

        public virtual EventCalender Event { get; set; }
        public virtual tbl_MeetingOrganizer Organizer { get; set; }
    }
}
