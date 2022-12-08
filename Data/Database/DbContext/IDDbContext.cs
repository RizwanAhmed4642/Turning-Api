using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Meeting_App.Data.Database.Tables;

namespace Meeting_App.Data.Database.Tables
{
    public partial class IDDbContext : DbContext
    {
        public IDDbContext()
        {
        }

        public IDDbContext(DbContextOptions<IDDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AppLocations> AppLocations { get; set; }
        public virtual DbSet<ApplicationVersion> ApplicationVersion { get; set; }
        public virtual DbSet<AspNetRoleClaims> AspNetRoleClaims { get; set; }
        public virtual DbSet<AspNetRoles> AspNetRoles { get; set; }
        public virtual DbSet<AspNetUserClaims> AspNetUserClaims { get; set; }
        public virtual DbSet<AspNetUserLogins> AspNetUserLogins { get; set; }
        public virtual DbSet<AspNetUserRoles> AspNetUserRoles { get; set; }
        public virtual DbSet<AspNetUserTokens> AspNetUserTokens { get; set; }
        public virtual DbSet<AspNetUsers> AspNetUsers { get; set; }
        public virtual DbSet<Attachment> Attachment { get; set; }
        public virtual DbSet<ContactDepartment> ContactDepartment { get; set; }
        public virtual DbSet<ContactsDetailView> ContactsDetailView { get; set; }
        public virtual DbSet<EventCalender> EventCalender { get; set; }
        public virtual DbSet<EventParticipant> EventParticipant { get; set; }
        public virtual DbSet<Files> Files { get; set; }
        public virtual DbSet<Folders> Folders { get; set; }
        public virtual DbSet<MeetingAttachment> MeetingAttachment { get; set; }
        public virtual DbSet<Notification> Notification { get; set; }
        public virtual DbSet<ViewFileFolderUnionDeleted> ViewFileFolderUnionDeleted { get; set; }
        public virtual DbSet<ViewFilesFolders> ViewFilesFolders { get; set; }
        public virtual DbSet<tbl_Conference> tbl_Conference { get; set; }
        public virtual DbSet<tbl_ConferenceAttachment> tbl_ConferenceAttachment { get; set; }
        public virtual DbSet<tbl_ConferenceAttendance> tbl_ConferenceAttendance { get; set; }
        public virtual DbSet<tbl_ConferenceParticipant> tbl_ConferenceParticipant { get; set; }
        public virtual DbSet<tbl_ConferenceParticipantView> tbl_ConferenceParticipantView { get; set; }
        public virtual DbSet<tbl_ContactCategory> tbl_ContactCategory { get; set; }
        public virtual DbSet<tbl_ContactCompany> tbl_ContactCompany { get; set; }
        public virtual DbSet<tbl_ContactDesignation> tbl_ContactDesignation { get; set; }
        public virtual DbSet<tbl_Contacts> tbl_Contacts { get; set; }
        public virtual DbSet<tbl_DailyEngagement> tbl_DailyEngagement { get; set; }
        public virtual DbSet<tbl_MeetingOrganizedBy> tbl_MeetingOrganizedBy { get; set; }
        public virtual DbSet<tbl_MeetingOrganizer> tbl_MeetingOrganizer { get; set; }
        public virtual DbSet<tbl_MeetingStatus> tbl_MeetingStatus { get; set; }
        public virtual DbSet<tbl_MeetingVenue> tbl_MeetingVenue { get; set; }
        public virtual DbSet<tbl_Status> tbl_Status { get; set; }
        public virtual DbSet<tbl_Task> tbl_Task { get; set; }
        public virtual DbSet<tbl_TaskAssignee> tbl_TaskAssignee { get; set; }
        public virtual DbSet<tbl_TaskCc> tbl_TaskCc { get; set; }
        public virtual DbSet<tbl_TaskDetails> tbl_TaskDetails { get; set; }
        public virtual DbSet<tbl_UserFP> tbl_UserFP { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Initial Catalog=TrainingAPPDB;user id=muddasir;password=abc@123;Data Source=172.16.15.5");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AppLocations>(entity =>
            {
                entity.Property(e => e.Category).HasMaxLength(50);

                entity.Property(e => e.CategoryCode).HasMaxLength(50);

                entity.Property(e => e.Code).HasMaxLength(50);

                entity.Property(e => e.ContectPerson).HasMaxLength(500);

                entity.Property(e => e.Email).HasMaxLength(500);

                entity.Property(e => e.IsCovidFacility).HasMaxLength(50);

                entity.Property(e => e.Latitude).HasColumnType("decimal(9, 6)");

                entity.Property(e => e.Longitude).HasColumnType("decimal(9, 6)");

                entity.Property(e => e.Name).HasMaxLength(250);

                entity.Property(e => e.NameUrdu).HasMaxLength(200);

                entity.Property(e => e.OldCode).HasMaxLength(50);

                entity.Property(e => e.PhoneNo).HasMaxLength(50);

                entity.Property(e => e.Type).HasMaxLength(50);
            });

            modelBuilder.Entity<ApplicationVersion>(entity =>
            {
                entity.Property(e => e.App_Version).HasMaxLength(50);

                entity.Property(e => e.Json_Version).HasMaxLength(50);
            });

            modelBuilder.Entity<AspNetRoleClaims>(entity =>
            {
                entity.HasIndex(e => e.RoleId);

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.AspNetRoleClaims)
                    .HasForeignKey(d => d.RoleId);
            });

