using System;
using System.Collections.Generic;

namespace Meeting_App.Data.Database.Tables
{
    public partial class EventCalender
    {
        public EventCalender()
        {
            EventParticipant = new HashSet<EventParticipant>();
            MeetingAttachment = new HashSet<MeetingAttachment>();
            tbl_MeetingOrganizedBy = new HashSet<tbl_MeetingOrganizedBy>();
        }

        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime? StartDateTime { get; set; }
        public DateTime? EndDateTime { get; set; }
        public string ExternalParticipants { get; set; }
        public string Attachment { get; set; }
        public bool? RecordStatus { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime? CreationDate { get; set; }
        public Guid? UpdatedBy { get; set; }
        public DateTime? UpdationDate { get; set; }
        public bool? IsDeleted { get; set; }
        public string ExternalParticipantsMobileNo { get; set; }
        public int? MeetingStatusId { get; set; }
        public int? MeetingOrganizerId { get; set; }
        public int? MeetingVenueId { get; set; }
        public string MeetingAttendVia { get; set; }
        public string TrainingType { get; set; }
        public string TraingCategore { get; set; }
        public string Cadre { get; set; }
        public string Departments { get; set; }

        public virtual tbl_MeetingOrganizer MeetingOrganizer { get; set; }
        public virtual tbl_MeetingStatus MeetingStatus { get; set; }
        public virtual tbl_MeetingVenue MeetingVenue { get; set; }
        public virtual ICollection<EventParticipant> EventParticipant { get; set; }
        public virtual ICollection<MeetingAttachment> MeetingAttachment { get; set; }
        public virtual ICollection<tbl_MeetingOrganizedBy> tbl_MeetingOrganizedBy { get; set; }
    }
}
