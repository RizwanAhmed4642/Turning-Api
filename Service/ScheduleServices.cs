using AutoMapper;
using Meeting_App.Data.Database.Context;
using Meeting_App.Data.Database.Tables;
using Meeting_App.Models;
using Meeting_App.Models.DTOs;
using Meeting_App.Service.Common;
using Microsoft.AspNetCore.Authorization;
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
using System.Web.Services.Description;

namespace Meeting_App.Service
{
    public class ScheduleServices
    {

        #region Fields
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _env;
        private readonly NotificationService _notificationService = new NotificationService();
        #endregion

        #region Constructors
        public ScheduleServices(IMapper mapper, IWebHostEnvironment env)
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
        public List<HrDesignation> GetDesignation()
        {

            using (var db = new IDDbContext())
            {
                using (var trans = db.Database.BeginTransaction())
                {
                    try
                    {

                        var result = db.HrDesignation.ToList();



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
