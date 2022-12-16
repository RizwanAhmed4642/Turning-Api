using AutoMapper;
using Meeting_App.Data.Database.Context;
using Meeting_App.Data.Database.Tables;
using Meeting_App.Models;
using Meeting_App.Models.DTOs;
using Meeting_App.Service;
using Meeting_App.Service.Common;
using Meeting_App.SIgnalRHub;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
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
    public class EventController : ControllerBase
    {
        #region Fields
        private EventServices _eventServices;
        private CommonService _cService;
        private NotificationService _notificationService = new NotificationService();

        private IHubContext<NotificationHub> _hub;

        #endregion

        #region Constructor
        public EventController(UserManager<Applicationuser> user, EventServices eventServices, IMapper mapper, IHubContext<NotificationHub> hub)
        {
            _eventServices = eventServices;
            _hub = hub;
            _cService = new CommonService(user);

        }
        #endregion

        #region AddEvent
        [HttpPost]
        [Route("AddEvent")]
        public async Task<IActionResult> AddEvent([FromForm] EventCalenderView model)
         {
            try
            {
                string userid = this.GetUserId();
                //string userid = "333C3DEB-F4E0-460F-0A8A-08D9C9F8DBEA";
               // model.EventParticipant = JsonConvert.DeserializeObject<List<EventParticipantView>>(model.EventParticipantData);

                var res = await _eventServices.AddEvent(model, userid);

                string msg = "";

                if (res.Id != 0)
                {
                    msg = "Saved Successfully";
                    await _hub.Clients.All.SendAsync("transferchartdata", _notificationService.GetNotifications("*", userid, true));
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

        [HttpPost]
        [Route("UpdateEvent")]
        public async Task<IActionResult> UpdateEvent([FromForm] EventCalenderView model)
        {
            try
            {
                string userid = this.GetUserId();
                //string userid = "333C3DEB-F4E0-460F-0A8A-08D9C9F8DBEA";

                model.EventParticipant = JsonConvert.DeserializeObject<List<EventParticipantView>>(model.EventParticipantData);

                var res = await _eventServices.Update(model, userid);

                string msg = "";

                if (res.Id != 0)
                {
                    msg = "Saved Successfully";
                    await _hub.Clients.All.SendAsync("transferchartdata", _notificationService.GetNotifications("*", userid, true));
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




        #region GetTrainings
        [Authorize]
        [HttpGet]
        [Route("GetTrainings")]
        public async Task<IActionResult> GetTrainings()
        {
            try
            {
                Guid userid = Guid.Parse(this.GetUserId());
                var isAdmin = await _cService.CheckRoleExists(userid, "Admin");
              
                var list = _eventServices.GetTrainings();

                return Ok(UtilService.GetResponse(list));
            }
            catch (Exception ex)
            {
                return Ok(UtilService.GetExResponse<Exception>(ex));
            }
        }
        #endregion

        #region GetAllEvents
        [Authorize]
        [HttpPost]
        [Route("GetAllEvents")]
        public async Task<IActionResult> GetAllEvents([FromBody] EventFilter filter)
        {
            try
            {
                Guid userid = Guid.Parse(this.GetUserId());
                var isAdmin = await _cService.CheckRoleExists(userid, "Admin");
                if (!isAdmin)
                {
                    filter.UserId = userid;
                }
                if (filter.ShowMyEvent)
                {
                    filter.UserId = userid;
                }
                var list = _eventServices.GetEvents(filter);

                return Ok(UtilService.GetResponse(list));
            }
            catch (Exception ex)
            {
                return Ok(UtilService.GetExResponse<Exception>(ex));
            }
        }

        #endregion


        #region Venue
        [HttpPost]
        [Route("AddVenue")]
        public IActionResult AddVenue([FromForm] MeetingVenueDTO model)
        {
            try
            {
                int res = 0;

                ///
                string userid = this.GetUserId();

                res = _eventServices.AddVenue(model, userid);

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

        [Authorize]
        [HttpGet]
        [Route("GetVenues")]
        public async Task<IActionResult> GetVenues()
        {
            try
            {
                Guid userid = Guid.Parse(this.GetUserId());
                var isAdmin = await _cService.CheckRoleExists(userid, "Admin");

                var list = _eventServices.GetVenue();

                return Ok(UtilService.GetResponse(list));
            }
            catch (Exception ex)
            {
                return Ok(UtilService.GetExResponse<Exception>(ex));
            }
        }



            #endregion

            #region TariningCategory
            [HttpPost]
        [Route("AddTariningCategory")]
        public IActionResult AddTariningCategory( TrainingCategoryDTO model)
        {
            try
            {
                int res = 0;

                ///
                string userid = this.GetUserId();

                res = _eventServices.AddTraingCategory(model, userid);

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


        [HttpGet]
        [Route("GetTariningCategory")]
        public IActionResult GetTariningCategory()
        {
            try
            {
                return Ok(UtilService.GetResponse<List<tbl_TrainingCategory>>(_eventServices.GetTrainingCategory()));
            }
            catch (Exception ex)
            {
                return Ok(UtilService.GetExResponse<Exception>(ex));
            }
        }
        #endregion

        #region TariningType
        [HttpPost]
        [Route("AddTariningType")]
        public IActionResult AddTariningType(TrainingTypeDTO model)
        {
            try
            {
                int res = 0;

                ///
                string userid = this.GetUserId();

                res = _eventServices.AddTrainingType(model, userid);

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


        [HttpGet]
        [Route("GetTrainingType")]
        public IActionResult GetTrainingType()
        {
            try
            {
                return Ok(UtilService.GetResponse<List<tbl_TraingType>>(_eventServices.GetTrainingType()));
            }
            catch (Exception ex)
            {
                return Ok(UtilService.GetExResponse<Exception>(ex));
            }
        }
        #endregion















        #region AddOrganizer
        [HttpPost]
        [Route("AddOrganizer")]
        public IActionResult AddOrganizer([FromForm] MeetingOrganizerDTO model)
        {
            try
            {
                int res = 0;

                ///
                string userid = this.GetUserId();

                res = _eventServices.AddOrganizer(model, userid);

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


        #region GetMeetingOrganizerList
        [HttpGet]
        [Route("GetMeetingOrganizerList")]
        public IActionResult GetMeetingOrganizerList()
        {
            try
            {
                return Ok(UtilService.GetResponse<List<tbl_MeetingOrganizer>>(_eventServices.GetMeetingOrganizerList()));
            }
            catch (Exception ex)
            {
                return Ok(UtilService.GetExResponse<Exception>(ex));
            }
        }

        #endregion



        #region GetMeetingVenueList
        [HttpGet]
        [Route("GetMeetingVenueList")]
        public IActionResult GetMeetingVenueList()
        {
            try
            {
                return Ok(UtilService.GetResponse<List<tbl_MeetingVenue>>(_eventServices.GetMeetingVenueList()));
            }
            catch (Exception ex)
            {
                return Ok(UtilService.GetExResponse<Exception>(ex));
            }
        }
        #endregion

        #region GetMeetingStatus
        [HttpGet]
        [Route("GetMeetingStatus")]
        public IActionResult GetMeetingStatus()
        {
            try
            {
                return Ok(UtilService.GetResponse<List<DDLMeetingStatusModel>>(_eventServices.GetMeetingStatus()));
            }
            catch (Exception ex)
            {
                return Ok(UtilService.GetExResponse<Exception>(ex));
            }
        }
        #endregion



        #region GetMeetingListsByStatus
        //[Authorize]
        [HttpGet]
        [Route("GetMeetingListsByStatus")]
        public async Task<IActionResult> GetMeetingListsByStatus(int status)
        {
            try
            {
                ///Get userid
                Guid userid = Guid.Parse(this.GetUserId());

                List<EventCalenderView> TaskList = _eventServices.GetMeetingListsByStatus(userid, status);


                if (await _cService.CheckRoleExists(userid, "Admin"))
                {
                    TaskList = _eventServices.GetMeetingListsByStatus(null, status);
                }
                else
                {
                    TaskList = _eventServices.GetMeetingListsByStatus(userid, status);
                }





                return Ok(UtilService.GetResponse(TaskList));
            }
            catch (Exception ex)
            {
                return Ok(UtilService.GetExResponse<Exception>(ex));
            }
        }

        #endregion



        #region GetMeetingCount

        [HttpGet]
        [Route("GetMeetingCount")]
        public async Task<IActionResult> GetMeetingCount()
        {
            try
            {
                ///Get userid
                Guid userid = Guid.Parse(this.GetUserId());




                MeetingCount MeetingCount;

                if (await _cService.CheckRoleExists(userid, "Admin"))
                {
                    MeetingCount = _eventServices.GetMeetingCount(null);
                }
                else
                {
                    MeetingCount = _eventServices.GetMeetingCount(userid);
                }


                return Ok(UtilService.GetResponse(MeetingCount));
            }
            catch (Exception ex)
            {
                return Ok(UtilService.GetExResponse<Exception>(ex));
            }
        }

        #endregion





        #region UpdateMeetingStatus
        [HttpPost]
        [Route("UpdateMeetingStatus")]
        public IActionResult UpdateMeetingStatus([FromForm] MeetingDetailDTO model)
        {
            try
            {
                int res = 0;


                string userid = this.GetUserId();
                //string userid = "4474BD83-AF36-42F8-3BB1-08D9CAAF2517";


                res = _eventServices.UpdateMeetingStatus(model, userid);

                string msg = "";

                if (res == 1)
                {
                    msg = "Saved Successfully";
                }
                else
                {
                    msg = "Meeting Id Should not be 0 or empty";
                }

                return Ok(UtilService.GetResponse<Json>(null, msg));


            }
            catch (Exception ex)
            {
                return BadRequest(UtilService.GetExResponse<Exception>(ex));
            }
        }
        #endregion



        #region DeleteMeeting
        //[Authorize]
        [HttpGet]
        [Route("DeleteMeeting")]
        public IActionResult DeleteMeeting(int Id)
        {
            try
            {

                _eventServices.DeleteMeeting(Id);

                return Ok(UtilService.GetResponse("Meeting Deleted"));
            }
            catch (Exception ex)
            {
                return Ok(UtilService.GetExResponse<Exception>(ex));
            }
        }

        #endregion

    }




}
