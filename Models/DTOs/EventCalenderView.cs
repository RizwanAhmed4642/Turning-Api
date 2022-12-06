using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Meeting_App.Models.DTOs
{
    public class EventCalenderView
    {
        public int? Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ExternalParticipant { get; set; }
        public string ExternalParticipantsMobileNo { get; set; }
        public DateTime? StartDateTime { get; set; }
        public DateTime? EndDateTime { get; set; }
        public string Attachment { get; set; }
        public bool? RecordStatus { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime? CreationDate { get; set; }
        public Guid? UpdatedBy { get; set; }
        public DateTime? UpdationDate { get; set; }
        public IFormFile AttachmentFile { get; set; }
        public List<IFormFile> MeetingAttachments { get; set; }
        public string Organizer { get; set; }
        public string Venue { get; set; }
        public bool? IsDeleted { get; set; }
        public string MeetingStatus { get; set; }
        public int? MeetingStatusId { get; set; }
        public int? MeetingVenueId { get; set; }
        public int? MeetingOrganizerId { get; set; }
        public string MeetingStatusIdName { get; set; }
        public string MeetingVenueIdName { get; set; }
        public string MeetingOrganizerIdName { get; set; }
        public string EventParticipantData { get; set; }
        public string ExternalParticipantData { get; set; }
        public string MeetingAttendVia { get; set; }
        //public List<TaskAssigneeList> TaskAssignees = new List<TaskAssigneeList>();
        //public List<TaskCcList> TaskCcs = new List<TaskCcList>();

        public List<EventParticipantView> EventParticipant = new List<EventParticipantView>();
    }

    public class EventParticipantView
    {
        public int? Id { get; set; }
        public int? EventId { get; set; }
        public Guid? ParticipantId { get; set; }
        public string ParticipantName { get; set; }
        public string ParticipantFullName { get; set; }
    }

    public class EventFilter
    {
        //public DateTime? StartDate { get; set; }
        //public DateTime? EndDate { get; set; }
        public Guid? UserId { get; set; }
        public bool? RecordStatus { get; set; }
        public bool ShowMyEvent { get; set; }
        public string MeetingStatus { get; set; }
        public bool? IsDeleted { get; set; }
    }



    public class MeetingVenueDTO
    {
        public int Id { get; set; }
        public string Venue { get; set; }
        public int? OderBy { get; set; }
        public bool? RecordStatus { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreationDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdationDate { get; set; }
    }

    public class MeetingOrganizerDTO
    {
        public int Id { get; set; }
        public string Organizer { get; set; }
        public int? OderBy { get; set; }
        public bool? RecordStatus { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreationDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdationDate { get; set; }
    }


    public class DDLMeetingStatusModel
    {
        public int Id { get; set; }
        public string Status { get; set; }
    }


    public class MeetingCount
    {

        public int Total { get; set; }
        public int Active { get; set; }
        public int InActive { get; set; }
        public int Postponed { get; set; }
        public int Rescheduled { get; set; }
        public int Cancelled { get; set; }
       
        public int Archived { get; set; }
    }


    public class MeetingDetailDTO
    {
        //public int Id { get; set; }
        public int MeetingId { get; set; }
       
      
       // public int MeetingStatus { get; set; }
        public int MeetingStatusId { get; set; }
       // public string MeetingStatusName { get; set; }
      
   
    }
}
