using AutoMapper;
using Meeting_App.Data.Database.Context;
using Meeting_App.Data.Database.Tables;
using Meeting_App.Models;
using Meeting_App.Models.DTOs;
using Meeting_App.Service.Common;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Meeting_App.Service
{
    public class DailyEngagementService
    {
        #region Fields
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _env;
        private readonly NotificationService _notificationService = new NotificationService();
        #endregion

        #region Constructors
        public DailyEngagementService(IMapper mapper, IWebHostEnvironment env)
        {

            _mapper = mapper;
            _env = env;
        }
        #endregion

        #region AddDailyEngagement
        public int AddDailyEngagement(DailyEngagementDTO model, string userid)
        {

            using (var db = new IDDbContext())
            {
                using (var trans = db.Database.BeginTransaction())
                {
                    try
                    {
                        //var userDesignation = db.AspNetUsers.Where(x => x.Id.ToString() == model.Designation).FirstOrDefault().Designation;
                        //var userDesignation = db.AspNetUsers.Where(x => model.ContactAssignees.Any(x=>x.ContactAssigneeId.ToString().Contains(x.Id.ToString()))).Select(x=>x.).ToList();
                        if (model.Id == 0 || model.Id == null)
                        {

                            var newDailyEngagement = this._mapper.Map<tbl_DailyEngagement>(model);

                            // newContact.Attachment = ;
                            newDailyEngagement.RecordStatus = true;
                            newDailyEngagement.IsDeleted = false;
                            newDailyEngagement.CreatedBy = userid;
                            newDailyEngagement.CreationDate = UtilService.GetPkCurrentDateTime();

                            // newContact.Designation =String.Join(",",model.ContactAssignees.Select(x=>x.ContactAssigneeName).ToArray());
                            newDailyEngagement.Task = model.Task;
                            //newDailyEngagement.WhereToGo = model.WhereToGo;
                            newDailyEngagement.WhereToGo = model.VenueId.ToString();
                            newDailyEngagement.WhenToGo = model.WhenToGo;
                            newDailyEngagement.VenueId = model.VenueId;
                            newDailyEngagement.EngagementSatusId = 1;







                            db.tbl_DailyEngagement.Add(newDailyEngagement);

                            db.SaveChanges();











                            trans.Commit();
                            return 1;

                        }
                        else if (model.Id != 0 || model.Id != null)
                        {



                            //var newDailyEngagement = this._mapper.Map<tbl_DailyEngagement>(model);

                            var UpdatedDailyEngagement =  db.tbl_DailyEngagement.FirstOrDefault(x => x.Id == model.Id); 

                            UpdatedDailyEngagement.UpdatedBy = userid;  
                            UpdatedDailyEngagement.UpdationDate = UtilService.GetPkCurrentDateTime(); 
                            UpdatedDailyEngagement.Task= model.Task;    
                            UpdatedDailyEngagement.WhenToGo= model.WhenToGo;    
                            UpdatedDailyEngagement.VenueId= model.VenueId;
                            db.SaveChanges();
                            //// newContact.Attachment = ;
                            //newDailyEngagement.RecordStatus = true;
                            //newDailyEngagement.IsDeleted = false;
                            //newDailyEngagement.UpdatedBy = userid;
                            //newDailyEngagement.UpdationDate = UtilService.GetPkCurrentDateTime();

                            //// newContact.Designation =String.Join(",",model.ContactAssignees.Select(x=>x.ContactAssigneeName).ToArray());
                            //newDailyEngagement.Task = model.Task;
                            ////newDailyEngagement.WhereToGo = model.WhereToGo;
                            //newDailyEngagement.WhereToGo = model.VenueId.ToString();
                            //newDailyEngagement.WhenToGo = model.WhenToGo;
                            //newDailyEngagement.VenueId = model.VenueId;
                            //newDailyEngagement.EngagementSatusId = 1;


                            trans.Commit();
                            return 1;

                        }
                        else
                        {
                            // Map Data of input model
                            var newTask = this._mapper.Map<tbl_DailyEngagement>(model);
                            // newTask.Attachment = await UploadFile(model.Attachment[0]);
                            newTask.UpdatedBy = userid;
                            //newTask.TaskStatus = model.TaskStatus;
                            newTask.UpdationDate = UtilService.GetPkCurrentDateTime();
                            db.Entry(newTask).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                            db.SaveChanges();

                            return 1;

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
       
        //---------------------App And Web API-----------
        #region GetDailyEngagementListsByFilter
        public async Task<List<ListDailyEngagementDTO>> GetDailyEngagementListsByFilter(Guid userid, DailyEngagementFilterDTO ContactkFilterDTO, bool isAdmin)
        {
            try
            {

                using (var db = new IDDbContext())
                {
                    //////////////////////////////////////////////////Filter//////////////////
                    
                    //////////////////////////////////////////////////Filter//////////////////

                    List<ListDailyEngagementDTO> res = new List<ListDailyEngagementDTO>();

                    if (isAdmin)

                        res = (from dailyEngagement in db.tbl_DailyEngagement
                               //join user in db.AspNetUsers on dailyEngagement.CreatedBy equals user.Id.ToString()
                              
                               where dailyEngagement.RecordStatus == ContactkFilterDTO.RecordStatus && dailyEngagement.EngagementSatusId == ContactkFilterDTO.DailyEngagementStatusId

                               select new ListDailyEngagementDTO
                               {
                                   Id = dailyEngagement.Id,

                                   Task = dailyEngagement.Task,
                                   WhereToGo = dailyEngagement.WhereToGo,
                                   WhenToGo = dailyEngagement.WhenToGo,
                                    VenueId = dailyEngagement.VenueId,
                                   VenueIdName = dailyEngagement.Venue.Venue,

                                   CreatedBy = dailyEngagement.CreatedBy,
                                   CreationDate = dailyEngagement.CreationDate,
                                   EngagementSatusId= dailyEngagement.EngagementSatusId,
                                   EngagementStatusIdName=dailyEngagement.EngagementSatusNavigation.Status,

                               }
                            ).ToList();
                    else
                        res = (from dailyEngagement in db.tbl_DailyEngagement
                               //join user in db.AspNetUsers on dailyEngagement.CreatedBy equals user.Id.ToString()
                               //where dailyEngagement.RecordStatus == true && dailyEngagement.CreatedBy == userid.ToString()
                               where dailyEngagement.RecordStatus == true 

                               select new ListDailyEngagementDTO
                               {
                                   Id = dailyEngagement.Id,

                                   Task = dailyEngagement.Task,
                                   WhereToGo = dailyEngagement.WhereToGo,
                                   WhenToGo = dailyEngagement.WhenToGo,

                                   VenueId = dailyEngagement.VenueId,
                                   VenueIdName = dailyEngagement.Venue.Venue,
                                   CreatedBy = dailyEngagement.CreatedBy,
                                   CreationDate = dailyEngagement.CreationDate,
                                   EngagementSatusId = dailyEngagement.EngagementSatusId,
                                   EngagementStatusIdName = dailyEngagement.EngagementSatusNavigation.Status,

                               }
                            ).ToList();






                    return res;


                }
            }
            catch
            {
                throw;
            }
        }

        #endregion

      



        public string ReadJson()
        {
            using (StreamReader r = new StreamReader(_env.ContentRootPath + "/Json/Menu.json"))
            {
                string json = r.ReadToEnd();
                return json;
            }

        }



        public void DeleteDailyEngagement(int Id)
        {

            using (var db = new IDDbContext())
            {
                using (var trans = db.Database.BeginTransaction())
                {
                    try
                    {

                        if (Id != 0)
                        {

                            var deleteDailyEngagement = db.tbl_DailyEngagement.Where(x => x.Id == Id).FirstOrDefault();

                            deleteDailyEngagement.RecordStatus = false;
                            deleteDailyEngagement.IsDeleted = true;

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


        #region GetDailyEngagementPublicList
        public List<DailyEngagementDTO> GetDailyEngagementPublicList()
        {
            try
            {
                using (var db = new IDDbContext())
                {


                    List<DailyEngagementDTO> res = new List<DailyEngagementDTO>();

                    res = (from dailyEngagementVar in db.tbl_DailyEngagement
                               //join user in db.AspNetUsers on task.CreatedBy equals user.Id.ToString()
                           where dailyEngagementVar.RecordStatus == true
                           select new DailyEngagementDTO
                           {

                               Id = dailyEngagementVar.Id,

                               Task = dailyEngagementVar.Task,
                               WhereToGo = dailyEngagementVar.WhereToGo,
                               WhenToGo = dailyEngagementVar.WhenToGo,

                               VenueId = dailyEngagementVar.VenueId,
                               VenueIdName = dailyEngagementVar.Venue.Venue,
                               CreatedBy = dailyEngagementVar.CreatedBy,
                               CreationDate = dailyEngagementVar.CreationDate,
                               EngagementSatusId = dailyEngagementVar.EngagementSatusId,
                               EngagementStatusIdName = dailyEngagementVar.EngagementSatusNavigation.Status,

                               

                           }).ToList();



                    return res;

                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion







        #region GetDailyEngagementCount
        public DailyEngagementCount GetDailyEngagementCount(Guid? userid)
        {
            try
            {
                using (var db = new IDDbContext())
                {

                    var tasks = db.tbl_DailyEngagement.Where(x => x.IsDeleted==false).AsQueryable();
                    //if (userid != null)
                    //{
                    //    tasks = tasks.Where(x =>x.CreatedBy == userid.);


                    //}

                    var result = tasks.ToList();
                    var output = new DailyEngagementCount
                    {
                        Total = result.Count(),
                        Active = result.Count(x => x.EngagementSatusId == 1 && x.RecordStatus == true),
                        InActive = result.Count(x => x.EngagementSatusId == 2 && x.RecordStatus == false),
                        Postponed = result.Count(x => x.EngagementSatusId == 3 && x.RecordStatus == true),
                        Rescheduled = result.Count(x => x.EngagementSatusId == 4 && x.RecordStatus == true),
                        Cancelled = result.Count(x => x.EngagementSatusId == 5 && x.RecordStatus == true),

                        Archived = result.Count(x => x.EngagementSatusId == 6 && x.RecordStatus == false)
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

        #region GetDailyEngagementListsByStatus
        public List<DailyEngagementDTO> GetDailyEngagementListsByStatus(Guid userid, int Status)
        {
            try
            {


                using (var db = new IDDbContext())
                {

                    List<DailyEngagementDTO> res = new List<DailyEngagementDTO>();

                    if (Status == 1 || Status == 3 || Status == 4 || Status == 5)
                    {
                        res = (from t in db.tbl_DailyEngagement
                               join c in db.tbl_MeetingStatus on t.EngagementSatusId equals c.Id
                               where ((t.EngagementSatusId == Status &&
                                t.RecordStatus == true && t.IsDeleted == false) || (Status == 0 && t.RecordStatus == true && t.IsDeleted == false))


                               select new DailyEngagementDTO
                               {
                                   Id = t.Id,
                                   Task = t.Task,
                                   VenueId = t.VenueId,
                                   VenueIdName = t.Venue.Venue,


                                   WhenToGo = t.WhenToGo,

                                   EngagementSatusId = c.Id,
                                   EngagementStatusIdName = c.Status,
                                   RecordStatus = t.RecordStatus,

                                   CreatedBy = t.CreatedBy,
                                   UpdatedBy = t.UpdatedBy,

                                   IsDeleted = t.IsDeleted,





                               }).ToList();




                        return res;
                    }

                    else if (Status == 6 || Status == 2)
                    {
                        res = (from t in db.tbl_DailyEngagement
                               join c in db.tbl_MeetingStatus on t.EngagementSatusId equals c.Id
                               where ((t.EngagementSatusId == Status &&
                                t.RecordStatus == false && t.IsDeleted == false) || (Status == 0 && t.RecordStatus == false && t.IsDeleted == false))


                               select new DailyEngagementDTO
                               {
                                   Id = t.Id,
                                   Task = t.Task,
                                   VenueId = t.VenueId,
                                   VenueIdName = t.Venue.Venue,


                                   WhenToGo = t.WhenToGo,

                                   EngagementSatusId = c.Id,
                                   EngagementStatusIdName = c.Status,
                                   RecordStatus = t.RecordStatus,

                                   CreatedBy = t.CreatedBy,
                                   UpdatedBy = t.UpdatedBy,

                                   IsDeleted = t.IsDeleted,





                               }).ToList();




                        return res;
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



        #region GetDailyEngagementById
        public List<DailyEngagementDTO> GetDailyEngagementById(int taskId)
        {
            try
            {


                using (var db = new IDDbContext())
                {

                    List<DailyEngagementDTO> res = new List<DailyEngagementDTO>();

                    res = db.tbl_DailyEngagement
                        .Where(t => t.Id == taskId && t.RecordStatus == true)
                        .Select(t => new DailyEngagementDTO
                        {
                            Id = t.Id,

                            Task = t.Task,
                            WhereToGo = t.WhereToGo,
                            WhenToGo = t.WhenToGo,

                            VenueId = t.VenueId,
                            VenueIdName = t.Venue.Venue,
                            CreatedBy = t.CreatedBy,
                            CreationDate = t.CreationDate,
                            EngagementSatusId = t.EngagementSatusId,
                            EngagementStatusIdName = t.EngagementSatusNavigation.Status,


                        }).ToList();

                    return res;

                }
            }
            catch
            {
                throw;
            }
        }

        #endregion

    }

}