            modelBuilder.Entity<AspNetRoles>(entity =>
            {
                entity.HasIndex(e => e.NormalizedName)
                    .HasName("RoleNameIndex")
                    .IsUnique()
                    .HasFilter("([NormalizedName] IS NOT NULL)");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Name).HasMaxLength(256);

                entity.Property(e => e.NormalizedName).HasMaxLength(256);
            });

            modelBuilder.Entity<AspNetUserClaims>(entity =>
            {
                entity.HasIndex(e => e.UserId);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserClaims)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUserLogins>(entity =>
            {
                entity.HasKey(e => new { e.LoginProvider, e.ProviderKey });

                entity.HasIndex(e => e.UserId);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserLogins)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUserRoles>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.RoleId });

                entity.HasIndex(e => e.RoleId);

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.AspNetUserRoles)
                    .HasForeignKey(d => d.RoleId);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserRoles)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUserTokens>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.LoginProvider, e.Name });

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserTokens)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUsers>(entity =>
            {
                entity.HasIndex(e => e.NormalizedEmail)
                    .HasName("EmailIndex");

                entity.HasIndex(e => e.NormalizedUserName)
                    .HasName("UserNameIndex")
                    .IsUnique()
                    .HasFilter("([NormalizedUserName] IS NOT NULL)");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Email).HasMaxLength(256);

                entity.Property(e => e.NormalizedEmail).HasMaxLength(256);

                entity.Property(e => e.NormalizedUserName).HasMaxLength(256);

                entity.Property(e => e.UserName).HasMaxLength(256);
            });

            modelBuilder.Entity<Attachment>(entity =>
            {
                entity.Property(e => e.SourceName).HasMaxLength(50);

                entity.HasOne(d => d.TaskDetail)
                    .WithMany(p => p.Attachment)
                    .HasForeignKey(d => d.TaskDetailId)
                    .HasConstraintName("FK_Attachment_tbl_TaskDetails");

                entity.HasOne(d => d.Task)
                    .WithMany(p => p.Attachment)
                    .HasForeignKey(d => d.TaskId)
                    .HasConstraintName("FK_Attachment_tbl_Task");
            });

            modelBuilder.Entity<ContactDepartment>(entity =>
            {
                entity.Property(e => e.CreatedBy).HasMaxLength(500);

                entity.Property(e => e.CreationDate).HasColumnType("datetime");

                entity.Property(e => e.DepartmentName)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.UpdatedBy).HasMaxLength(500);

                entity.Property(e => e.UpdationDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<ContactsDetailView>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("ContactsDetailView");

                entity.Property(e => e.MobileNo).HasMaxLength(15);

                entity.Property(e => e.companyName).HasMaxLength(50);

                entity.Property(e => e.departmentName)
                    .HasMaxLength(250)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<EventCalender>(entity =>
            {
                entity.Property(e => e.Attachment).HasMaxLength(500);

                entity.Property(e => e.CreationDate).HasColumnType("datetime");

                entity.Property(e => e.EndDateTime).HasColumnType("datetime");

                entity.Property(e => e.MeetingAttendVia).HasMaxLength(50);

                entity.Property(e => e.StartDateTime).HasColumnType("datetime");

                entity.Property(e => e.Title).HasMaxLength(500);

                entity.Property(e => e.UpdationDate).HasColumnType("datetime");

                entity.HasOne(d => d.MeetingOrganizer)
                    .WithMany(p => p.EventCalender)
                    .HasForeignKey(d => d.MeetingOrganizerId)
                    .HasConstraintName("FK_EventCalender_tbl.MeetingOrganizer");

                entity.HasOne(d => d.MeetingStatus)
                    .WithMany(p => p.EventCalender)
                    .HasForeignKey(d => d.MeetingStatusId)
                    .HasConstraintName("FK_EventCalender_tbl_MeetingStatus");

                entity.HasOne(d => d.MeetingVenue)
                    .WithMany(p => p.EventCalender)
                    .HasForeignKey(d => d.MeetingVenueId)
                    .HasConstraintName("FK_EventCalender_tbl.MeetingVenue");
            });

            modelBuilder.Entity<EventParticipant>(entity =>
            {
                entity.HasOne(d => d.Event)
                    .WithMany(p => p.EventParticipant)
                    .HasForeignKey(d => d.EventId)
                    .HasConstraintName("FK_EventParticipant_EventCalender");

                entity.HasOne(d => d.Participant)
                    .WithMany(p => p.EventParticipant)
                    .HasForeignKey(d => d.ParticipantId)
                    .HasConstraintName("FK_EventParticipant_AspNetUsers");
            });

            modelBuilder.Entity<Files>(entity =>
            {
                entity.HasKey(e => e.PkCode);

                entity.Property(e => e.CreatedBy).HasMaxLength(128);

                entity.Property(e => e.DeletedBy).HasMaxLength(128);

                entity.Property(e => e.Description).HasMaxLength(500);

                entity.Property(e => e.Extension).HasMaxLength(500);

                entity.Property(e => e.FileSize).HasMaxLength(50);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.Path).HasMaxLength(500);

                entity.Property(e => e.UpdatedBy).HasMaxLength(128);

                entity.HasOne(d => d.Fk_FolderNavigation)
                    .WithMany(p => p.Files)
                    .HasForeignKey(d => d.Fk_Folder)
                    .HasConstraintName("FK_Files_Folders");
            });

            modelBuilder.Entity<Folders>(entity =>
            {
                entity.HasKey(e => e.PkCode);

                entity.Property(e => e.CreatedBy).HasMaxLength(128);

                entity.Property(e => e.DeletedBy).HasMaxLength(128);

                entity.Property(e => e.Description).HasMaxLength(500);

                entity.Property(e => e.Name).HasMaxLength(500);

                entity.Property(e => e.UpdatedBy).HasMaxLength(128);

                entity.HasOne(d => d.Fk_Parent)
                    .WithMany(p => p.InverseFk_Parent)
                    .HasForeignKey(d => d.Fk_ParentId)
                    .HasConstraintName("FK_Folders_Folders");
            });

            modelBuilder.Entity<MeetingAttachment>(entity =>
            {
                entity.Property(e => e.SourceName).HasMaxLength(50);

                entity.HasOne(d => d.Meeting)
                    .WithMany(p => p.MeetingAttachment)
                    .HasForeignKey(d => d.MeetingId)
                    .HasConstraintName("FK_MeetingAttachment_EventCalender");
            });

            modelBuilder.Entity<Notification>(entity =>
            {
                entity.Property(e => e.SourceId).HasMaxLength(50);

                entity.Property(e => e.SourceType).HasMaxLength(50);

                entity.Property(e => e.Title).HasMaxLength(50);

                entity.Property(e => e.Type).HasMaxLength(50);

                entity.HasOne(d => d.UserIdFromNavigation)
                    .WithMany(p => p.NotificationUserIdFromNavigation)
                    .HasForeignKey(d => d.UserIdFrom)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Notification_AspNetUsers");

                entity.HasOne(d => d.UserIdToNavigation)
                    .WithMany(p => p.NotificationUserIdToNavigation)
                    .HasForeignKey(d => d.UserIdTo)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Notification_AspNetUsers1");
            });

            modelBuilder.Entity<ViewFileFolderUnionDeleted>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("ViewFileFolderUnionDeleted");

                entity.Property(e => e.CreatedBy).HasMaxLength(128);

                entity.Property(e => e.DeletedBy).HasMaxLength(128);

                entity.Property(e => e.DeletedByUser).HasMaxLength(256);

                entity.Property(e => e.ItemType)
                    .IsRequired()
                    .HasMaxLength(6)
                    .IsUnicode(false);

                entity.Property(e => e.Name).HasMaxLength(500);

                entity.Property(e => e.UpdatedBy).HasMaxLength(128);
            });

            modelBuilder.Entity<ViewFilesFolders>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("ViewFilesFolders");

                entity.Property(e => e.FileCreatedBy).HasMaxLength(128);

                entity.Property(e => e.FileCreaterUserName).HasMaxLength(256);

                entity.Property(e => e.FileDescripion).HasMaxLength(500);

                entity.Property(e => e.FileExtension).HasMaxLength(10);

                entity.Property(e => e.FileName).HasMaxLength(500);

                entity.Property(e => e.FilePath).HasMaxLength(500);

                entity.Property(e => e.FileSize)
                    .HasMaxLength(100)
                    .IsFixedLength();

                entity.Property(e => e.FolderCreatedBy).HasMaxLength(128);

                entity.Property(e => e.FolderCreaterUserName).HasMaxLength(256);

                entity.Property(e => e.FolderDescripion).HasMaxLength(500);

                entity.Property(e => e.FolderName).HasMaxLength(500);
            });

            modelBuilder.Entity<tbl_Conference>(entity =>
            {
                entity.Property(e => e.CreationDate).HasColumnType("datetime");

                entity.Property(e => e.EndDateTime).HasColumnType("datetime");

                entity.Property(e => e.StartDateTime).HasColumnType("datetime");

                entity.Property(e => e.Title).HasMaxLength(500);

                entity.Property(e => e.UpdationDate).HasColumnType("datetime");

                entity.HasOne(d => d.ConferenceOrganizer)
                    .WithMany(p => p.tbl_Conference)
                    .HasForeignKey(d => d.ConferenceOrganizerId)
                    .HasConstraintName("FK_tbl_Conference_tbl_MeetingOrganizer");

                entity.HasOne(d => d.ConferenceStatus)
                    .WithMany(p => p.tbl_Conference)
                    .HasForeignKey(d => d.ConferenceStatusId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tbl_Conference_tbl_MeetingStatus");

                entity.HasOne(d => d.ConferenceVenue)
                    .WithMany(p => p.tbl_Conference)
                    .HasForeignKey(d => d.ConferenceVenueId)
                    .HasConstraintName("FK_tbl_Conference_tbl_MeetingVenue");
            });

            modelBuilder.Entity<tbl_ConferenceAttachment>(entity =>
            {
                entity.Property(e => e.SourceName).HasMaxLength(50);

                entity.HasOne(d => d.Conference)
                    .WithMany(p => p.tbl_ConferenceAttachment)
                    .HasForeignKey(d => d.ConferenceId)
                    .HasConstraintName("FK_tbl_ConferenceAttachment_tbl_Conference");
            });

            modelBuilder.Entity<tbl_ConferenceAttendance>(entity =>
            {
                entity.Property(e => e.CreatedBy).HasMaxLength(150);

                entity.Property(e => e.CreatedByUserId).HasMaxLength(150);

                entity.Property(e => e.ParticipantId).HasMaxLength(150);
            });

            modelBuilder.Entity<tbl_ConferenceParticipant>(entity =>
            {
                entity.HasOne(d => d.Conference)
                    .WithMany(p => p.tbl_ConferenceParticipant)
                    .HasForeignKey(d => d.ConferenceId)
                    .HasConstraintName("FK_tbl_ConferenceParticipant_tbl_Conference");

                entity.HasOne(d => d.Participant)
                    .WithMany(p => p.tbl_ConferenceParticipant)
                    .HasForeignKey(d => d.ParticipantId)
                    .HasConstraintName("FK_tbl_ConferenceParticipant_AspNetUsers");
            });

            modelBuilder.Entity<tbl_ConferenceParticipantView>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("tbl_ConferenceParticipantView");

                entity.Property(e => e.ConferenceStatus).HasMaxLength(200);

                entity.Property(e => e.ConferenceTitle).HasMaxLength(500);

                entity.Property(e => e.Email).HasMaxLength(256);

                entity.Property(e => e.StartDateTime).HasColumnType("datetime");
            });

            modelBuilder.Entity<tbl_ContactCategory>(entity =>
            {
                entity.Property(e => e.CreatedBy).HasMaxLength(500);

                entity.Property(e => e.CreationDate).HasColumnType("datetime");

                entity.Property(e => e.UpdatedBy).HasMaxLength(500);

                entity.Property(e => e.UpdationDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<tbl_ContactCompany>(entity =>
            {
                entity.Property(e => e.CompanyName).HasMaxLength(50);

                entity.Property(e => e.CreatedBy).HasMaxLength(500);

                entity.Property(e => e.CreationDate).HasColumnType("datetime");

                entity.Property(e => e.UpdatedBy).HasMaxLength(500);

                entity.Property(e => e.UpdationDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<tbl_ContactDesignation>(entity =>
            {
                entity.Property(e => e.CategoryCode).HasMaxLength(50);

                entity.Property(e => e.CreatedBy).HasMaxLength(500);

                entity.Property(e => e.CreationDate).HasColumnType("datetime");

                entity.Property(e => e.UpdatedBy).HasMaxLength(500);

                entity.Property(e => e.UpdationDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<tbl_Contacts>(entity =>
            {
                entity.Property(e => e.CNIC).HasMaxLength(13);

                entity.Property(e => e.CreatedBy).HasMaxLength(500);

                entity.Property(e => e.CreationDate).HasColumnType("datetime");

                entity.Property(e => e.DateOfBirth).HasColumnType("date");

                entity.Property(e => e.Designation).HasMaxLength(50);

                entity.Property(e => e.Gender)
                    .HasMaxLength(10)
                    .IsFixedLength();

                entity.Property(e => e.MobileNo).HasMaxLength(15);

                entity.Property(e => e.PhoneNo).HasMaxLength(15);

                entity.Property(e => e.Title).HasMaxLength(100);

                entity.Property(e => e.UpdatedBy).HasMaxLength(500);

                entity.Property(e => e.UpdationDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<tbl_DailyEngagement>(entity =>
            {
                entity.Property(e => e.CreatedBy).HasMaxLength(500);

                entity.Property(e => e.CreationDate).HasColumnType("datetime");

                entity.Property(e => e.EngagementSatus).HasMaxLength(500);

                entity.Property(e => e.UpdatedBy).HasMaxLength(500);

                entity.Property(e => e.UpdationDate).HasColumnType("datetime");

                entity.Property(e => e.WhenToGo).HasColumnType("datetime");

                entity.HasOne(d => d.EngagementSatusNavigation)
                    .WithMany(p => p.tbl_DailyEngagement)
                    .HasForeignKey(d => d.EngagementSatusId)
                    .HasConstraintName("FK_tbl_DailyEngagement_tbl_MeetingStatus");

                entity.HasOne(d => d.Venue)
                    .WithMany(p => p.tbl_DailyEngagement)
                    .HasForeignKey(d => d.VenueId)
                    .HasConstraintName("FK_tbl_DailyEngagement_tbl_MeetingVenue");
            });

            modelBuilder.Entity<tbl_MeetingOrganizedBy>(entity =>
            {
                entity.HasOne(d => d.Event)
                    .WithMany(p => p.tbl_MeetingOrganizedBy)
                    .HasForeignKey(d => d.EventId)
                    .HasConstraintName("FK_tbl_MeetingOrganizedBy_EventCalender");

                entity.HasOne(d => d.Organizer)
                    .WithMany(p => p.tbl_MeetingOrganizedBy)
                    .HasForeignKey(d => d.OrganizerId)
                    .HasConstraintName("FK_tbl_MeetingOrganizedBy_tbl_MeetingOrganizedBy");
            });

            modelBuilder.Entity<tbl_MeetingOrganizer>(entity =>
            {
                entity.Property(e => e.CreatedBy).HasMaxLength(500);

                entity.Property(e => e.CreationDate).HasColumnType("datetime");

                entity.Property(e => e.UpdatedBy).HasMaxLength(500);

                entity.Property(e => e.UpdationDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<tbl_MeetingStatus>(entity =>
            {
                entity.Property(e => e.Status).HasMaxLength(200);
            });

            modelBuilder.Entity<tbl_MeetingVenue>(entity =>
            {
                entity.Property(e => e.CreatedBy).HasMaxLength(500);

                entity.Property(e => e.CreationDate).HasColumnType("datetime");

                entity.Property(e => e.UpdatedBy).HasMaxLength(500);

                entity.Property(e => e.UpdationDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<tbl_Status>(entity =>
            {
                entity.Property(e => e.Status).HasMaxLength(200);
            });

            modelBuilder.Entity<tbl_Task>(entity =>
            {
                entity.Property(e => e.CreatedBy).HasMaxLength(500);

                entity.Property(e => e.CreationDate).HasColumnType("datetime");

                entity.Property(e => e.Designation).HasMaxLength(50);

                entity.Property(e => e.DueDate).HasColumnType("datetime");

                entity.Property(e => e.ExtendedDate).HasColumnType("datetime");

                entity.Property(e => e.Priority).HasMaxLength(50);

                entity.Property(e => e.Title).HasMaxLength(100);

                entity.Property(e => e.UpdatedBy).HasMaxLength(500);

                entity.Property(e => e.UpdationDate).HasColumnType("datetime");

                entity.HasOne(d => d.TaskStatusNavigation)
                    .WithMany(p => p.tbl_Task)
                    .HasForeignKey(d => d.TaskStatus)
                    .HasConstraintName("FK_tbl_Task_tbl_Status");
            });

            modelBuilder.Entity<tbl_TaskAssignee>(entity =>
            {
                entity.HasOne(d => d.TaskAssignTo)
                    .WithMany(p => p.tbl_TaskAssignee)
                    .HasForeignKey(d => d.TaskAssignToID)
                    .HasConstraintName("FK_tbl_TaskAssignee_AspNetUsers");

                entity.HasOne(d => d.Task)
                    .WithMany(p => p.tbl_TaskAssignee)
                    .HasForeignKey(d => d.TaskId)
                    .HasConstraintName("FK_tbl_TaskAssignee_tbl_Task");
            });

            modelBuilder.Entity<tbl_TaskCc>(entity =>
            {
                entity.HasOne(d => d.TaskCc)
                    .WithMany(p => p.tbl_TaskCc)
                    .HasForeignKey(d => d.TaskCcID)
                    .HasConstraintName("FK_tbl_TaskCc_AspNetUsers");

                entity.HasOne(d => d.Task)
                    .WithMany(p => p.tbl_TaskCc)
                    .HasForeignKey(d => d.TaskId)
                    .HasConstraintName("FK_tbl_TaskCc_tbl_Task");
            });

            modelBuilder.Entity<tbl_TaskDetails>(entity =>
            {
                entity.Property(e => e.CommentFor).HasMaxLength(500);

                entity.Property(e => e.CreatedBy).HasMaxLength(50);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.ReadDateTime).HasColumnType("datetime");

                entity.HasOne(d => d.Task)
                    .WithMany(p => p.tbl_TaskDetails)
                    .HasForeignKey(d => d.TaskId)
                    .HasConstraintName("FK_tbl_TaskDetails_tbl_Task");
            });

            modelBuilder.Entity<tbl_UserFP>(entity =>
            {
                entity.Property(e => e.CreatedBy).HasMaxLength(150);

                entity.Property(e => e.CreatedByUserId).HasMaxLength(150);

                entity.Property(e => e.UserId).HasMaxLength(150);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
