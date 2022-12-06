using System;
using System.Collections.Generic;

namespace Meeting_App.Data.Database.Tables
{
    public partial class tbl_ConferenceParticipantView
    {
        public int Id { get; set; }
        public int? ConferenceId { get; set; }
        public Guid? ParticipantId { get; set; }
        public string ParticipantName { get; set; }
        public string Designation { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string ConferenceTitle { get; set; }
        public DateTime? StartDateTime { get; set; }
        public string ChairedBy { get; set; }
        public string Venue { get; set; }
        public string ConferenceStatus { get; set; }
        public string Division { get; set; }
        public string District { get; set; }
        public string Tehsil { get; set; }
        public int? AttendanceId { get; set; }
        public DateTime? AttendanceDatetime { get; set; }
    }
}
