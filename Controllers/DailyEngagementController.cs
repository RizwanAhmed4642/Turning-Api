using AutoMapper;
using Meeting_App.Data.Database.Tables;
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
    public class DailyEngagementController : ControllerBase
    {
        #region Fields
        private DailyEngagementService _dailyEngagementService;
        private CommonService _cService;
        private NotificationService _notificationService = new NotificationService();

        private IHubContext<NotificationHub> _hub;

        #endregion

        #region Constructor
        public DailyEngagementController(UserManager<Applicationuser> user, DailyEngagementService dailyEngagementService, IMapper mapper, IHubContext<NotificationHub> hub)
        {
            _dailyEngagementService = dailyEngagementService;
            _hub = hub;
            _cService = new CommonService(user);

        }
        #endregion



        #region AddDailyEngagement
        [HttpPost]
        [Route("AddDailyEngagement")]
        public IActionResult AddDailyEngagement([FromForm] DailyEngagementDTO model)
        {
            try
            {
                int res = 0;

                ///
                string userid = this.GetUserId();

                res = _dailyEngagementService.AddDailyEngagement(model, userid);

                string msg = "";

                if (res == 1)
                {
                    msg = "Saved Successfully";
                    _hub.Clients.All.SendAsync("transferchartdata", _notificationService.GetNotifications("*", userid, true));
                }
                else
                {
                    msg = "Updated Successfully";
                }
                return Ok(UtilService.GetResponse<Json>(null, msg));


            }
            catch (Exception ex)
            {
                return BadRequest(UtilService.GetExResponse<Exception>(ex));
            }
        }
        #endregion






        #region GetDailyEngagementListsByFilter
        //[Authorize]
        [HttpPost]
        [Route("GetDailyEngagementListsByFilter")]
        public async Task<IActionResult> GetDailyEngagementListsByFilter(DailyEngagementFilterDTO DailyEngagementFilterDTO)
        {
            try
            {
                ///Get userid
                Guid userid = Guid.Parse(this.GetUserId());

                List<ListDailyEngagementDTO> ContactList = await _dailyEngagementService.GetDailyEngagementListsByFilter(userid, DailyEngagementFilterDTO, await _cService.CheckRoleExists(userid, "Admin"));

                return Ok(UtilService.GetResponse(ContactList));
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


                var ContactList = _dailyEngagementService.ReadJson();

                Json pendencyDTO = JsonConvert.DeserializeObject<Json>(ContactList);


                return Ok(UtilService.GetResponse<Json>(pendencyDTO));
            }
            catch (Exception ex)
            {
                return Ok(UtilService.GetExResponse<Exception>(ex));
            }
        }

        #endregion


        #region DeleteDailyEngagement
        //[Authorize]
        [HttpGet]
        [Route("DeleteDailyEngagement")]
        public IActionResult DeleteDailyEngagement(int Id)
        {
            try
            {

                _dailyEngagementService.DeleteDailyEngagement(Id);

                return Ok(UtilService.GetResponse("Contact Deleted"));
            }
            catch (Exception ex)
            {
                return Ok(UtilService.GetExResponse<Exception>(ex));
            }
        }

        #endregion

        #region GetDailyEngagementPublicList
        [HttpGet]
        [Route("GetDailyEngagementPublicList")]
        public IActionResult GetContactDesignationList()
        {
            try
            {
                return Ok(UtilService.GetResponse<List<DailyEngagementDTO>>(_dailyEngagementService.GetDailyEngagementPublicList()));
            }
            catch (Exception ex)
            {
                return Ok(UtilService.GetExResponse<Exception>(ex));
            }
        }
        #endregion

        #region GetDailyEngagementCount

        [HttpGet]
        [Route("GetDailyEngagementCount")]
        public async Task<IActionResult> GetDailyEngagementCount()
        {
            try
            {
                ///Get userid
                Guid userid = Guid.Parse(this.GetUserId());




                DailyEngagementCount DailyEngagementCount;

                if (await _cService.CheckRoleExists(userid, "Admin"))
                {
                    DailyEngagementCount = _dailyEngagementService.GetDailyEngagementCount(null);
                }
                else
                {
                    DailyEngagementCount = _dailyEngagementService.GetDailyEngagementCount(userid);
                }


                return Ok(UtilService.GetResponse(DailyEngagementCount));
            }
            catch (Exception ex)
            {
                return Ok(UtilService.GetExResponse<Exception>(ex));
            }
        }

        #endregion




        #region GetDailyEngagementListsByStatus
        //[Authorize]
        [HttpGet]
        [Route("GetDailyEngagementListsByStatus")]
        public IActionResult GetDailyEngagementListsByStatus(int status)
        {
            try
            {
                ///Get userid
                Guid userid = Guid.Parse(this.GetUserId());


                List<DailyEngagementDTO> TaskList = _dailyEngagementService.GetDailyEngagementListsByStatus(userid, status);

                return Ok(UtilService.GetResponse(TaskList));
            }
            catch (Exception ex)
            {
                return Ok(UtilService.GetExResponse<Exception>(ex));
            }
        }

        #endregion

        #region GetDailyEngagementById
        //[Authorize]
        [HttpGet]
        [Route("GetDailyEngagementById")]
        public IActionResult GetDailyEngagementById(int taskId)
        {
            try
            {
                ///Get userid
                ///Guid userid = Guid.Parse(this.GetUserId());


                List<DailyEngagementDTO> TaskList = _dailyEngagementService.GetDailyEngagementById(taskId);

                return Ok(UtilService.GetResponse(TaskList));
            }
            catch (Exception ex)
            {
                return Ok(UtilService.GetExResponse<Exception>(ex));
            }
        }

        #endregion
    }
}
