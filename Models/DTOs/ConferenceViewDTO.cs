using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Meeting_App.Models.DTOs
{
    public class ConferenceViewDTO
    {
        public int? Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        
        public DateTime? StartDateTime { get; set; }
        public DateTime? EndDateTime { get; set; }
     
        public bool? RecordStatus { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime? CreationDate { get; set; }
        public Guid? UpdatedBy { get; set; }
        public DateTime? UpdationDate { get; set; }
     
        public List<IFormFile> ConferenceAttachments { get; set; }
       
        public bool? IsDeleted { get; set; }
     
        public int ConferenceStatusId { get; set; }
        public int? ConferenceVenueId { get; set; }
        public int? ConferenceOrganizerId { get; set; }
        public string ConferenceStatusIdName { get; set; }
        public string ConferenceVenueIdName { get; set; }
        public string ConferenceOrganizerIdName { get; set; }
        public string ConferenceParticipantData { get; set; }
        

        public List<ConferenceParticipantView> ConferenceParticipant = new List<ConferenceParticipantView>();
    }

    public class ConferenceParticipantView
    {
        public int? Id { get; set; }
        public int? ConferenceId { get; set; }
        public Guid? ParticipantId { get; set; }
        public string ParticipantName { get; set; }
        public string ParticipantFullName { get; set; }
    }

    public class ConferenceFilter
    {
        
        public Guid? UserId { get; set; }
        public bool? RecordStatus { get; set; }
        public bool ShowMyConference { get; set; }
        public string ConferenceStatus { get; set; }
        public bool? IsDeleted { get; set; }
    }



    public class ConferenceVenueDTO
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

    public class ConferenceOrganizerDTO
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


    public class DDLConferenceStatusModel
    {
        public int Id { get; set; }
        public string Status { get; set; }
    }


    public class ConferenceCount
    {

        public int Total { get; set; }
        public int Active { get; set; }
        public int InActive { get; set; }
        public int Postponed { get; set; }
        public int Rescheduled { get; set; }
        public int Cancelled { get; set; }

        public int Archived { get; set; }
    }


    public class ConferenceDetailDTO
    {
       
        public int ConferenceId { get; set; }


        
        public int ConferenceStatusId { get; set; }
     


    }
}

