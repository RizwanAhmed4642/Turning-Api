using AutoMapper;
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
    public class TaskService
    {
        #region Fields
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _env;
        private readonly NotificationService _notificationService = new NotificationService();
        #endregion

        #region Constructors
        public TaskService(IMapper mapper, IWebHostEnvironment env)
        {

            _mapper = mapper;
            _env = env;
        }
        #endregion

        #region AddTask
        public async Task<int> AddTask(TaskDTO model, string userid)
        {

            using (var db = new IDDbContext())
            {
                using (var trans = db.Database.BeginTransaction())
                {
                    try
                    {
                        var userDesignation = db.AspNetUsers.Where(x => x.Id.ToString() == model.Designation).FirstOrDefault().Designation;
                        //var userDesignation = db.AspNetUsers.Where(x => model.TaskAssignees.Any(x=>x.TaskAssigneeId.ToString().Contains(x.Id.ToString()))).Select(x=>x.).ToList();
                        if (model.Id == 0 || model.Id == null)
                        {

                            var newTask = this._mapper.Map<tbl_Task>(model);

                            // newTask.Attachment = ;
                            newTask.RecordStatus = true;
                            newTask.CreatedBy = userid;
                            newTask.CreationDate = UtilService.GetPkCurrentDateTime();
                            newTask.TaskStatus = model.TaskStatus;
                            // newTask.Designation =String.Join(",",model.TaskAssignees.Select(x=>x.TaskAssigneeName).ToArray());
                            newTask.Designation = userDesignation;
                            newTask.Priority = model.Priority;
                            newTask.ParentTaskId = model.ParentTaskId;


                            // newTask.tas = new List<tbl_TaskAssignee>();


                            //var TaskAssingees = "";
                            //foreach (var r in model.TaskAssignees)
                            //{
                            //    TaskAssingees += r.TaskAssigneeName + ", ";
                            //}

                            //newTask.Tbl_TaskCcs = new List<tbl_TaskCc>();

                            //foreach (var p in model.TaskCcs)
                            //{
                            //    newTask.Tbl_TaskCcs.Add(
                            //    new tbl_TaskCc
                            //    {
                            //        TaskCcID = p.TaskCcId,
                            //        TaskId = newTask.Id

                            //    }
                            //    );
                            //}
                            //var TaskCcs = "";
                            //foreach (var r in model.TaskCcs)
                            //{
                            //    TaskCcs += r.TaskCcName + ", ";
                            //}

                            db.tbl_Task.Add(newTask);

                            db.SaveChanges();
                            foreach (var p in model.TaskCcs)
                            {
                                tbl_TaskCc taskCc = new tbl_TaskCc();
                                taskCc.TaskId = newTask.Id;
                                taskCc.TaskCcID = p.TaskCcId;
                                db.tbl_TaskCc.Add(taskCc);
                                db.SaveChanges();


                                _notificationService.AddNotification(new NotificationModel
                                {
                                    Title = "New CC Assigned!",
                                    Description = "You have been added as CC in a new task.",
                                    UserIdFrom = Guid.Parse(newTask.CreatedBy),
                                    UserIdTo = (Guid)p.TaskCcId,
                                    Link = "/taskDetail",
                                    SourceId = newTask.Id.ToString(),
                                    SourceType = "Task",
                                    RecordStatus = true

                                });
                                try {
                                    SendSMSUfone(new SMSViewModel
                                    {
                                        Body = "Dear Sir, You have been added as CC in a new task by competent authority. \n\r You are requested to visit task management system" +
                                 " through https://dsr.pshealthpunjab.gov.pk/mainDashboard",

                                        Receiver = db.AspNetUsers.FirstOrDefault(x => x.Id == p.TaskCcId).PhoneNumber

                                    });
                                    var task = db.tbl_Task.Where(x => x.Id == newTask.Id).FirstOrDefault();

                                    task.IsSMSSent = true;

                                    db.SaveChanges();
                                }
                                catch (Exception)
                                {
                                    var task = db.tbl_Task.Where(x => x.Id == newTask.Id).FirstOrDefault();

                                    task.IsSMSSent = false;

                                    db.SaveChanges();

                                }




                            }

                            foreach (var p in model.TaskAssignees)
                            {
                                tbl_TaskAssignee taskAssignee = new tbl_TaskAssignee();
                                taskAssignee.TaskId = newTask.Id;
                                taskAssignee.TaskAssignToID = p.TaskAssigneeId;
                                db.tbl_TaskAssignee.Add(taskAssignee);
                                db.SaveChanges();

                                _notificationService.AddNotification(new NotificationModel
                                {
                                    Title = "New Task Assigned!",
                                    Description = "You have been assigned a new task.",
                                    UserIdFrom = Guid.Parse(newTask.CreatedBy),
                                    UserIdTo = (Guid)p.TaskAssigneeId,
                                    Link = "/taskDetail",
                                    SourceId = newTask.Id.ToString(),
                                    SourceType = "Task",
                                    RecordStatus = true

                                });


                                try {
                                    SendSMSUfone(new SMSViewModel
                                    {
                                        Body = "Dear Sir, You have been assigned a new task by competent authority. \n\r You are requested to visit task management system" +
                                  " through https://dsr.pshealthpunjab.gov.pk/mainDashboard",

                                        Receiver = db.AspNetUsers.FirstOrDefault(x => x.Id == p.TaskAssigneeId).PhoneNumber

                                    });

                                    var task = db.tbl_Task.Where(x => x.Id == newTask.Id).FirstOrDefault();

                                    task.IsSMSSent = true;

                                    db.SaveChanges();
                                }
                                catch (Exception)
                                {

                                    var task = db.tbl_Task.Where(x => x.Id == newTask.Id).FirstOrDefault();

                                    task.IsSMSSent = false;

                                    db.SaveChanges();
                                }


                            }







                            if (model.TaskAttachments != null)
                            {
                                foreach (var item in model.TaskAttachments)
                                {
                                    var newAttachment = new Attachment();
                                    newAttachment.AttachmentName = await UploadFile(item);
                                    newAttachment.TaskId = newTask.Id;
                                    newAttachment.SourceName = "Task";
                                    newAttachment.RecordStatus = true;

                                    db.Attachment.Add(newAttachment);
                                    db.SaveChanges();
                                }
                            }



                            trans.Commit();
                            return 1;

                        }
                        else
                        {
                            // Map Data of input model
                            var newTask = this._mapper.Map<tbl_Task>(model);
                            // newTask.Attachment = await UploadFile(model.Attachment[0]);
                            newTask.UpdatedBy = userid;
                            newTask.TaskStatus = model.TaskStatus;
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
        public SMSViewModel SendSMSTelenor(SMSViewModel sms)
        {
            //string userName = "923464950571";
            //string password = "Hisdu%40012_p%26shd-Hisdu%2Fp%26shd";

            string userName = "923464950571";
            string password = "Hisdu%40012_p%26shd-Hisdu%2Fp%26shd";

            SMSSendingService service = new SMSSendingService(userName, password);
            sms.Mask = sms.Mask == null ? "PSHD" : sms.Mask;
            //sms.Mask = sms.Mask == null ? "P%26SHD" : sms.Mask;
            sms.Sender = userName;
            sms = service.SendQuickMessage(sms);

            return sms;
        }

        #region AddTaskDetail
        public async Task<int> AddTaskDetail(TaskDetailDTO model, string userid)
        {

            using (var db = new IDDbContext())
            {
                using (var trans = db.Database.BeginTransaction())
                {
                    try
                    {


                        if (model.TaskId != 0)
                        {
                            var task = db.tbl_Task.Where(x => x.Id == model.TaskId).FirstOrDefault();
                            if (model.TaskStatus != 0)
                            {


                                task.TaskStatus = model.TaskStatus;
                                if (model.TaskStatus == 9)
                                {
                                    task.IsReopen = true;
                                }
                                db.SaveChanges();
                            }
                            if (model.Comments != null)
                            {

                                var newTaskDetatil = new tbl_TaskDetails();

                                newTaskDetatil.Comments = model.Comments;
                                newTaskDetatil.TaskId = model.TaskId;
                                // newTask.Attachment = await UploadFile(model.Attachment[0]);
                                newTaskDetatil.RecordStatus = true;
                                newTaskDetatil.IsRead = false;
                                newTaskDetatil.CreatedBy = userid;
                                newTaskDetatil.CreatedDate = UtilService.GetPkCurrentDateTime();
                                if (userid == task.CreatedBy)
                                {
                                    newTaskDetatil.CommentFor = task.AssignTo.ToString();
                                }
                                else
                                {
                                    newTaskDetatil.CommentFor = task.CreatedBy.ToString();
                                }
                                db.tbl_TaskDetails.Add(newTaskDetatil);

                                db.SaveChanges();

                                if (model.TaskAttachments != null)
                                {
                                    foreach (var item in model.TaskAttachments)
                                    {
                                        var newAttachment = new Attachment();
                                        newAttachment.AttachmentName = await UploadFile(item);
                                        newAttachment.TaskDetailId = newTaskDetatil.Id;
                                        newAttachment.SourceName = "Comments";
                                        newAttachment.RecordStatus = true;

                                        db.Attachment.Add(newAttachment);
                                        db.SaveChanges();
                                    }
                                }
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

        #region GetTasksList
        public List<ListTaskDTO> GetTasksList(Guid userid)
        {
            try
            {

                using (var db = new IDDbContext())
                {

                    List<ListTaskDTO> res = new List<ListTaskDTO>();

                    res = (from task in db.tbl_Task
                           join user in db.AspNetUsers on task.CreatedBy equals user.Id.ToString()
                           where task.CreatedBy == userid.ToString() || task.tbl_TaskAssignee.Any(x => x.TaskAssignToID == userid) && task.RecordStatus == true
                           select new ListTaskDTO
                           {
                               Id = task.Id,
                               Title = task.Title,
                               Description = task.Description,
                               Designation = task.Designation,
                               AssignTo = task.AssignTo,
                               DueDate = task.DueDate,
                               Priority = task.Priority,
                               CreatedDate = task.CreationDate,
                               TaskStatus = task.TaskStatusNavigation.Status,
                               RecordStatus = task.RecordStatus,
                               CreatedBy = user.Designation,
                               Attachment = task.Attachment
                              .Where(x => x.SourceName == "Task")
                              .Select(x => "/Uploads/" + x.AttachmentName).ToList()
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

        #region GetTasksCount
        public TaskCount GetTasksCount(Guid? userid)
        {
            try
            {
                using (var db = new IDDbContext())
                {

                    var tasks = db.tbl_Task.Where(x => x.RecordStatus == true).AsQueryable();
                    if (userid != null)
                    {
                        tasks = tasks.Where(x => x.tbl_TaskAssignee.Any(x => x.TaskAssignToID == userid) || x.tbl_TaskCc.Any(x => x.TaskCcID == userid) || x.CreatedBy == userid.ToString());


                    }

                    var result = tasks.ToList();
                    var output = new TaskCount
                    {
                        Total = result.Count(),
                        Completed = result.Count(x => x.TaskStatus == 4),
                        Pending = result.Count(x => x.TaskStatus == 1 || x.TaskStatus == 2),
                        Submitted = result.Count(x => x.TaskStatus == 7),
                        InProgress = result.Count(x => x.TaskStatus == 3),
                        ReOpen = result.Count(x => x.TaskStatus == 9),
                        OverDue = result.Count(x => x.DueDate < DateTime.Now && (x.TaskStatus == 1 || x.TaskStatus == 3))

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

        #region GetAllTasks
        public List<ListTaskDTO> GetAllTasks(Guid userid, bool AllRecord)
        {
            try
            {
                using (var db = new IDDbContext())
                {

                    if (AllRecord)
                    {


                        List<ListTaskDTO> res = new List<ListTaskDTO>();

                        res = (from task in db.tbl_Task
                               join user in db.AspNetUsers on task.CreatedBy equals user.Id.ToString()
                               where task.RecordStatus == true
                               select new ListTaskDTO
                               {
                                   Id = task.Id,
                                   Title = task.Title,
                                   Description = task.Description,
                                   Designation = task.Designation,
                                   AssignTo = task.AssignTo,
                                   DueDate = task.DueDate,
                                   Priority = task.Priority,
                                   TaskStatus = task.TaskStatusNavigation.Status,
                                   RecordStatus = task.RecordStatus,
                                   CreatedBy = user.Designation,
                                   Attachment = task.Attachment
                                  .Where(x => x.SourceName == "Task")
                                  .Select(x => "/Uploads/" + x.AttachmentName).ToList()
                               }).ToList();

                        return res;
                    }
                    else
                    {
                        List<ListTaskDTO> res = new List<ListTaskDTO>();

                        res = (from task in db.tbl_Task
                               join user in db.AspNetUsers on task.CreatedBy equals user.Id.ToString()
                               where task.RecordStatus == true && (task.tbl_TaskAssignee.Any(x => x.TaskAssignToID == userid) || task.CreatedBy == userid.ToString())
                               select new ListTaskDTO
                               {
                                   Id = task.Id,
                                   Title = task.Title,
                                   Description = task.Description,
                                   Designation = task.Designation,
                                   AssignTo = task.AssignTo,
                                   DueDate = task.DueDate,
                                   Priority = task.Priority,
                                   TaskStatus = task.TaskStatusNavigation.Status,
                                   RecordStatus = task.RecordStatus,
                                   CreatedBy = user.Designation,
                                   Attachment = task.Attachment
                                  .Where(x => x.SourceName == "Task")
                                  .Select(x => "/Uploads/" + x.AttachmentName).ToList()
                               }).ToList();

                        return res;
                    }

                }
            }
            catch
            {
                throw;
            }
        }

        #endregion

        #region GetTasksListByStatus
        public List<ListTaskDTO> GetTasksListByStatus(Guid userid, int Status)
        {
            try
            {


                using (var db = new IDDbContext())
                {

                    List<ListTaskDTO> res = new List<ListTaskDTO>();

                    res = (from t in db.tbl_Task
                           join c in db.tbl_Status on t.TaskStatus equals c.Id
                           where (t.CreatedBy == userid.ToString() || t.tbl_TaskAssignee.Any(x => x.TaskAssignToID == userid)
                           && t.TaskStatus == Status && t.RecordStatus == true)
                           select new ListTaskDTO
                           {
                               Id = t.Id,
                               Title = t.Title,
                               Description = t.Description,
                               Designation = t.Designation,
                               AssignTo = t.AssignTo,
                               Priority = t.Priority,
                               DueDate = t.DueDate,
                               //Attachment = t.Attachment,
                               TaskStatus = c.Status,
                               RecordStatus = t.RecordStatus

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

        #region GetTaskListsByFilter
        public async Task<List<ListTaskDTO>> GetTaskListsByFilter(Guid userid, TaskFilterDTO TaskFilterDTO, bool isAdmin)
        {
            try
            {

                using (var db = new IDDbContext())
                {
                    List<ListTaskDTO> res = new List<ListTaskDTO>();

                    if (isAdmin)

                        res = (from task in db.tbl_Task
                               join user in db.AspNetUsers on task.CreatedBy equals user.Id.ToString()
                               where task.RecordStatus == true
                               select new ListTaskDTO
                               {
                                   Id = task.Id,
                                   Title = task.Title,
                                   Description = task.Description,
                                   Designation = task.Designation,
                                   AssignTo = task.AssignTo,
                                   DueDate = task.DueDate,
                                   ExtendedDate = task.ExtendedDate,
                                   Priority = task.Priority,
                                   TaskStatus = task.TaskStatusNavigation.Status,
                                   RecordStatus = task.RecordStatus,
                                   CreatedBy = user.Designation,
                                   CreatedDate = task.CreationDate,
                                   CreatedById = task.CreatedBy,
                                   TaskAssignee = String.Join(",", task.tbl_TaskAssignee.Select(x => x.TaskAssignTo.FullName).ToList()),
                                   TaskAssigneeId = String.Join(",", task.tbl_TaskAssignee.Select(x => x.TaskAssignToID).ToList()),
                                   TaskCC = String.Join(",", task.tbl_TaskCc.Select(x => x.TaskCc.FullName).ToList()),
                                   UnReadComentsCount = task.tbl_TaskDetails.Where(x => x.IsRead == false && x.CreatedBy != userid.ToString()).Count(),
                                   ParentTaskId=task.ParentTaskId,
                                   IsSMSSent = task.IsSMSSent,

                                   Attachment = task.Attachment
                                    .Where(x => x.SourceName == "Task")
                                    .Select(x => "/Uploads/" + x.AttachmentName).ToList()
                               }
                            ).OrderByDescending(x => x.UnReadComentsCount).ToList();
                    else
                        res = (from task in db.tbl_Task
                               join user in db.AspNetUsers on task.CreatedBy equals user.Id.ToString()
                               where task.RecordStatus == true && (task.tbl_TaskAssignee.Any(x => x.TaskAssignToID == userid) || task.tbl_TaskCc.Any(x => x.TaskCcID == userid)) || task.CreatedBy == userid.ToString()

                               select new ListTaskDTO
                               {
                                   Id = task.Id,
                                   Title = task.Title,
                                   Description = task.Description,
                                   Designation = task.Designation,
                                   AssignTo = task.AssignTo,
                                   DueDate = task.DueDate,
                                   ExtendedDate = task.ExtendedDate,
                                   Priority = task.Priority,
                                   TaskStatus = task.TaskStatusNavigation.Status,
                                   RecordStatus = task.RecordStatus,
                                   CreatedDate = task.CreationDate,
                                   CreatedBy = user.Designation,
                                   CreatedById = task.CreatedBy,
                                   TaskAssignee = String.Join(",", task.tbl_TaskAssignee.Select(x => x.TaskAssignTo.FullName).ToList()),
                                   TaskAssigneeId = String.Join(",", task.tbl_TaskAssignee.Select(x => x.TaskAssignToID).ToList()),
                                   TaskCC = String.Join(",", task.tbl_TaskCc.Select(x => x.TaskCc.Designation).ToList()),
                                   UnReadComentsCount = task.tbl_TaskDetails.Where(x => x.IsRead == false && x.CreatedBy != userid.ToString()).Count(),
                                   ParentTaskId = task.ParentTaskId,
                                   IsSMSSent = task.IsSMSSent,
                                   Attachment = task.Attachment
                                     .Where(x => x.SourceName == "Task")
                                     .Select(x => "/Uploads/" + x.AttachmentName).ToList()
                               }
                            ).OrderByDescending(x => x.UnReadComentsCount).ToList();

                    //tasks = tasks.Where(x => x.AssignTo == userid).ToList();

                    if (TaskFilterDTO.Priority != "All")
                    {
                        res = res.Where(x => x.Priority == TaskFilterDTO.Priority).ToList();
                    }
                    if (TaskFilterDTO.userList != null)
                    {

                        res = res.Where(x => x.TaskAssigneeId.Contains(TaskFilterDTO.userList.ToString())).ToList();
                        //res = res.Where(x => x.AssignTo == TaskFilterDTO.userList).ToList();


                    }
                    if (TaskFilterDTO.Status != "All")
                    {
                        if (TaskFilterDTO.Status == "Overdue")
                        {
                            res = res.Where(x => x.DueDate < DateTime.Now && x.TaskStatus != "Completed" && x.TaskStatus != "Submit For Approval").ToList();
                        }
                        else
                        {
                            res = res.Where(x => x.TaskStatus.ToString() == TaskFilterDTO.Status).ToList();
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

        #region GetDashboardCount
        public DashboardCount GetDashboardCount(Guid userid)
        {
            try
            {


                using (var db = new IDDbContext())
                {
                    DashboardCount dashboardCount = new DashboardCount();
                    var res = db.tbl_Task.Where(x => (x.CreatedBy == userid.ToString() || x.tbl_TaskAssignee.Any(x => x.TaskAssignToID == userid) || x.tbl_TaskCc.Any(x => x.TaskCcID == userid)) && x.RecordStatus == true).ToList();
                    dashboardCount.Pending = res.Where(x => x.TaskStatus == 1).Count();
                    dashboardCount.InProcess = res.Where(x => x.TaskStatus == 2).Count();
                    dashboardCount.Rejected = res.Where(x => x.TaskStatus == 5).Count();
                    dashboardCount.Resolved = res.Where(x => x.TaskStatus == 4).Count();
                    return dashboardCount;

                }
            }
            catch
            {
                throw;
            }
        }

        #endregion

        #region GetTaskById
        public List<ListTaskDTO> GetTaskById(int taskId)
        {
            try
            {


                using (var db = new IDDbContext())
                {

                    List<ListTaskDTO> res = new List<ListTaskDTO>();

                    res = db.tbl_Task
                        .Where(t => t.Id == taskId && t.RecordStatus == true)
                        .Select(t => new ListTaskDTO
                        {
                            Id = t.Id,
                            Title = t.Title,
                            Description = t.Description,
                            Designation = t.Designation,
                            AssignTo = t.AssignTo,
                            Priority = t.Priority,
                            DueDate = t.DueDate,
                            TaskStatus = t.TaskStatusNavigation.Status,
                            RecordStatus = t.RecordStatus,
                            Attachment = t.Attachment.Where(x => x.SourceName == "Task").Select(x => "/Uploads/" + x.AttachmentName).ToList(),
                            CreatedBy = t.CreatedBy,
                            TaskAssignee = String.Join(",", t.tbl_TaskAssignee.Select(x => x.TaskAssignTo.FullName).ToList()),
                            TaskAssigneeId = String.Join(",", t.tbl_TaskAssignee.Select(x => x.TaskAssignToID).ToList()),
                            ExtendedDate = t.ExtendedDate,
                            //t.SubTaskId == taskId
                            ParentTaskId = t.ParentTaskId,
                            IsAssignAble = db.tbl_Task.Where(x => x.ParentTaskId == t.Id).Count() > 0 ? true : false,

                            IsParentTaskDetail = db.tbl_Task.Where(x => x.Id == t.ParentTaskId).Select(t => new ListTaskDTO
                            {
                                Id = t.Id,
                                Title = t.Title,
                                Description = t.Description,
                                Designation = t.Designation,
                                AssignTo = t.AssignTo,
                                Priority = t.Priority,
                                DueDate = t.DueDate,
                                TaskStatus = t.TaskStatusNavigation.Status,
                                RecordStatus = t.RecordStatus,
                                Attachment = t.Attachment.Where(x => x.SourceName == "Task").Select(x => "/Uploads/" + x.AttachmentName).ToList(),
                                CreatedBy = t.CreatedBy,
                                ExtendedDate = t.ExtendedDate,
                                //t.SubTaskId == taskId
                                ParentTaskId = t.ParentTaskId,
                                TaskAssignee = String.Join(",", t.tbl_TaskAssignee.Select(x => x.TaskAssignTo.FullName).ToList()),
                            }).ToList(),


                            IsSubTaskDetail = db.tbl_Task.Where(x => x.ParentTaskId == t.Id).Select(t => new ListTaskDTO
                            {
                                Id = t.Id,
                                Title = t.Title,
                                Description = t.Description,
                                Designation = t.Designation,
                                AssignTo = t.AssignTo,
                                Priority = t.Priority,
                                DueDate = t.DueDate,
                                TaskStatus = t.TaskStatusNavigation.Status,
                                RecordStatus = t.RecordStatus,
                                Attachment = t.Attachment.Where(x => x.SourceName == "Task").Select(x => "/Uploads/" + x.AttachmentName).ToList(),
                                CreatedBy = t.CreatedBy,
                                ExtendedDate = t.ExtendedDate,
                                //t.SubTaskId == taskId
                                ParentTaskId = t.ParentTaskId,
                                TaskAssignee = String.Join(",", t.tbl_TaskAssignee.Select(x => x.TaskAssignTo.FullName).ToList()),
                            }).ToList()
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

        #region GetTaskDetails
        public List<TaskDetailDTO> GetTaskDetails(int taskid)
        {
            try
            {

                using (var db = new IDDbContext())
                {

                    List<TaskDetailDTO> res = new List<TaskDetailDTO>();

                    res = (from t in db.tbl_TaskDetails
                           join c in db.AspNetUsers on t.CreatedBy equals c.Id.ToString()
                       
                           where (t.TaskId == taskid && t.RecordStatus == true)
                           select new TaskDetailDTO
                           {
                               Id = t.Id,
                               TaskStatusName = t.Task.TaskStatusNavigation.Status,
                               TaskId = (int)t.TaskId,
                               Attachment = t.Attachment
                               .Where(x => x.SourceName == "Comments")
                               .Select(x => "/Uploads/" + x.AttachmentName).ToList(),
                               Comments = t.Comments,
                               ReadDateTime = t.ReadDateTime,
                               IsRead = t.IsRead,
                               CreatedBy = c.Designation,
                               CreatedDate = t.CreatedDate,
                               TaskAssignee = String.Join(",", t.Task.tbl_TaskAssignee.Select(x => x.TaskAssignTo.FullName).ToList()),
                               TaskAssigneeId = String.Join(",", t.Task.tbl_TaskAssignee.Select(x => x.TaskAssignToID).ToList()),
                               ExtendedDate = t.Task.ExtendedDate


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

        public string ReadJson()
        {
            using (StreamReader r = new StreamReader(_env.ContentRootPath + "/Json/Menu.json"))
            {
                string json = r.ReadToEnd();
                return json;
            }

        }



        public void DeleteTask(int Id)
        {

            using (var db = new IDDbContext())
            {
                using (var trans = db.Database.BeginTransaction())
                {
                    try
                    {

                        if (Id != 0)
                        {

                            var task = db.tbl_Task.Where(x => x.Id == Id).FirstOrDefault();

                            task.RecordStatus = false;

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

        public void DeleteTaskComment(int Id)
        {

            using (var db = new IDDbContext())
            {
                using (var trans = db.Database.BeginTransaction())
                {
                    try
                    {

                        if (Id != 0)
                        {

                            var deleteComment = db.tbl_TaskDetails.Where(x => x.Id == Id).FirstOrDefault();

                            deleteComment.RecordStatus = false;

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


        #region ReSendSMS
        

        public int ReSendSMS(int Id)
        {

            using (var db = new IDDbContext())
            {

                using (var trans = db.Database.BeginTransaction())
                {
                    var taskCc = db.tbl_TaskCc.Where(x => x.TaskId == Id).ToList();
                    var taskAssignee = db.tbl_TaskAssignee.Where(x => x.TaskId == Id).ToList();
                    try
                    {

                        foreach (var p in taskCc)
                        {
                            try
                            {
                                SendSMSUfone(new SMSViewModel
                                {
                                    Body = "Dear Sir, You have been added as CC in a new task by competent authority. \n\r You are requested to visit task management system" +
                             " through https://dsr.pshealthpunjab.gov.pk/mainDashboard",

                                    Receiver = db.AspNetUsers.FirstOrDefault(x => x.Id == p.TaskCcID).PhoneNumber

                                });

                                var task = db.tbl_Task.Where(x => x.Id == Id).FirstOrDefault();

                                task.IsSMSSent = true;

                                db.SaveChanges();
                            }
                            catch (Exception)
                            {
                                var task = db.tbl_Task.Where(x => x.Id == Id).FirstOrDefault();

                                task.IsSMSSent = false;

                                db.SaveChanges();
                                return -1;
                            }




                        }

                        foreach (var p in taskAssignee)
                        {
                           

                            try
                            {
                                SendSMSUfone(new SMSViewModel
                                {
                                    Body = "Dear Sir, You have been assigned a new task by competent authority. \n\r You are requested to visit task management system" +
                              " through https://dsr.pshealthpunjab.gov.pk/mainDashboard",

                                    Receiver = db.AspNetUsers.FirstOrDefault(x => x.Id == p.TaskAssignToID).PhoneNumber

                                });

                                var task = db.tbl_Task.Where(x => x.Id == Id).FirstOrDefault();

                                task.IsSMSSent = true;

                                db.SaveChanges();
                            }
                            catch (Exception)
                            {

                                var task = db.tbl_Task.Where(x => x.Id == Id).FirstOrDefault();

                                task.IsSMSSent = false;

                                db.SaveChanges();
                                return -1;
                            }


                        }


                    }
                    catch (Exception)
                    {
                        trans.Rollback();
                        throw;
                    }

                    trans.Commit();
                    return 1;

                }

            }




        }

        #endregion

        public void ReadComment(int Id, string userId)
        {

            using (var db = new IDDbContext())
            {
                using (var trans = db.Database.BeginTransaction())
                {
                    try
                    {

                        if (Id != 0)
                        {

                            var taskDetail = db.tbl_TaskDetails.Where(x => x.TaskId == Id && x.IsRead == false).ToList();
                            foreach (var i in taskDetail)
                            {
                                if (i.CommentFor == userId)
                                {
                                    i.IsRead = true;
                                    i.ReadDateTime = DateTime.Now;
                                }
                            }




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



        public int UpdateTaskExtendedDate(Guid userId, TaskDTO model)
        {

            using (var db = new IDDbContext())
            {
                using (var trans = db.Database.BeginTransaction())
                {
                    try
                    {
                        var _Task = db.tbl_Task.Find(model.Id);
                        if (_Task != null)
                        {




                            _Task.ExtendedDate = model.ExtendedDate;
                            _Task.UpdatedBy = userId.ToString();
                            _Task.UpdationDate = UtilService.GetPkCurrentDateTime();

                            db.tbl_Task.Update(_Task);
                            db.SaveChanges();




                            trans.Commit();
                            return 1;


                        }
                        return 0;

                    }
                    catch (Exception)
                    {
                        trans.Rollback();
                        throw;
                    }
                }

            }
        }

        //public SMSViewModel SendSMSUfone(SMSViewModel sms)
        //{
        //    SMSSendingService service = new SMSSendingService();
        //    sms.Mask = sms.Mask == null ? "PSHD" : sms.Mask;
        //    sms.Sender = "03315519747";
        //    sms = service.SendQuickMessage(sms);
        //    return sms;
        //}



        public SMSViewModel SendSMSUfone(SMSViewModel sms)
        {
            SMSSendingService service = new SMSSendingService();
            sms.Mask = sms.Mask == null ? "PSHD" : sms.Mask;
           // sms.Mask = sms.Mask == null ? "P%26SHD" : sms.Mask;
            sms.Sender = "03018482714";
            sms = service.SendQuickMessage(sms);
            return sms;
        }



        public SMSViewModel SendSMS(SMSViewModel sms)
        {
            return SendSMSUfone(sms);
        }





    }

}
