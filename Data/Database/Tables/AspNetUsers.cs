using System;
using System.Collections.Generic;

namespace Meeting_App.Data.Database.Tables
{
    public partial class AspNetUsers
    {
        public AspNetUsers()
        {
            AspNetUserClaims = new HashSet<AspNetUserClaims>();
            AspNetUserLogins = new HashSet<AspNetUserLogins>();
            AspNetUserRoles = new HashSet<AspNetUserRoles>();
            AspNetUserTokens = new HashSet<AspNetUserTokens>();
            EventParticipant = new HashSet<EventParticipant>();
            NotificationUserIdFromNavigation = new HashSet<Notification>();
            NotificationUserIdToNavigation = new HashSet<Notification>();
            tbl_ConferenceParticipant = new HashSet<tbl_ConferenceParticipant>();
            tbl_TaskAssignee = new HashSet<tbl_TaskAssignee>();
            tbl_TaskCc = new HashSet<tbl_TaskCc>();
        }

        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string NormalizedUserName { get; set; }
        public string Email { get; set; }
        public string NormalizedEmail { get; set; }
        public bool EmailConfirmed { get; set; }
        public string PasswordHash { get; set; }
        public string SecurityStamp { get; set; }
        public string ConcurrencyStamp { get; set; }
        public string PhoneNumber { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public bool TwoFactorEnabled { get; set; }
        public DateTimeOffset? LockoutEnd { get; set; }
        public bool LockoutEnabled { get; set; }
        public int AccessFailedCount { get; set; }
        public string NP { get; set; }
        public string Designation { get; set; }
        public string FullName { get; set; }
        public int Order { get; set; }
        public int ContactId { get; set; }
        public bool? IsDeleted { get; set; }
        public bool? IsRecordStatus { get; set; }
        public int? ContactDesignationId { get; set; }
        public int? ContactCategoryId { get; set; }
        public int? ContactCompanyId { get; set; }
        public int? ContactDepartmentId { get; set; }
        public string ProfilePic { get; set; }

        public virtual ICollection<AspNetUserClaims> AspNetUserClaims { get; set; }
        public virtual ICollection<AspNetUserLogins> AspNetUserLogins { get; set; }
        public virtual ICollection<AspNetUserRoles> AspNetUserRoles { get; set; }
        public virtual ICollection<AspNetUserTokens> AspNetUserTokens { get; set; }
        public virtual ICollection<EventParticipant> EventParticipant { get; set; }
        public virtual ICollection<Notification> NotificationUserIdFromNavigation { get; set; }
        public virtual ICollection<Notification> NotificationUserIdToNavigation { get; set; }
        public virtual ICollection<tbl_ConferenceParticipant> tbl_ConferenceParticipant { get; set; }
        public virtual ICollection<tbl_TaskAssignee> tbl_TaskAssignee { get; set; }
        public virtual ICollection<tbl_TaskCc> tbl_TaskCc { get; set; }
    }
}
