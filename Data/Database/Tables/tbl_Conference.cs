using System;
using System.Collections.Generic;

namespace Meeting_App.Data.Database.Tables
{
    public partial class tbl_Conference
    {
        public tbl_Conference()
        {
            tbl_ConferenceAttachment = new HashSet<tbl_ConferenceAttachment>();
            tbl_ConferenceParticipant = new HashSet<tbl_ConferenceParticipant>();
        }

        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime? StartDateTime { get; set; }
        public DateTime? EndDateTime { get; set; }
        public bool? RecordStatus { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime? CreationDate { get; set; }
        public Guid? UpdatedBy { get; set; }
        public DateTime? UpdationDate { get; set; }
        public bool? IsDeleted { get; set; }
        public int ConferenceStatusId { get; set; }
        public int? ConferenceOrganizerId { get; set; }
        public int? ConferenceVenueId { get; set; }

        public virtual tbl_MeetingOrganizer ConferenceOrganizer { get; set; }
        public virtual tbl_MeetingStatus ConferenceStatus { get; set; }
        public virtual tbl_MeetingVenue ConferenceVenue { get; set; }
        public virtual ICollection<tbl_ConferenceAttachment> tbl_ConferenceAttachment { get; set; }
        public virtual ICollection<tbl_ConferenceParticipant> tbl_ConferenceParticipant { get; set; }
    }
}
