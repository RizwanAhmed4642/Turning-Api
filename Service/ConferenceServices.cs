using AutoMapper;
//using DPUruNet;
using Meeting_App.Data.Database.Context;
using Meeting_App.Data.Database.Tables;
using Meeting_App.Models;
using Meeting_App.Models.DTOs;
using Meeting_App.Service.Common;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Meeting_App.Service
{
    public class ConferenceServices
    {
        private CommonService _cService;

        #region Fields
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _env;
        private readonly NotificationService _notificationService = new NotificationService();
        #endregion

        #region ConstructorsAddConference
        public ConferenceServices(IMapper mapper, IWebHostEnvironment env)
        {

            _mapper = mapper;
            _env = env;
        }
        #endregion

        #region AddConference
        public async Task<ConferenceViewDTO> AddConference(ConferenceViewDTO model, string userid)
        {

            using (var db = new IDDbContext())
            {
                using (var trans = db.Database.BeginTransaction())
                {
                    try
                    {
                        var conference = new tbl_Conference();

                        conference.Title = model.Title;
                        conference.Description = model.Description;
                        conference.StartDateTime = model.StartDateTime;
                        //conference.EndDateTime = model.EndDateTime;

                        conference.ConferenceVenueId = model.ConferenceVenueId;

                        conference.ConferenceOrganizerId = model.ConferenceOrganizerId;

                        conference.ConferenceStatusId = model.ConferenceStatusId;
                        if (conference.ConferenceStatusId == '1')
                        {
                            conference.RecordStatus = true;


                        }
                        else if (conference.ConferenceStatusId == '2')
                        {
                            conference.RecordStatus = false;
                        }
                        else
                        {
                            conference.RecordStatus = true;

                        }
                        conference.CreatedBy = Guid.Parse(userid);
                        conference.IsDeleted = false;
                        conference.CreationDate = UtilService.GetPkCurrentDateTime();






                        await db.tbl_Conference.AddAsync(conference);
                        db.SaveChanges();

                        var OrganizerId = conference.ConferenceOrganizerId;
                        var OrganizerName = db.tbl_MeetingOrganizer.Where(x => x.Id == OrganizerId)?.FirstOrDefault()?.Organizer;
                        var VenueId = conference.ConferenceVenueId;
                        var VenueName = db.tbl_MeetingVenue.Where(x => x.Id == VenueId)?.FirstOrDefault()?.Venue;

                        if (model.ConferenceAttachments != null)
                        {
                            foreach (var item in model.ConferenceAttachments)
                            {
                                var newAttachment = new tbl_ConferenceAttachment();
                                newAttachment.AttachmentName = await UploadFile(item);
                                newAttachment.ConferenceId = conference.Id;
                                newAttachment.SourceName = "Meeting";
                                newAttachment.RecordStatus = true;

                                db.tbl_ConferenceAttachment.Add(newAttachment);
                                db.SaveChanges();
                            }
                        }
                        var PIDs = model.ConferenceParticipant.Select(x => x.ParticipantId).ToList();
                       // var Parts = db.AspNetUsers.Where(x => PIDs.Contains(x.Id)).Select(x => x.Designation).ToList();
                        var Parts = db.AspNetUsers.Where(x => PIDs.Contains(x.Id)).Select(x=>x.FullName).ToList();
                        var ParticipantsJoin = String.Join(",", Parts.Select(x => x.ToString()).ToArray());
                        foreach (var p in model.ConferenceParticipant)
                        {
                            tbl_ConferenceParticipant conferenceAssignee = new tbl_ConferenceParticipant();
                            conferenceAssignee.ConferenceId = conference.Id;
                            conferenceAssignee.ParticipantId = p.ParticipantId;
                            db.tbl_ConferenceParticipant.Add(conferenceAssignee);
                            db.SaveChanges();
                            var user = db.AspNetUsers.FirstOrDefault(a => a.ContactId == p.Id);


                            //-------------conferenceAssignee User Role------------
                            var userch = db.AspNetUsers.FirstOrDefault(a => a.Id == p.ParticipantId);
                            if (userch != null)
                            {
                                var Role = db.AspNetRoles.FirstOrDefault(a => a.Name.ToUpper() == "CONFERENCEVIEW");
                                if (Role != null)
                                {
                                    var UserRoles = db.AspNetUserRoles.FirstOrDefault(a => a.UserId == userch.Id && a.RoleId == Role.Id);
                                    if (UserRoles == null)
                                    {
                                        var AssignRole = new AspNetUserRoles();
                                        AssignRole.UserId = userch.Id;
                                        AssignRole.RoleId = Role.Id;
                                        db.AspNetUserRoles.Add(AssignRole);
                                    }
                                }
                            }
                            //-------------conferenceAssignee User Role------------
                            _notificationService.AddNotification(new NotificationModel
                            {
                                Title = "New Meeting Assigned!",
                                Description = "You have been assigned a new Meetnig.",
                                UserIdFrom = Guid.Parse(conference.CreatedBy.ToString()),
                                UserIdTo = (Guid)p.ParticipantId,
                                Link = "/taskDetail",
                                SourceId = conference.Id.ToString(),
                                SourceType = "Meeting",
                                RecordStatus = true

                            });





                            try
                            {
                                SendSMSUfone(new SMSViewModel
                                {
                                    Body = $"Dear Sir/Madam, You are requested to attend the following Conference " +
                               $"\nMeeting Title : {conference.Title}" +
                               $"\nChaired By : {OrganizerName} " +
                               $"\nDate Time : {conference.StartDateTime}" +
                               $"\nVenue :  {VenueName} " +
                                $"\nParticipants :{ ParticipantsJoin}",

                                    Receiver = db.AspNetUsers.FirstOrDefault(x => x.Id == p.ParticipantId).PhoneNumber

                                });

                                var task = db.tbl_Conference.Where(x => x.Id == conference.Id).FirstOrDefault();

                                //  task.IsSMSSent = true;

                                db.SaveChanges();
                            }
                            catch (Exception)
                            {

                                var task = db.tbl_Conference.Where(x => x.Id == conference.Id).FirstOrDefault();

                                //task.IsSMSSent = false;

                                db.SaveChanges();
                            }






                        }



                        model.Id = conference.Id;
                        trans.Commit();
                        return model;

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                        trans.Rollback();
                        throw;
                    }
                }


            }
        }
        #endregion
        #region UpdateConference
        public async Task<ConferenceViewDTO> UpdateConference(ConferenceViewDTO model, string userid)
        {

            using (var db = new IDDbContext())
            {
                using (var trans = db.Database.BeginTransaction())
                {
                    try
                    {
                        var conference = db.tbl_Conference.Include(x => x.tbl_ConferenceParticipant).FirstOrDefault(x => x.Id == model.Id);
                        db.tbl_ConferenceParticipant.RemoveRange(conference.tbl_ConferenceParticipant);
                        db.SaveChanges();

                        conference.Title = model.Title;
                        conference.StartDateTime = model.StartDateTime;
                        //conference.EndDateTime = model.EndDateTime;
                        conference.Description = model.Description;
                        conference.UpdatedBy = Guid.Parse(userid);

                        conference.ConferenceVenueId = model.ConferenceVenueId;

                        conference.ConferenceOrganizerId = model.ConferenceOrganizerId;

                        conference.ConferenceStatusId = model.ConferenceStatusId;
                        if (conference.ConferenceStatusId == '1')
                        {
                            conference.RecordStatus = true;


                        }
                        else if (conference.ConferenceStatusId == '2')
                        {
                            conference.RecordStatus = false;
                        }
                        else
                        {
                            conference.RecordStatus = true;

                        }
                        conference.IsDeleted = false;
                        conference.UpdationDate = UtilService.GetPkCurrentDateTime();







                        db.SaveChanges();

                        var OrganizerId = conference.ConferenceOrganizerId;
                        var OrganizerName = db.tbl_MeetingOrganizer.Where(x => x.Id == OrganizerId)?.FirstOrDefault()?.Organizer;
                        var VenueId = conference.ConferenceVenueId;
                        var VenueName = db.tbl_MeetingVenue.Where(x => x.Id == VenueId)?.FirstOrDefault()?.Venue;

                        if (model.ConferenceAttachments != null)
                        {
                            foreach (var item in model.ConferenceAttachments)
                            {
                                var newAttachment = new tbl_ConferenceAttachment();
                                newAttachment.AttachmentName = await UploadFile(item);
                                newAttachment.ConferenceId = conference.Id;
                                newAttachment.SourceName = "Meeting";
                                newAttachment.RecordStatus = true;

                                db.tbl_ConferenceAttachment.Add(newAttachment);
                                db.SaveChanges();
                            }
                        }


                        var PIDs = model.ConferenceParticipant.Select(x => x.ParticipantId).ToList();
                       // var Parts = db.AspNetUsers.Where(x => PIDs.Contains(x.Id)).Select(x => x.Designation).ToList();
                        var Parts = db.AspNetUsers.Where(x => PIDs.Contains(x.Id)).Select(x => x.FullName).ToList();
                        var ParticipantsJoin = String.Join(",", Parts.Select(x => x.ToString()).ToArray());
                        foreach (var p in model.ConferenceParticipant)
                        {
                            tbl_ConferenceParticipant conferenceAssignee = new tbl_ConferenceParticipant();
                            conferenceAssignee.ConferenceId = conference.Id;
                            conferenceAssignee.ParticipantId = p.ParticipantId;
                            db.tbl_ConferenceParticipant.Add(conferenceAssignee);
                            db.SaveChanges();
                            //-------------conferenceAssignee User Role------------
                            var userch = db.AspNetUsers.FirstOrDefault(a => a.Id == p.ParticipantId);
                            if (userch != null)
                            {
                                var Role = db.AspNetRoles.FirstOrDefault(a => a.Name.ToUpper() == "CONFERENCEVIEW");
                                if (Role != null)
                                {
                                    var UserRoles = db.AspNetUserRoles.FirstOrDefault(a => a.UserId == userch.Id && a.RoleId == Role.Id);
                                    if (UserRoles == null)
                                    {
                                        var AssignRole = new AspNetUserRoles();
                                        AssignRole.UserId = userch.Id;
                                        AssignRole.RoleId = Role.Id;
                                        db.AspNetUserRoles.Add(AssignRole);
                                    }
                                }
                            }
                            //-------------conferenceAssignee User Role------------
                            _notificationService.AddNotification(new NotificationModel
                            {
                                Title = "New Meeting Assigned!",
                                Description = "You have been assigned a new Meetnig.",
                                UserIdFrom = Guid.Parse(conference.CreatedBy.ToString()),
                                UserIdTo = (Guid)p.ParticipantId,
                                Link = "/taskDetail",
                                SourceId = conference.Id.ToString(),
                                SourceType = "Meeting",
                                RecordStatus = true

                            });

                            if (conference.ConferenceStatusId == 1)
                            {



                                try
                                {
                                    SendSMSUfone(new SMSViewModel
                                    {
                                        Body = $"Dear Sir/Madam, You are requested to attend the following Conference " +
                                   $"\nConference Title : {conference.Title}" +
                                   $"\nChaired By : {OrganizerName} " +
                                   $"\nDate Time : {conference.StartDateTime}" +
                                   $"\nVenue :  {VenueName} " +
                                    $"\nParticipants :{ ParticipantsJoin}",

                                        Receiver = db.AspNetUsers.FirstOrDefault(x => x.Id == p.ParticipantId).PhoneNumber

                                    });

                                    var task = db.tbl_Conference.Where(x => x.Id == conference.Id).FirstOrDefault();

                                    //  task.IsSMSSent = true;

                                    db.SaveChanges();
                                }
                                catch (Exception)
                                {

                                    var task = db.tbl_Conference.Where(x => x.Id == conference.Id).FirstOrDefault();

                                    //task.IsSMSSent = false;

                                    db.SaveChanges();
                                }


                            }

                            else if (conference.ConferenceStatusId == 3)
                            {



                                try
                                {
                                    SendSMSUfone(new SMSViewModel
                                    {
                                        Body = $"Dear Sir/Madam, You are informed that following Conference is postponed" +
                                   $"\nConference Title : {conference.Title}" +
                                   $"\nChaired By : {OrganizerName} " +
                                   $"\nDate Time : {conference.StartDateTime}" +
                                   $"\nVenue :  {VenueName} " +
                                    $"\nParticipants :{ ParticipantsJoin}",

                                        Receiver = db.AspNetUsers.FirstOrDefault(x => x.Id == p.ParticipantId).PhoneNumber

                                    });

                                    var task = db.tbl_Conference.Where(x => x.Id == conference.Id).FirstOrDefault();

                                    //  task.IsSMSSent = true;

                                    db.SaveChanges();
                                }
                                catch (Exception)
                                {

                                    var task = db.tbl_Conference.Where(x => x.Id == conference.Id).FirstOrDefault();

                                    //task.IsSMSSent = false;

                                    db.SaveChanges();
                                }


                            }


                            else if (conference.ConferenceStatusId == 4)
                            {



                                try
                                {
                                    SendSMSUfone(new SMSViewModel
                                    {
                                        Body = $"Dear Sir/Madam, You are informed that following Conference is reschduled" +
                                   $"\nConference Title : {conference.Title}" +
                                   $"\nChaired By : {OrganizerName} " +
                                   $"\nDate Time : {conference.StartDateTime}" +
                                   $"\nVenue :  {VenueName} " +
                                    $"\nParticipants :{ ParticipantsJoin}",

                                        Receiver = db.AspNetUsers.FirstOrDefault(x => x.Id == p.ParticipantId).PhoneNumber

                                    });

                                    var task = db.tbl_Conference.Where(x => x.Id == conference.Id).FirstOrDefault();

                                    //  task.IsSMSSent = true;

                                    db.SaveChanges();
                                }
                                catch (Exception)
                                {

                                    var task = db.tbl_Conference.Where(x => x.Id == conference.Id).FirstOrDefault();

                                    //task.IsSMSSent = false;

                                    db.SaveChanges();
                                }


                            }


                            else if (conference.ConferenceStatusId == 5)
                            {



                                try
                                {
                                    SendSMSUfone(new SMSViewModel
                                    {
                                        Body = $"Dear Sir/Madam, You are informed that following Conference is cancelled" +
                                   $"\nConference Title : {conference.Title}" +
                                   $"\nChaired By : {OrganizerName} " +
                                   $"\nVenue :  {VenueName} " +
                                    $"\nParticipants :{ ParticipantsJoin}",

                                        Receiver = db.AspNetUsers.FirstOrDefault(x => x.Id == p.ParticipantId).PhoneNumber

                                    });

                                    var task = db.tbl_Conference.Where(x => x.Id == conference.Id).FirstOrDefault();

                                    //  task.IsSMSSent = true;

                                    db.SaveChanges();
                                }
                                catch (Exception)
                                {

                                    var task = db.tbl_Conference.Where(x => x.Id == conference.Id).FirstOrDefault();

                                    //task.IsSMSSent = false;

                                    db.SaveChanges();
                                }


                            }


                        }

                        trans.Commit();
                        model.Id = conference.Id;
                        return model;

                    }
                    catch (Exception)
                    {


                        trans.Rollback();
                        throw;
                    }
                }


            }
        }

        public List<ConferenceViewDTO> GetAllConferences(ConferenceFilter filter)
        {

            using (var db = new IDDbContext())
            {
                using (var trans = db.Database.BeginTransaction())
                {
                    try
                    {
                        var qury = db.tbl_ConferenceParticipant.AsQueryable().Where(x => x.Conference.IsDeleted == false);
                        if (filter.RecordStatus == true)
                        {
                            qury = qury.Where(x => x.Conference.RecordStatus == true && x.Conference.IsDeleted == false);
                        }
                        if (filter.RecordStatus == false)
                        {
                            qury = qury.Where(x => x.Conference.RecordStatus == false && x.Conference.IsDeleted == false);
                        }

                        if (filter.ConferenceStatus == "Active")
                        {
                            qury = qury.Where(x => x.Conference.RecordStatus == true && x.Conference.ConferenceStatusId == 1);
                        }
                        if (filter.ConferenceStatus == "InActive")
                        {
                            qury = qury.Where(x => x.Conference.RecordStatus == false && x.Conference.ConferenceStatusId == 2);
                        }
                        if (filter.ConferenceStatus == "Postponed")
                        {
                            qury = qury.Where(x => x.Conference.RecordStatus == true && x.Conference.ConferenceStatusId == 3);
                        }
                        if (filter.ConferenceStatus == "Rescheduled")
                        {
                            qury = qury.Where(x => x.Conference.RecordStatus == true && x.Conference.ConferenceStatusId == '4');
                        }
                        if (filter.ConferenceStatus == "Archived")
                        {
                            qury = qury.Where(x => x.Conference.RecordStatus == true && x.Conference.ConferenceStatusId == '6');
                        }
                        if (filter.ConferenceStatus == "Cancelled")
                        {
                            qury = qury.Where(x => x.Conference.RecordStatus == true && x.Conference.ConferenceStatusId == '5');
                        }
                        if (filter.UserId != null)
                        {
                            qury = qury.Where(x => x.ParticipantId == filter.UserId || x.Conference.CreatedBy == filter.UserId);
                        }

                        var conferenceIds = qury.Select(x => x.ConferenceId).ToList();

                        var conferences = db.tbl_Conference.Include(x => x.tbl_ConferenceParticipant).Include(x => x.ConferenceVenue).Include(x => x.ConferenceOrganizer).Include(x => x.ConferenceStatus).Where(x => conferenceIds.Contains(x.Id)).ToList();
                        var result = new List<ConferenceViewDTO>();
                        foreach (var x in conferences)
                        {
                            var e = new ConferenceViewDTO
                            {
                                Id = x.Id,
                                Title = x.Title,

                                StartDateTime = x.StartDateTime,
                                EndDateTime = x.EndDateTime,

                                Description = x.Description,


                                RecordStatus = x.RecordStatus,

                                IsDeleted = x.IsDeleted,
                                ConferenceVenueId = x.ConferenceVenueId,
                                ConferenceOrganizerId = x.ConferenceOrganizerId,
                                ConferenceStatusId = x.ConferenceStatusId,
                                ConferenceOrganizerIdName = x?.ConferenceOrganizer?.Organizer,
                                ConferenceVenueIdName = x?.ConferenceVenue?.Venue,
                                ConferenceStatusIdName = x?.ConferenceStatus?.Status,


                            };
                            foreach (var p in x.tbl_ConferenceParticipant)
                            {
                                e.ConferenceParticipant
                                    .Add(new ConferenceParticipantView
                                    {
                                        ParticipantName = db.AspNetUsers.FirstOrDefault(x => x.Id == p.ParticipantId).Designation,
                                        ParticipantId = p.ParticipantId,
                                        ConferenceId = p.ConferenceId,
                                        ParticipantFullName = db.AspNetUsers.FirstOrDefault(x => x.Id == p.ParticipantId).FullName,
                                        Id = p.Id
                                    }
                                    );

                            }
                            e.ConferenceParticipantData = string.Join(",", x.tbl_ConferenceParticipant.Select(x => x.Participant.FullName));

                            result.Add(e);

                        }


                        return result;


                    }
                    catch (Exception)
                    {
                        trans.Rollback();
                        throw;
                    }
                }


            }
        }


        public List<tbl_ConferenceParticipantView> GetConferencesList()
        {
            using (var db = new IDDbContext())
            {
                using (var trans = db.Database.BeginTransaction())
                {
                    try
                    {
                        var query = db.tbl_ConferenceParticipantView.AsQueryable();
                        return query.ToList();
                    }
                    catch (Exception)
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }
        }
        #endregion





        #region HelperMethods
        public async Task<string> UploadFile(IFormFile file)
        {
            try
            {
                string filename = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                filename = this.EnsureCorrectFilename($"{Guid.NewGuid().ToString("N")}_{ DateTime.UtcNow.AddHours(5).ToString("ddMMyyyyHHmmssffffff")}");
                filename = filename + ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                string path = _env.WebRootPath + "/Uploads/" + filename;


                using (FileStream output = System.IO.File.Create(path))
                    await file.CopyToAsync(output);



                return filename;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private string EnsureCorrectFilename(string filename)
        {
            if (filename.Contains("\\"))
                filename = filename.Substring(filename.LastIndexOf("\\") + 1);

            return filename;
        }


        #endregion


        public SMSViewModel SendSMSUfone(SMSViewModel sms)
        {
            SMSSendingService service = new SMSSendingService();
            sms.Mask = sms.Mask == null ? "PSHD" : sms.Mask;
            //sms.Mask = sms.Mask == null ? "P%26SHD" : sms.Mask;
            //sms.Sender = "03315519747";
            sms.Sender = "03018482714";
            sms = service.SendQuickMessage(sms);
            return sms;
        }


        public SMSViewModel SendSMS(SMSViewModel sms)
        {
            return SendSMSUfone(sms);
        }


        #region AddVenue
        public int AddVenue(ConferenceVenueDTO model, string userid)
        {

            using (var db = new IDDbContext())
            {
                using (var trans = db.Database.BeginTransaction())
                {
                    try
                    {

                        var newConferenceVenue = this._mapper.Map<tbl_MeetingVenue>(model);

                        newConferenceVenue.RecordStatus = true;
                        newConferenceVenue.CreatedBy = userid;
                        newConferenceVenue.CreationDate = UtilService.GetPkCurrentDateTime();

                        newConferenceVenue.Venue = model.Venue;
                        newConferenceVenue.OderBy = 100;








                        db.tbl_MeetingVenue.Add(newConferenceVenue);

                        db.SaveChanges();











                        trans.Commit();
                        return 1;




                    }
                    catch (Exception)
                    {
                        trans.Rollback();
                        throw;
                    }
                }


            }
        }
        #endregion

        #region AddOrganizer
        public int AddOrganizer(ConferenceOrganizerDTO model, string userid)
        {

            using (var db = new IDDbContext())
            {
                using (var trans = db.Database.BeginTransaction())
                {
                    try
                    {

                        var newConferenceOrganizer = this._mapper.Map<tbl_MeetingOrganizer>(model);

                        newConferenceOrganizer.RecordStatus = true;
                        newConferenceOrganizer.CreatedBy = userid;
                        newConferenceOrganizer.CreationDate = UtilService.GetPkCurrentDateTime();

                        newConferenceOrganizer.Organizer = model.Organizer;
                        newConferenceOrganizer.OderBy = 100;








                        db.tbl_MeetingOrganizer.Add(newConferenceOrganizer);

                        db.SaveChanges();











                        trans.Commit();
                        return 1;




                    }
                    catch (Exception)
                    {
                        trans.Rollback();
                        throw;
                    }
                }


            }
        }
        #endregion
        #region GetConferenceOrganizerList
        public List<tbl_MeetingOrganizer> GetConferenceOrganizerList()
        {
            try
            {
                using (var db = new IDDbContext())
                {

                    return db.tbl_MeetingOrganizer.ToList();

                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion


        #region GetConferenceVenueList
        public List<tbl_MeetingVenue> GetConferenceVenueList()
        {
            try
            {
                using (var db = new IDDbContext())
                {

                    return db.tbl_MeetingVenue.ToList();

                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion


        #region GetConferenceStatus
        public List<DDLConferenceStatusModel> GetConferenceStatus()
        {
            try
            {
                using var db = new IDDbContext();

                return db.tbl_MeetingStatus.Select(x => new DDLConferenceStatusModel { Id = x.Id, Status = x.Status }).ToList();
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion


        #region GetConferenceListsByStatus
        public List<ConferenceViewDTO> GetConferenceListsByStatus(Guid userid, int Status)
        {
            try
            {


                using (var db = new IDDbContext())
                {

                    List<ConferenceViewDTO> res = new List<ConferenceViewDTO>();

                    res = (from t in db.tbl_Conference
                           join c in db.tbl_MeetingStatus on t.ConferenceStatusId equals c.Id
                           where ((t.ConferenceStatusId == Status &&
                            t.RecordStatus == true) || (Status == 0 && t.RecordStatus == true))


                           select new ConferenceViewDTO
                           {
                               Id = t.Id,
                               Title = t.Title,
                               Description = t.Description,

                               StartDateTime = t.StartDateTime,

                               ConferenceStatusId = c.Id,
                               RecordStatus = t.RecordStatus,

                               CreatedBy = t.CreatedBy,
                               UpdatedBy = t.UpdatedBy,

                               IsDeleted = t.IsDeleted,
                               ConferenceVenueId = t.ConferenceVenueId,
                               ConferenceOrganizerId = t.ConferenceOrganizerId,

                               ConferenceOrganizerIdName = t.ConferenceOrganizer.Organizer,
                               ConferenceVenueIdName = t.ConferenceVenue.Venue,
                               ConferenceStatusIdName = t.ConferenceStatus.Status,





                           }).ToList();


                    foreach (var item in res)
                    {
                        var PIDs = db.tbl_ConferenceParticipant.Where(x => x.ConferenceId == item.Id).Select(x => x.ParticipantId).ToList();
                       // var Parts = db.AspNetUsers.Where(x => PIDs.Contains(x.Id)).Select(x => x.Designation).ToList();
                        var Parts = db.AspNetUsers.Where(x => PIDs.Contains(x.Id)).Select(x => x.FullName).ToList();
                        var ParticipantsJoin = String.Join(",", Parts.Select(x => x.ToString()).ToArray());
                        item.ConferenceParticipantData = ParticipantsJoin;
                    }


                    return res;


                }
            }
            catch
            {
                throw;
            }
        }

        #endregion


        #region GetConferenceCount
        public ConferenceCount GetConferenceCount(Guid? userid)
        {
            try
            {
                using (var db = new IDDbContext())
                {

                    var tasks = db.tbl_Conference.Where(x => x.RecordStatus == true).AsQueryable();


                    var result = tasks.ToList();
                    var output = new ConferenceCount
                    {
                        Total = result.Count(),
                        Active = result.Count(x => x.ConferenceStatusId == 1),
                        InActive = result.Count(x => x.ConferenceStatusId == 2),
                        Postponed = result.Count(x => x.ConferenceStatusId == 3),
                        Rescheduled = result.Count(x => x.ConferenceStatusId == 4),
                        Cancelled = result.Count(x => x.ConferenceStatusId == 5),

                        Archived = result.Count(x => x.ConferenceStatusId == 6)


                    };

                    return output;

                }
            }
            catch
            {
                throw;
            }
        }

        #endregion


        #region UpdateConferenceStatus
        public int UpdateConferenceStatus(ConferenceDetailDTO model, string userid)
        {

            using (var db = new IDDbContext())
            {
                using (var trans = db.Database.BeginTransaction())
                {
                    try
                    {


                        if (model.ConferenceId != 0)
                        {
                            var task = db.tbl_Conference.Where(x => x.Id == model.ConferenceId).FirstOrDefault();
                            if (model.ConferenceStatusId != 0)
                            {


                                task.ConferenceStatusId = model.ConferenceStatusId;
                                task.UpdatedBy = Guid.Parse(userid);
                                task.UpdationDate = DateTime.UtcNow.AddHours(5);

                                db.SaveChanges();

                            }


                            trans.Commit();

                            return 1;
                        }
                        else
                        {
                            return 0;
                        }
                    }
                    catch (Exception)
                    {
                        trans.Rollback();
                        throw;
                    }
                }

            }
        }
        #endregion

        public List<tbl_ConferenceParticipantView> GetConferenceParticipants(int ConferenceId)
        {

            using (var db = new IDDbContext())
            {
                using (var trans = db.Database.BeginTransaction())
                {
                    try
                    {
                        if (ConferenceId > 0)
                        {
                            var res = db.tbl_ConferenceParticipantView.Where(x => x.ConferenceId == ConferenceId).ToList();
                            return res;
                        }
                        return null;
                    }
                    catch (Exception)
                    {
                        trans.Rollback();
                        throw;
                    }
                }

            }
        }
        public List<tbl_ConferenceParticipantView> GetConferenceParticipantList(int ConferenceId, string userid)
        {

            using (var db = new IDDbContext())
            {
                using (var trans = db.Database.BeginTransaction())
                {
                    try
                    {
                        if (ConferenceId > 0)
                        {
                            userid = userid.ToUpper();
                            var loggedInUser = db.AspNetUsers.FirstOrDefault(x => x.Id.ToString() == userid);
                            if(loggedInUser != null)
                            {
                                var contact = db.tbl_Contacts.FirstOrDefault(x => x.Id == loggedInUser.ContactId);
                                if(contact != null)
                                {
                                    if (!string.IsNullOrEmpty(contact.Division))
                                    {
                                        var res = db.tbl_ConferenceParticipantView.Where(x => x.ConferenceId == ConferenceId && x.Division.Equals(contact.Division)).ToList();
                                        return res;
                                    }else
                                    {
                                        var res = db.tbl_ConferenceParticipantView.Where(x => x.ConferenceId == ConferenceId).ToList();
                                        return res;
                                    }
                                }
                            }
                        }
                        return null;
                    }
                    catch (Exception)
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }
        }

        public tbl_ConferenceAttendance GetConferenceParticipantAttendance(FingerPrintDTO fp, tbl_UserFP userFp, string userId)
        {

            using (var db = new IDDbContext())
            {
                using (var trans = db.Database.BeginTransaction())
                {
                    try
                    {
                       
                        var conferenceAttendance = new tbl_ConferenceAttendance();

                        conferenceAttendance.ConferenceId = fp.ConferenceId;
                        conferenceAttendance.ParticipantId = fp.ParticipantId;
                        conferenceAttendance.FingerPrintId = userFp.Id;
                        conferenceAttendance.Datetime = DateTime.UtcNow.AddHours(5);
                        conferenceAttendance.CreatedByUserId = userId;

                        db.tbl_ConferenceAttendance.Add(conferenceAttendance);
                        db.SaveChanges();
                        trans.Commit();
                        return conferenceAttendance;
                    }
                    catch (Exception)
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }
        }

        public void DeleteConference(int Id)
        {

            using (var db = new IDDbContext())
            {
                using (var trans = db.Database.BeginTransaction())
                {
                    try
                    {

                        if (Id != 0)
                        {

                            var Conference = db.tbl_Conference.Where(x => x.Id == Id).FirstOrDefault();

                            Conference.RecordStatus = false;
                            Conference.IsDeleted = true;


                            db.SaveChanges();

                            trans.Commit();

                        }

                    }
                    catch (Exception)
                    {
                        trans.Rollback();
                        throw;
                    }
                }

            }
        }


        public void DeleteConferenceParticipant(string ParticipantId, int ConferenceId)
        {

            using (var db = new IDDbContext())
            {
                using (var trans = db.Database.BeginTransaction())
                {
                    try
                    {
                        if (!string.IsNullOrEmpty(ParticipantId) && ConferenceId > 0)
                        {
                            {
                                var PID = db.tbl_ConferenceParticipant.FirstOrDefault(x => x.ParticipantId.ToString() == ParticipantId && x.ConferenceId == ConferenceId);

                                if (PID != null)
                                {

                                    var res = db.tbl_ConferenceParticipant.Remove(PID);

                                    db.SaveChanges();
                                    trans.Commit();
                                }

                            }
                        }
                    }
                    catch (Exception)
                    {
                        trans.Rollback();
                        throw;
                    }
                }

            }
        }
    }
}
