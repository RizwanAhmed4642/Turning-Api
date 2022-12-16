using AutoMapper;
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
    public class EventServices
    {

        #region Fields
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _env;
        private readonly NotificationService _notificationService = new NotificationService();
        #endregion

        #region Constructors
        public EventServices(IMapper mapper, IWebHostEnvironment env)
        {

            _mapper = mapper;
            _env = env;
        }
        #endregion

        #region AddEvent
        public async Task<EventCalenderView> AddEvent(EventCalenderView model, string userid)
        {

            using (var db = new IDDbContext())
            {
                using (var trans = db.Database.BeginTransaction())
                {
                    try
                    {
                        var eventCalender = new EventCalender();
                        eventCalender.Title = model.Title;
                        eventCalender.Cadre = model.Cadre;
                        eventCalender.TrainingType = model.TrainingType;
                        eventCalender.TraingCategore = model.TrainingCategory;
                        eventCalender.Departments = model.Departments;
                        eventCalender.Description = model.Description;
                        eventCalender.StartDateTime = model.StartDateTime;
                        eventCalender.MeetingStatusId = model.MeetingStatusId;
                        eventCalender.CreatedBy = Guid.Parse(userid);
                        eventCalender.IsDeleted = false;
                        eventCalender.CreationDate = UtilService.GetPkCurrentDateTime();
                      await db.EventCalender.AddAsync(eventCalender);
                        db.SaveChanges();



                        model.Id = eventCalender.Id;
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
        #region UpdateAddEvent
        public async Task<EventCalenderView> Update(EventCalenderView model, string userid)
        {

            using (var db = new IDDbContext())
            {
                using (var trans = db.Database.BeginTransaction())
                {
                    try
                    {
                        var eventCalender = db.EventCalender.Include(x => x.EventParticipant).FirstOrDefault(x => x.Id == model.Id);
                        db.EventParticipant.RemoveRange(eventCalender.EventParticipant);
                        db.SaveChanges();

                        eventCalender.Title = model.Title;
                        eventCalender.StartDateTime = model.StartDateTime;
                        //eventCalender.EndDateTime = model.EndDateTime;
                        eventCalender.Description = model.Description;
                        eventCalender.UpdatedBy = Guid.Parse(userid);

                        eventCalender.MeetingVenueId = model.MeetingVenueId;

                        eventCalender.MeetingOrganizerId = model.MeetingOrganizerId;
                        eventCalender.ExternalParticipants = model.ExternalParticipant;
                        eventCalender.ExternalParticipantsMobileNo = model.ExternalParticipantsMobileNo;
                        eventCalender.MeetingAttendVia = model.MeetingAttendVia;
                        //eventCalender.RecordStatus = model.RecordStatus;
                        //eventCalender.MeetingStatus = model.MeetingStatus;
                        eventCalender.MeetingStatusId = model.MeetingStatusId;
                        if (eventCalender.MeetingStatusId == '1')
                        {
                            eventCalender.RecordStatus = true;


                        }
                        else if (eventCalender.MeetingStatusId == '2')
                        {
                            eventCalender.RecordStatus = false;
                        }
                        else
                        {
                            eventCalender.RecordStatus = true;

                        }
                        eventCalender.IsDeleted = false;
                        eventCalender.UpdationDate = UtilService.GetPkCurrentDateTime();
                        //if (model.AttachmentFile != null)
                        //{
                        //    eventCalender.Attachment = await UploadFile(model.AttachmentFile);
                        //}






                        db.SaveChanges();

                        var OrganizerId = eventCalender.MeetingOrganizerId;
                        var OrganizerName = db.tbl_MeetingOrganizer.Where(x => x.Id == OrganizerId)?.FirstOrDefault()?.Organizer;
                        var VenueId = eventCalender.MeetingVenueId;
                        var VenueName = db.tbl_MeetingVenue.Where(x => x.Id == VenueId)?.FirstOrDefault()?.Venue;

                        if (model.MeetingAttachments != null)
                        {
                            foreach (var item in model.MeetingAttachments)
                            {
                                var newAttachment = new MeetingAttachment();
                                newAttachment.AttachmentName = await UploadFile(item);
                                newAttachment.MeetingId = eventCalender.Id;
                                newAttachment.SourceName = "Meeting";
                                newAttachment.RecordStatus = true;

                                db.MeetingAttachment.Add(newAttachment);
                                db.SaveChanges();
                            }
                        }


                        var PIDs = model.EventParticipant.Select(x => x.ParticipantId).ToList();
                        //var Parts = db.AspNetUsers.Where(x => PIDs.Contains(x.Id)).Select(x => x.Designation).ToList();
                        var Parts = db.AspNetUsers.Where(x => PIDs.Contains(x.Id)).Select(x => x.FullName).ToList();
                        var ParticipantsJoin = String.Join(",", Parts.Select(x => x.ToString()).ToArray());
                        foreach (var p in model.EventParticipant)
                        {
                            EventParticipant meetingAssignee = new EventParticipant();
                            meetingAssignee.EventId = eventCalender.Id;
                            meetingAssignee.ParticipantId = p.ParticipantId;
                            db.EventParticipant.Add(meetingAssignee);
                            db.SaveChanges();
                            //-------------conferenceAssignee User Role------------
                            var userch = db.AspNetUsers.FirstOrDefault(a => a.Id == p.ParticipantId);
                            if (userch != null)
                            {
                                var Role = db.AspNetRoles.FirstOrDefault(a => a.Name.ToUpper() == "MEETINGMANAGEMENTVIEW");
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
                                UserIdFrom = Guid.Parse(eventCalender.CreatedBy.ToString()),
                                UserIdTo = (Guid)p.ParticipantId,
                                Link = "/taskDetail",
                                SourceId = eventCalender.Id.ToString(),
                                SourceType = "Meeting",
                                RecordStatus = true

                            });

                            if (eventCalender.MeetingStatusId == 1)
                            {



                                try
                                {
                                    SendSMSUfone(new SMSViewModel
                                    {
                                        Body = $"Dear Sir/Madam, You are requested to attend the following meeting {eventCalender.MeetingAttendVia}" +
                                   $"\nMeeting Title : {eventCalender.Title}" +
                                   $"\nChaired By : {OrganizerName} " +
                                   $"\nDate Time : {eventCalender.StartDateTime}" +
                                   $"\nVenue :  {VenueName} " +
                                    $"\nParticipants :{ParticipantsJoin},{eventCalender.ExternalParticipants}" +


                                   $"\n\rYou can view this meeting on your calendar by visiting" +
                              " https://mms.pshealthpunjab.gov.pk/mainDashboard",

                                        Receiver = db.AspNetUsers.FirstOrDefault(x => x.Id == p.ParticipantId).PhoneNumber

                                    });

                                    var task = db.EventCalender.Where(x => x.Id == eventCalender.Id).FirstOrDefault();

                                    //  task.IsSMSSent = true;

                                    db.SaveChanges();
                                }
                                catch (Exception)
                                {

                                    var task = db.EventCalender.Where(x => x.Id == eventCalender.Id).FirstOrDefault();

                                    //task.IsSMSSent = false;

                                    db.SaveChanges();
                                }


                            }

                            else if (eventCalender.MeetingStatusId == 3)
                            {



                                try
                                {
                                    SendSMSUfone(new SMSViewModel
                                    {
                                        Body = $"Dear Sir/Madam, You are informed that following meeting is postponed" +
                                   $"\nMeeting Title : {eventCalender.Title}" +
                                   $"\nChaired By : {OrganizerName} " +
                                   $"\nDate Time : {eventCalender.StartDateTime}" +
                                   $"\nVenue :  {VenueName} " +
                                    $"\nParticipants :{ParticipantsJoin},{eventCalender.ExternalParticipants}",

                                        Receiver = db.AspNetUsers.FirstOrDefault(x => x.Id == p.ParticipantId).PhoneNumber

                                    });

                                    var task = db.EventCalender.Where(x => x.Id == eventCalender.Id).FirstOrDefault();

                                    //  task.IsSMSSent = true;

                                    db.SaveChanges();
                                }
                                catch (Exception)
                                {

                                    var task = db.EventCalender.Where(x => x.Id == eventCalender.Id).FirstOrDefault();

                                    //task.IsSMSSent = false;

                                    db.SaveChanges();
                                }


                            }


                            else if (eventCalender.MeetingStatusId == 4)
                            {



                                try
                                {
                                    SendSMSUfone(new SMSViewModel
                                    {
                                        Body = $"Dear Sir/Madam, You are informed that following meeting is reschduled" +
                                   $"\nMeeting Title : {eventCalender.Title}" +
                                   $"\nChaired By : {OrganizerName} " +
                                   $"\nDate Time : {eventCalender.StartDateTime}" +
                                   $"\nVenue :  {VenueName} " +
                                    $"\nParticipants :{ParticipantsJoin},{eventCalender.ExternalParticipants}",

                                        Receiver = db.AspNetUsers.FirstOrDefault(x => x.Id == p.ParticipantId).PhoneNumber

                                    });

                                    var task = db.EventCalender.Where(x => x.Id == eventCalender.Id).FirstOrDefault();

                                    //  task.IsSMSSent = true;

                                    db.SaveChanges();
                                }
                                catch (Exception)
                                {

                                    var task = db.EventCalender.Where(x => x.Id == eventCalender.Id).FirstOrDefault();

                                    //task.IsSMSSent = false;

                                    db.SaveChanges();
                                }


                            }


                            else if (eventCalender.MeetingStatusId == 5)
                            {



                                try
                                {
                                    SendSMSUfone(new SMSViewModel
                                    {
                                        Body = $"Dear Sir/Madam, You are informed that following meeting is cancelled" +
                                   $"\nMeeting Title : {eventCalender.Title}" +
                                   $"\nChaired By : {OrganizerName} " +
                                   $"\nVenue :  {VenueName} " +
                                    $"\nParticipants :{ParticipantsJoin},{eventCalender.ExternalParticipants}",

                                        Receiver = db.AspNetUsers.FirstOrDefault(x => x.Id == p.ParticipantId).PhoneNumber

                                    });

                                    var task = db.EventCalender.Where(x => x.Id == eventCalender.Id).FirstOrDefault();

                                    //  task.IsSMSSent = true;

                                    db.SaveChanges();
                                }
                                catch (Exception)
                                {

                                    var task = db.EventCalender.Where(x => x.Id == eventCalender.Id).FirstOrDefault();

                                    //task.IsSMSSent = false;

                                    db.SaveChanges();
                                }


                            }


                        }
                        if (eventCalender.ExternalParticipantsMobileNo != null)
                        {
                            string[] exeternalParticipantSMS = eventCalender.ExternalParticipantsMobileNo.Split(',');

                            foreach (var forwoexeternalParticipantSMS in exeternalParticipantSMS)
                            {




                                if (eventCalender.MeetingStatusId == 1)
                                {
                                    try
                                    {
                                        SendSMSUfone(new SMSViewModel
                                        {
                                            Body = $"Dear Sir/Madam, You are requested to attend the following meeting {eventCalender.MeetingAttendVia}" +
                                       $"\nMeeting Title : {eventCalender.Title}" +
                                       $"\nChaired By : {OrganizerName} " +
                                       $"\nDate Time : {eventCalender.StartDateTime}" +
                                       $"\nVenue :  {VenueName} " +
                                        $"\nParticipants :{ParticipantsJoin},{eventCalender.ExternalParticipants}" +


                                       $"\n\rYou can view this meeting on your calendar by visiting" +
                                  " https://mms.pshealthpunjab.gov.pk/mainDashboard",

                                            Receiver = forwoexeternalParticipantSMS,

                                        });


                                    }
                                    catch (Exception)
                                    {


                                    }

                                }

                                else if (eventCalender.MeetingStatusId == 3)
                                {
                                    try
                                    {
                                        SendSMSUfone(new SMSViewModel
                                        {
                                            Body = $"Dear Sir/Madam, You are informed that following meeting is postponed" +
                                       $"\nMeeting Title : {eventCalender.Title}" +
                                       $"\nChaired By : {OrganizerName} " +
                                       $"\nDate Time : {eventCalender.StartDateTime}" +
                                       $"\nVenue :  {VenueName} " +
                                        $"\nParticipants :{ParticipantsJoin},{eventCalender.ExternalParticipants}" +


                                       $"\n\rYou can view this meeting on your calendar by visiting" +
                                  " https://mms.pshealthpunjab.gov.pk/mainDashboard",

                                            Receiver = forwoexeternalParticipantSMS,

                                        });


                                    }
                                    catch (Exception)
                                    {


                                    }

                                }

                                else if (eventCalender.MeetingStatusId == 4)
                                {
                                    try
                                    {
                                        SendSMSUfone(new SMSViewModel
                                        {
                                            Body = $"Dear Sir/Madam, You are informed that following meeting is reschduld " +
                                       $"\nMeeting Title : {eventCalender.Title}" +
                                       $"\nChaired By : {OrganizerName} " +
                                       $"\nDate Time : {eventCalender.StartDateTime}" +
                                       $"\nVenue :  {VenueName} " +
                                        $"\nParticipants :{ParticipantsJoin},{eventCalender.ExternalParticipants}" +


                                       $"\n\rYou can view this meeting on your calendar by visiting" +
                                  " https://mms.pshealthpunjab.gov.pk/mainDashboard",

                                            Receiver = forwoexeternalParticipantSMS,

                                        });


                                    }
                                    catch (Exception)
                                    {


                                    }

                                }
                                else if (eventCalender.MeetingStatusId == 5)
                                {
                                    try
                                    {
                                        SendSMSUfone(new SMSViewModel
                                        {
                                            Body = $"Dear Sir/Madam, You are informed that following meeting is cancelled " +
                                       $"\nMeeting Title : {eventCalender.Title}" +
                                       $"\nChaired By : {OrganizerName} " +
                                       $"\nDate Time : {eventCalender.StartDateTime}" +
                                       $"\nVenue :  {VenueName} " +
                                        $"\nParticipants :{ParticipantsJoin},{eventCalender.ExternalParticipants}" +


                                       $"\n\rYou can view this meeting on your calendar by visiting" +
                                  " https://mms.pshealthpunjab.gov.pk/mainDashboard",

                                            Receiver = forwoexeternalParticipantSMS,

                                        });


                                    }
                                    catch (Exception)
                                    {


                                    }

                                }
                            }
                        }

                        trans.Commit();
                        model.Id = eventCalender.Id;
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

        public List<EventCalender> GetEvents(EventFilter filter)
        {

            using (var db = new IDDbContext())
            {
                using (var trans = db.Database.BeginTransaction())
                {
                    try
                    {

                        var result = db.EventCalender.ToList();
                   


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
        public List<EventCalender> GetTrainings()
        {

            using (var db = new IDDbContext())
            {
                using (var trans = db.Database.BeginTransaction())
                {
                    try
                    {

                        var result = db.EventCalender.ToList();
                   


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

        #endregion





        #region HelperMethods
        public async Task<string> UploadFile(IFormFile file)
        {
            try
            {
                string filename = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                filename = this.EnsureCorrectFilename($"{Guid.NewGuid().ToString("N")}_{DateTime.UtcNow.AddHours(5).ToString("ddMMyyyyHHmmssffffff")}");
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
        //public SMSViewModel SendSMSTelenor(SMSViewModel sms)
        //{
        //    string userName = "923464950571";
        //    string password = "Hisdu%40012_p%26shd-Hisdu%2Fp%26shd";

        //    SMSSendingService service = new SMSSendingService(userName, password);
        //    sms.Mask = sms.Mask == null ? "PSHD" : sms.Mask;
        //    sms.Sender = userName;
        //    sms = service.SendQuickMessage(sms);

        //    return sms;
        //}

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


        #region Venue
        public int AddVenue(MeetingVenueDTO model, string userid)
        {

            using (var db = new IDDbContext())
            {
                using (var trans = db.Database.BeginTransaction())
                {
                    try
                    {

                        var newMeetingVenue = this._mapper.Map<tbl_MeetingVenue>(model);

                        newMeetingVenue.RecordStatus = true;
                        newMeetingVenue.CreatedBy = userid;
                        newMeetingVenue.CreationDate = UtilService.GetPkCurrentDateTime();

                        newMeetingVenue.Venue = model.Venue;
                        newMeetingVenue.OderBy = 100;








                        db.tbl_MeetingVenue.Add(newMeetingVenue);

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

        public List<tbl_MeetingVenue> GetVenue()
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

        #region TraingCategory
        public int AddTraingCategory(TrainingCategoryDTO model, string userid)
        {

            using (var db = new IDDbContext())
            {
                using (var trans = db.Database.BeginTransaction())
                {
                    try
                    {

                        var newTraingCategory = this._mapper.Map<tbl_TrainingCategory>(model);

                        newTraingCategory.RecordStatus = true;
                        newTraingCategory.CreatedBy = userid;
                        newTraingCategory.CreationDate = UtilService.GetPkCurrentDateTime();

                        newTraingCategory.TraingCategoryName = model.TraingCategoryName;
                        newTraingCategory.OderBy = 100;


                        db.tbl_TrainingCategory.Add(newTraingCategory);

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

        #region TraningType
        public int AddTrainingType(TrainingTypeDTO model, string userid)
        {

            using (var db = new IDDbContext())
            {
                using (var trans = db.Database.BeginTransaction())
                {
                    try
                    {

                        tbl_TraingType _entity = this._mapper.Map<tbl_TraingType>(model);

                        _entity.RecordStatus = true;
                        _entity.CreatedBy = userid;
                        _entity.CreationDate = UtilService.GetPkCurrentDateTime();


                        _entity.OderBy = 100;


                        db.tbl_TraingType.Add(_entity);

                        db.SaveChanges();


                        trans.Commit();
                        return 1;




                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        var message = ex.Message;
                        throw;
                    }
                }


            }
        }


        public List<tbl_TrainingCategory> GetTrainingCategory()
        {
            try
            {
                using (var db = new IDDbContext())
                {

                    return db.tbl_TrainingCategory.ToList();

                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<tbl_TraingType> GetTrainingType()
        {
            try
            {
                using (var db = new IDDbContext())
                {

                    return db.tbl_TraingType.ToList();

                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        #endregion

        #region AddOrganizer
        public int AddOrganizer(MeetingOrganizerDTO model, string userid)
        {

            using (var db = new IDDbContext())
            {
                using (var trans = db.Database.BeginTransaction())
                {
                    try
                    {

                        var newMeetingOrganizer = this._mapper.Map<tbl_MeetingOrganizer>(model);

                        newMeetingOrganizer.RecordStatus = true;
                        newMeetingOrganizer.CreatedBy = userid;
                        newMeetingOrganizer.CreationDate = UtilService.GetPkCurrentDateTime();

                        newMeetingOrganizer.Organizer = model.Organizer;
                        newMeetingOrganizer.OderBy = 100;








                        db.tbl_MeetingOrganizer.Add(newMeetingOrganizer);

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
        #region GetMeetingOrganizerList
        public List<tbl_MeetingOrganizer> GetMeetingOrganizerList()
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


        #region GetMeetingVenueList
        public List<tbl_MeetingVenue> GetMeetingVenueList()
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


        #region GetMeetingStatus
        public List<DDLMeetingStatusModel> GetMeetingStatus()
        {
            try
            {
                using var db = new IDDbContext();

                return db.tbl_MeetingStatus.Select(x => new DDLMeetingStatusModel { Id = x.Id, Status = x.Status }).ToList();
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion


        #region GetMeetingListsByStatus
        public List<EventCalenderView> GetMeetingListsByStatus(Guid? userid, int Status)
        {
            try
            {


                using (var db = new IDDbContext())
                {
                    List<EventCalenderView> res = new List<EventCalenderView>();
                    if (userid != null)
                    {



                        if (Status == 1 || Status == 3 || Status == 4 || Status == 5)
                        {




                            res = (from t in db.EventCalender
                                       //join mp in db.EventParticipant on t.Id equals mp.EventId
                                   join c in db.tbl_MeetingStatus on t.MeetingStatusId equals c.Id
                                   //where ((t.CreatedBy == userid || mp.ParticipantId == userid) && ((t.MeetingStatusId == Status &&
                                   where ((t.CreatedBy == userid || t.EventParticipant.Any(x => x.ParticipantId == userid)) && ((t.MeetingStatusId == Status &&
                                    t.RecordStatus == true && t.IsDeleted == false)))


                                   select new EventCalenderView
                                   {
                                       Id = t.Id,
                                       Title = t.Title,
                                       Description = t.Description,

                                       StartDateTime = t.StartDateTime,

                                       MeetingStatusId = c.Id,
                                       RecordStatus = t.RecordStatus,
                                       ExternalParticipant = t.ExternalParticipants,
                                       ExternalParticipantsMobileNo = t.ExternalParticipantsMobileNo,
                                       //eventCalender.CreatedBy = Guid.Parse(userid);
                                       CreatedBy = t.CreatedBy,
                                       UpdatedBy = t.UpdatedBy,

                                       IsDeleted = t.IsDeleted,
                                       MeetingVenueId = t.MeetingVenueId,
                                       MeetingOrganizerId = t.MeetingOrganizerId,

                                       MeetingOrganizerIdName = t.MeetingOrganizer.Organizer,
                                       MeetingVenueIdName = t.MeetingVenue.Venue,
                                       MeetingStatusIdName = t.MeetingStatus.Status,
                                       MeetingAttendVia = t.MeetingAttendVia,




                                   }).ToList();


                            foreach (var item in res)
                            {
                                var PIDs = db.EventParticipant.Where(x => x.EventId == item.Id).Select(x => x.ParticipantId).ToList();
                                //var Parts = db.AspNetUsers.Where(x => PIDs.Contains(x.Id)).Select(x => x.Designation).ToList();
                                var Parts = db.AspNetUsers.Where(x => PIDs.Contains(x.Id)).Select(x => x.FullName).ToList();
                                var ParticipantsJoin = String.Join(",", Parts.Select(x => x.ToString()).ToArray());
                                item.EventParticipantData = ParticipantsJoin;


                            }


                            var tot = res.Count();

                            return res;
                        }
                        else if (Status == 6 || Status == 2)
                        {
                            res = (from t in db.EventCalender
                                       //join mp in db.EventParticipant on t.Id equals mp.EventId
                                   join c in db.tbl_MeetingStatus on t.MeetingStatusId equals c.Id
                                   where ((t.CreatedBy == userid || t.EventParticipant.Any(x => x.ParticipantId == userid)) && (t.MeetingStatusId == Status &&
                                    t.RecordStatus == false && t.IsDeleted == false))


                                   select new EventCalenderView
                                   {
                                       Id = t.Id,
                                       Title = t.Title,
                                       Description = t.Description,

                                       StartDateTime = t.StartDateTime,

                                       MeetingStatusId = c.Id,
                                       RecordStatus = t.RecordStatus,
                                       ExternalParticipant = t.ExternalParticipants,
                                       ExternalParticipantsMobileNo = t.ExternalParticipantsMobileNo,
                                       //eventCalender.CreatedBy = Guid.Parse(userid);
                                       CreatedBy = t.CreatedBy,
                                       UpdatedBy = t.UpdatedBy,

                                       IsDeleted = t.IsDeleted,
                                       MeetingVenueId = t.MeetingVenueId,
                                       MeetingOrganizerId = t.MeetingOrganizerId,

                                       MeetingOrganizerIdName = t.MeetingOrganizer.Organizer,
                                       MeetingVenueIdName = t.MeetingVenue.Venue,
                                       MeetingStatusIdName = t.MeetingStatus.Status,
                                       MeetingAttendVia = t.MeetingAttendVia,




                                   }).ToList();


                            foreach (var item in res)
                            {
                                var PIDs = db.EventParticipant.Where(x => x.EventId == item.Id).Select(x => x.ParticipantId).ToList();
                                //var Parts = db.AspNetUsers.Where(x => PIDs.Contains(x.Id)).Select(x => x.Designation).ToList();
                                var Parts = db.AspNetUsers.Where(x => PIDs.Contains(x.Id)).Select(x => x.FullName).ToList();
                                var ParticipantsJoin = String.Join(",", Parts.Select(x => x.ToString()).ToArray());
                                item.EventParticipantData = ParticipantsJoin;
                            }


                            var tot = res.Count();

                            return res;

                        }
                        else if (Status == 0)
                        {
                            res = (from t in db.EventCalender
                                       //join mp in db.EventParticipant on t.Id equals mp.EventId
                                   join c in db.tbl_MeetingStatus on t.MeetingStatusId equals c.Id
                                   where ((t.CreatedBy == userid || t.EventParticipant.Any(x => x.ParticipantId == userid)) && (Status == 0 && t.IsDeleted == false))


                                   select new EventCalenderView
                                   {
                                       Id = t.Id,
                                       Title = t.Title,
                                       Description = t.Description,

                                       StartDateTime = t.StartDateTime,

                                       MeetingStatusId = c.Id,
                                       RecordStatus = t.RecordStatus,
                                       ExternalParticipant = t.ExternalParticipants,
                                       ExternalParticipantsMobileNo = t.ExternalParticipantsMobileNo,
                                       //eventCalender.CreatedBy = Guid.Parse(userid);
                                       CreatedBy = t.CreatedBy,
                                       UpdatedBy = t.UpdatedBy,

                                       IsDeleted = t.IsDeleted,
                                       MeetingVenueId = t.MeetingVenueId,
                                       MeetingOrganizerId = t.MeetingOrganizerId,

                                       MeetingOrganizerIdName = t.MeetingOrganizer.Organizer,
                                       MeetingVenueIdName = t.MeetingVenue.Venue,
                                       MeetingStatusIdName = t.MeetingStatus.Status,
                                       MeetingAttendVia = t.MeetingAttendVia,




                                   }).ToList();


                            foreach (var item in res)
                            {
                                var PIDs = db.EventParticipant.Where(x => x.EventId == item.Id).Select(x => x.ParticipantId).ToList();
                                //var Parts = db.AspNetUsers.Where(x => PIDs.Contains(x.Id)).Select(x => x.Designation).ToList();
                                var Parts = db.AspNetUsers.Where(x => PIDs.Contains(x.Id)).Select(x => x.FullName).ToList();
                                var ParticipantsJoin = String.Join(",", Parts.Select(x => x.ToString()).ToArray());
                                item.EventParticipantData = ParticipantsJoin;
                            }


                            var tot = res.Count();

                            return res;


                        }



                    }

                    else
                    {




                        if (Status == 1 || Status == 3 || Status == 4 || Status == 5)
                        {




                            res = (from t in db.EventCalender
                                       //join mp in db.EventParticipant on t.Id equals mp.EventId
                                   join c in db.tbl_MeetingStatus on t.MeetingStatusId equals c.Id
                                   //where ((t.CreatedBy == userid || mp.ParticipantId == userid) && ((t.MeetingStatusId == Status &&
                                   where ((t.EventParticipant.Count > 0) && ((t.MeetingStatusId == Status &&
                                    t.RecordStatus == true && t.IsDeleted == false)))


                                   select new EventCalenderView
                                   {
                                       Id = t.Id,
                                       Title = t.Title,
                                       Description = t.Description,

                                       StartDateTime = t.StartDateTime,

                                       MeetingStatusId = c.Id,
                                       RecordStatus = t.RecordStatus,
                                       ExternalParticipant = t.ExternalParticipants,
                                       ExternalParticipantsMobileNo = t.ExternalParticipantsMobileNo,
                                       //eventCalender.CreatedBy = Guid.Parse(userid);
                                       CreatedBy = t.CreatedBy,
                                       UpdatedBy = t.UpdatedBy,

                                       IsDeleted = t.IsDeleted,
                                       MeetingVenueId = t.MeetingVenueId,
                                       MeetingOrganizerId = t.MeetingOrganizerId,

                                       MeetingOrganizerIdName = t.MeetingOrganizer.Organizer,
                                       MeetingVenueIdName = t.MeetingVenue.Venue,
                                       MeetingStatusIdName = t.MeetingStatus.Status,
                                       MeetingAttendVia = t.MeetingAttendVia,




                                   }).ToList();


                            foreach (var item in res)
                            {
                                var PIDs = db.EventParticipant.Where(x => x.EventId == item.Id).Select(x => x.ParticipantId).ToList();
                                //var Parts = db.AspNetUsers.Where(x => PIDs.Contains(x.Id)).Select(x => x.Designation).ToList();
                                var Parts = db.AspNetUsers.Where(x => PIDs.Contains(x.Id)).Select(x => x.FullName).ToList();
                                var ParticipantsJoin = String.Join(",", Parts.Select(x => x.ToString()).ToArray());
                                item.EventParticipantData = ParticipantsJoin;


                            }


                            var tot = res.Count();

                            return res;
                        }
                        else if (Status == 6 || Status == 2)
                        {
                            res = (from t in db.EventCalender
                                       //join mp in db.EventParticipant on t.Id equals mp.EventId
                                   join c in db.tbl_MeetingStatus on t.MeetingStatusId equals c.Id
                                   where ((t.EventParticipant.Count > 0) && (t.MeetingStatusId == Status &&
                                    t.RecordStatus == false && t.IsDeleted == false))


                                   select new EventCalenderView
                                   {
                                       Id = t.Id,
                                       Title = t.Title,
                                       Description = t.Description,

                                       StartDateTime = t.StartDateTime,

                                       MeetingStatusId = c.Id,
                                       RecordStatus = t.RecordStatus,
                                       ExternalParticipant = t.ExternalParticipants,
                                       ExternalParticipantsMobileNo = t.ExternalParticipantsMobileNo,
                                       //eventCalender.CreatedBy = Guid.Parse(userid);
                                       CreatedBy = t.CreatedBy,
                                       UpdatedBy = t.UpdatedBy,

                                       IsDeleted = t.IsDeleted,
                                       MeetingVenueId = t.MeetingVenueId,
                                       MeetingOrganizerId = t.MeetingOrganizerId,

                                       MeetingOrganizerIdName = t.MeetingOrganizer.Organizer,
                                       MeetingVenueIdName = t.MeetingVenue.Venue,
                                       MeetingStatusIdName = t.MeetingStatus.Status,
                                       MeetingAttendVia = t.MeetingAttendVia,




                                   }).ToList();


                            foreach (var item in res)
                            {
                                var PIDs = db.EventParticipant.Where(x => x.EventId == item.Id).Select(x => x.ParticipantId).ToList();
                                //var Parts = db.AspNetUsers.Where(x => PIDs.Contains(x.Id)).Select(x => x.Designation).ToList();
                                var Parts = db.AspNetUsers.Where(x => PIDs.Contains(x.Id)).Select(x => x.FullName).ToList();
                                var ParticipantsJoin = String.Join(",", Parts.Select(x => x.ToString()).ToArray());
                                item.EventParticipantData = ParticipantsJoin;
                            }


                            var tot = res.Count();

                            return res;

                        }
                        else if (Status == 0)
                        {
                            res = (from t in db.EventCalender
                                       //join mp in db.EventParticipant on t.Id equals mp.EventId
                                   join c in db.tbl_MeetingStatus on t.MeetingStatusId equals c.Id
                                   where ((t.EventParticipant.Count > 0) && (Status == 0 && t.IsDeleted == false))


                                   select new EventCalenderView
                                   {
                                       Id = t.Id,
                                       Title = t.Title,
                                       Description = t.Description,

                                       StartDateTime = t.StartDateTime,

                                       MeetingStatusId = c.Id,
                                       RecordStatus = t.RecordStatus,
                                       ExternalParticipant = t.ExternalParticipants,
                                       ExternalParticipantsMobileNo = t.ExternalParticipantsMobileNo,
                                       //eventCalender.CreatedBy = Guid.Parse(userid);
                                       CreatedBy = t.CreatedBy,
                                       UpdatedBy = t.UpdatedBy,

                                       IsDeleted = t.IsDeleted,
                                       MeetingVenueId = t.MeetingVenueId,
                                       MeetingOrganizerId = t.MeetingOrganizerId,

                                       MeetingOrganizerIdName = t.MeetingOrganizer.Organizer,
                                       MeetingVenueIdName = t.MeetingVenue.Venue,
                                       MeetingStatusIdName = t.MeetingStatus.Status,
                                       MeetingAttendVia = t.MeetingAttendVia,




                                   }).ToList();


                            foreach (var item in res)
                            {
                                var PIDs = db.EventParticipant.Where(x => x.EventId == item.Id).Select(x => x.ParticipantId).ToList();
                                //var Parts = db.AspNetUsers.Where(x => PIDs.Contains(x.Id)).Select(x => x.Designation).ToList();
                                var Parts = db.AspNetUsers.Where(x => PIDs.Contains(x.Id)).Select(x => x.FullName).ToList();
                                var ParticipantsJoin = String.Join(",", Parts.Select(x => x.ToString()).ToArray());
                                item.EventParticipantData = ParticipantsJoin;
                            }


                            var tot = res.Count();

                            return res;


                        }


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


        #region GetMeetingCount
        public MeetingCount GetMeetingCount(Guid? userid)
        {
            try
            {
                using (var db = new IDDbContext())
                {
                    var tasks = db.EventCalender.Where(x => x.IsDeleted == false).AsQueryable();
                    var qury = db.EventParticipant.AsQueryable().Where(x => x.Event.IsDeleted == false);
                    if (userid != null)
                    {
                        tasks = tasks.Where(x => x.EventParticipant.Any(x => x.ParticipantId == userid) || x.CreatedBy == userid);


                    }


                    // old way var tasks = db.EventCalender.Where(x => x.RecordStatus == true).AsQueryable();
                    //List<EventCalenderView> tasks = new List<EventCalenderView>();

                    //if(userid  !=null) { 

                    //tasks = (from t in db.EventCalender
                    //         join mp in db.EventParticipant on t.Id equals mp.EventId
                    //         join c in db.tbl_MeetingStatus on t.MeetingStatusId equals c.Id
                    //         where ((t.CreatedBy == userid || mp.ParticipantId == userid) && ((
                    //          t.RecordStatus == true && t.IsDeleted == false)))
                    //             select new EventCalenderView
                    //             {
                    //                 Id = t.Id,
                    //                 Title = t.Title,
                    //                 Description = t.Description,

                    //                 StartDateTime = t.StartDateTime,

                    //                 MeetingStatusId = c.Id,
                    //                 RecordStatus = t.RecordStatus,
                    //                 ExternalParticipant = t.ExternalParticipants,
                    //                 ExternalParticipantsMobileNo = t.ExternalParticipantsMobileNo,
                    //                 //eventCalender.CreatedBy = Guid.Parse(userid);
                    //                 CreatedBy = t.CreatedBy,
                    //                 UpdatedBy = t.UpdatedBy,

                    //                 IsDeleted = t.IsDeleted,
                    //                 MeetingVenueId = t.MeetingVenueId,
                    //                 MeetingOrganizerId = t.MeetingOrganizerId,

                    //                 MeetingOrganizerIdName = t.MeetingOrganizer.Organizer,
                    //                 MeetingVenueIdName = t.MeetingVenue.Venue,
                    //                 MeetingStatusIdName = t.MeetingStatus.Status,
                    //                 MeetingAttendVia = t.MeetingAttendVia,




                    //             }).ToList();


                    //}
                    //else
                    //{
                    //    tasks = (from t in db.EventCalender


                    //             where (
                    //              t.RecordStatus == true && t.IsDeleted == false)
                    //             select new EventCalenderView
                    //             {
                    //                 Id = t.Id,
                    //                 Title = t.Title,
                    //                 Description = t.Description,

                    //                 StartDateTime = t.StartDateTime,

                    //                 MeetingStatusId = c.Id,
                    //                 RecordStatus = t.RecordStatus,
                    //                 ExternalParticipant = t.ExternalParticipants,
                    //                 ExternalParticipantsMobileNo = t.ExternalParticipantsMobileNo,
                    //                 //eventCalender.CreatedBy = Guid.Parse(userid);
                    //                 CreatedBy = t.CreatedBy,
                    //                 UpdatedBy = t.UpdatedBy,

                    //                 IsDeleted = t.IsDeleted,
                    //                 MeetingVenueId = t.MeetingVenueId,
                    //                 MeetingOrganizerId = t.MeetingOrganizerId,

                    //                 MeetingOrganizerIdName = t.MeetingOrganizer.Organizer,
                    //                 MeetingVenueIdName = t.MeetingVenue.Venue,
                    //                 MeetingStatusIdName = t.MeetingStatus.Status,
                    //                 MeetingAttendVia = t.MeetingAttendVia,




                    //             }).ToList();

                    //}
                    //if (userid != null)
                    //{
                    //    tasks = tasks.Where(x =>x.CreatedBy == userid.);


                    //}

                    var result = tasks.Where(x => x.EventParticipant.Count > 0).ToList();
                    var output = new MeetingCount
                    {
                        Total = result.Count(),
                        Active = result.Count(x => x.MeetingStatusId == 1 && x.RecordStatus == true),
                        InActive = result.Count(x => x.MeetingStatusId == 2 && x.RecordStatus == false),
                        Postponed = result.Count(x => x.MeetingStatusId == 3 && x.RecordStatus == true),

                        Rescheduled = result.Count(x => x.MeetingStatusId == 4 && x.RecordStatus == true),
                        Cancelled = result.Count(x => x.MeetingStatusId == 5 && x.RecordStatus == true),

                        Archived = result.Count(x => x.MeetingStatusId == 6 && x.RecordStatus == false)
                        //Archived = result.Count(x => x.StartDateTime < DateTime.Now )

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


        #region UpdateMeetingStatus
        public int UpdateMeetingStatus(MeetingDetailDTO model, string userid)
        {

            using (var db = new IDDbContext())
            {
                using (var trans = db.Database.BeginTransaction())
                {
                    try
                    {


                        if (model.MeetingId != 0)
                        {
                            var task = db.EventCalender.Where(x => x.Id == model.MeetingId).FirstOrDefault();
                            if (model.MeetingStatusId != 0)
                            {


                                task.MeetingStatusId = model.MeetingStatusId;
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


        public void DeleteMeeting(int Id)
        {

            using (var db = new IDDbContext())
            {
                using (var trans = db.Database.BeginTransaction())
                {
                    try
                    {

                        if (Id != 0)
                        {

                            var Meeting = db.EventCalender.Where(x => x.Id == Id).FirstOrDefault();

                            Meeting.RecordStatus = false;
                            Meeting.IsDeleted = true;

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
    }
}
