using AutoMapper;
using Meeting_App.Data.Database.Context;
using Meeting_App.Models;
using Meeting_App.Models.DTOs;
using Meeting_App.Service;
using Meeting_App.Service.Common;
using Meeting_App.SIgnalRHub;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Meeting_App.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        #region Fields
        private TaskService _taskService;
        private CommonService _cService;
        private NotificationService _notificationService=new NotificationService();

        private IHubContext<NotificationHub> _hub;

        #endregion

        #region Constructor
        public TaskController (UserManager<Applicationuser> user, TaskService taskService,IMapper mapper, IHubContext<NotificationHub> hub)
        {
            _taskService = taskService;
            _hub = hub;
            _cService = new CommonService(user);
            
        }
        #endregion

        

        #region AddTask
        [HttpPost]
        [Route("AddTask")]
        public async Task<IActionResult> AddTask([FromForm] TaskDTO model)
        {
            try
            {
                int res = 0;
                
                ///
                string userid = this.GetUserId();
                //string userid = "333C3DEB-F4E0-460F-0A8A-08D9C9F8DBEA";

                model.TaskAssignees = JsonConvert.DeserializeObject<List<TaskAssigneeList>>(model.TaskAssigneeData);

                if(model.TaskCcData!="Not Available") { 
                model.TaskCcs = JsonConvert.DeserializeObject<List<TaskCcList>>(model.TaskCcData);
                }
                res = await _taskService.AddTask(model, userid);

                string msg = "";

                if (res == 1)
                {
                    msg = "Saved Successfully";
                    await _hub.Clients.All.SendAsync("transferchartdata", _notificationService.GetNotifications("*",userid, true));
                }
                else
                {
                    msg = "Updated Successfully";
                }
                return  Ok(UtilService.GetResponse<Json>(null,msg));


            }
            catch (Exception ex)
            {
                return BadRequest(UtilService.GetExResponse<Exception>(ex));
            }
        }
        #endregion

        #region AddTaskDetail
        [HttpPost]
        [Route("AddTaskDetail")]
        public async Task<IActionResult> AddTaskDetail([FromForm] TaskDetailDTO model)
        {
            try
            {
                int res = 0;

                ///
                 string userid = this.GetUserId();
                //string userid = "4474BD83-AF36-42F8-3BB1-08D9CAAF2517";


                res = await _taskService.AddTaskDetail(model, userid);

                string msg = "";

                if (res == 1)
                {
                    msg = "Saved Successfully";
                }
                else
                {
                    msg = "Task Id Should not be 0 or empty";
                }
                
                return Ok(UtilService.GetResponse<Json>(null, msg));


            }
            catch (Exception ex)
            {
                return BadRequest(UtilService.GetExResponse<Exception>(ex));
            }
        }
        #endregion

        #region GetAllTasks
        [Authorize]
        [HttpGet]
        [Route("GetAllTasks")]
        public async Task<IActionResult> GetAllTasks()
        {
            try
            {
                Guid userid = Guid.Parse(this.GetUserId());

                    List<ListTaskDTO> TaskList = _taskService.GetAllTasks(userid,await _cService.CheckRoleExists(userid, "Admin"));

                return Ok(UtilService.GetResponse(TaskList));
            }
            catch (Exception ex)
            {
                return Ok(UtilService.GetExResponse<Exception>(ex));
            }
        }

        #endregion

        #region GetTasksCount
      
        [HttpGet]
        [Route("GetTasksCount")]
        public async Task<IActionResult> GetTasksCount()
        {
            try
            {
                ///Get userid
                Guid userid = Guid.Parse(this.GetUserId());


               

                TaskCount TaskCount;

                if (await _cService.CheckRoleExists(userid, "Admin"))
                {
                     TaskCount = _taskService.GetTasksCount(null);
                }
                else
                {
                    TaskCount = _taskService.GetTasksCount(userid);
                }
                 

                return Ok(UtilService.GetResponse(TaskCount));
            }
            catch (Exception ex)
            {
                return Ok(UtilService.GetExResponse<Exception>(ex));
            }
        }

        #endregion

        #region GetTaskListByUserId
        //[Authorize]
        [HttpGet]
        [Route("GetTaskListByUserId")]
        public IActionResult GetTaskListByUserId(Guid userid)
        {
            try
            {
                ///Get userid
                //Guid userid = Guid.Parse(this.GetUserId());

               

                List<ListTaskDTO> TaskList = _taskService.GetTasksList(userid);

                return Ok(UtilService.GetResponse(TaskList));
            }
            catch (Exception ex)
            {
                return Ok(UtilService.GetExResponse<Exception>(ex));
            }
        }

        #endregion

        #region GetTaskListsByStatus
        //[Authorize]
        [HttpGet]
        [Route("GetTaskListsByStatus")]
        public IActionResult GetTaskListsByStatus(int status)
        {
            try
            {
                ///Get userid
                 Guid userid = Guid.Parse(this.GetUserId());

                
                List<ListTaskDTO> TaskList = _taskService.GetTasksListByStatus(userid,status);

                return Ok(UtilService.GetResponse(TaskList));
            }
            catch (Exception ex)
            {
                return Ok(UtilService.GetExResponse<Exception>(ex));
            }
        }

        #endregion


        #region GetTaskListsByFilter
        //[Authorize]
        [HttpPost]
        [Route("GetTaskListsByFilter")]
        public async Task<IActionResult> GetTaskListsByFilter(TaskFilterDTO TaskFilterDTO)
        {
            try
            {
                ///Get userid
                Guid userid = Guid.Parse(this.GetUserId());

                List<ListTaskDTO> TaskList =await _taskService.GetTaskListsByFilter(userid, TaskFilterDTO, await _cService.CheckRoleExists(userid, "Admin"));

                return Ok(UtilService.GetResponse(TaskList));
            }
            catch (Exception ex)
            {
                return Ok(UtilService.GetExResponse<Exception>(ex));
            }
        }

        #endregion

        #region GetTaskById
        //[Authorize]
        [HttpGet]
        [Route("GetTaskById")]
        public IActionResult GetTaskById(int taskId)
        {
            try
            {
                ///Get userid
                ///Guid userid = Guid.Parse(this.GetUserId());


                List<ListTaskDTO> TaskList = _taskService.GetTaskById(taskId);

                return Ok(UtilService.GetResponse(TaskList));
            }
            catch (Exception ex)
            {
                return Ok(UtilService.GetExResponse<Exception>(ex));
            }
        }

        #endregion

        #region GetDashboardCount
        //[Authorize]
        [HttpGet]
        [Route("GetDashboardCount")]
        public IActionResult GetDashboardCount()
        {
            try
            {
                ///Get userid
                 Guid userid = Guid.Parse(this.GetUserId());


                DashboardCount dashboardCount = _taskService.GetDashboardCount(userid);

                return Ok(UtilService.GetResponse(dashboardCount));
            }
            catch (Exception ex)
            {
                return Ok(UtilService.GetExResponse<Exception>(ex));
            }
        }

        #endregion

        #region GetDetailsTaskList
        //[Authorize]
        [HttpGet]
        [Route("GetDetailsTask")]
        public IActionResult GetDetailsTaskList(int taskid)
        {
            try
            {
                ///Get userid
                // Guid userid = Guid.Parse(this.GetUserId());


                List<TaskDetailDTO> TaskList = _taskService.GetTaskDetails(taskid);

                return Ok(UtilService.GetResponse(TaskList));
            }
            catch (Exception ex)
            {
                return Ok(UtilService.GetExResponse<Exception>(ex));
            }
        }

        #endregion

        #region GetMenu
        //[Authorize]
        [HttpGet]
        [Route("GetMenu")]
        public IActionResult GetMenu()
        {
            try
            {
                ///Get userid
                // Guid userid = Guid.Parse(this.GetUserId());


               var TaskList = _taskService.ReadJson();

                Json pendencyDTO = JsonConvert.DeserializeObject<Json>(TaskList);


                return Ok(UtilService.GetResponse<Json>(pendencyDTO));
            }
            catch (Exception ex)
            {
                return Ok(UtilService.GetExResponse<Exception>(ex));
            }
        }

        #endregion


        #region DeleteTask
        //[Authorize]
        [HttpGet]
        [Route("DeleteTask")]
        public IActionResult DeleteTask(int Id)
        {
            try
            {

                  _taskService.DeleteTask(Id);

                return Ok(UtilService.GetResponse("Task Deleted"));
            }
            catch (Exception ex)
            {
                return Ok(UtilService.GetExResponse<Exception>(ex));
            }
        }

        #endregion

        #region ReSendSMS
        //[Authorize]
        [HttpGet]
        [Route("ReSendSMS")]
        public IActionResult ReSendSMS(int Id)
        {
            try
            {

               int status = _taskService.ReSendSMS(Id);
                if(status == 1)
                {
                    return Ok(UtilService.GetResponse("Sms Sent"));
                }
                else
                {
                    return Ok(UtilService.GetResponse("Sms not Sent"));
                }
                
            }
            catch (Exception ex)
            {
                return Ok(UtilService.GetExResponse<Exception>(ex));
            }
        }

        #endregion
        #region DeleteTaskComment
        //[Authorize]
        [HttpGet]
        [Route("DeleteTaskComment")]
        public IActionResult DeleteTaskComment(int Id)
        {
            try
            {

                _taskService.DeleteTaskComment(Id);

                return Ok(UtilService.GetResponse("Commented Deleted"));
            }
            catch (Exception ex)
            {
                return Ok(UtilService.GetExResponse<Exception>(ex));
            }
        }

        #endregion

        #region ReadComment
        //[Authorize]
        [HttpGet]
        [Route("ReadComment")]
        public IActionResult ReadComment(int Id)
        {
            try
            {
                string userid = this.GetUserId();
                _taskService.ReadComment(Id,userid);

                return Ok(UtilService.GetResponse("Commented Read"));
            }
            catch (Exception ex)
            {
                return Ok(UtilService.GetExResponse<Exception>(ex));
            }
        }

        #endregion

        #region UpdateTaskExtendedDate
        //[Authorize]
        [HttpPost]
        [Route("UpdateTaskExtendedDate")]
        public IActionResult UpdateTaskExtendedDate([FromForm] TaskDTO model)
        {
            try
            {
                Guid userId = Guid.Parse(this.GetUserId());
               
               int res = _taskService.UpdateTaskExtendedDate(userId , model);
                if(res == 1)
                return Ok(UtilService.GetResponse("Extended Date Added Successfully"));
               else
                    return Ok(UtilService.GetResponse("Model Cannot be empty"));
            }
            catch (Exception ex)
            {
                return Ok(UtilService.GetExResponse<Exception>(ex));
            }
        }

        #endregion

        #region GetSummariesAndNotes



        #endregion

    }
}
