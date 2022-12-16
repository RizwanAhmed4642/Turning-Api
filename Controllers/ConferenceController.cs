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
    public class ConferenceController : ControllerBase
    {
        #region Fields
        private ConferenceServices _conferenceServices;
        private CommonService _cService;
        private NotificationService _notificationService = new NotificationService();

        private IHubContext<NotificationHub> _hub;

        #endregion

        #region Constructor
        public ConferenceController(UserManager<Applicationuser> user, ConferenceServices ConferenceServices, IMapper mapper, IHubContext<NotificationHub> hub)
        {
            _conferenceServices = ConferenceServices;
            _hub = hub;
            _cService = new CommonService(user);

        }
        #endregion

        #region AddConference
        [HttpPost]
        [Route("AddConference")]
        public async Task<IActionResult> AddConference([FromForm] ConferenceViewDTO model)
        {
            try
            {
                string userid = this.GetUserId();
                //string userid = "333C3DEB-F4E0-460F-0A8A-08D9C9F8DBEA";
                model.ConferenceParticipant = JsonConvert.DeserializeObject<List<ConferenceParticipantView>>(model.ConferenceParticipantData);

                var res = await _conferenceServices.AddConference(model, userid);

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
        [Route("UpdateConference")]
        public async Task<IActionResult> UpdateConference([FromForm] ConferenceViewDTO model)
        {
            try
            {
                string userid = this.GetUserId();
                //string userid = "333C3DEB-F4E0-460F-0A8A-08D9C9F8DBEA";

                model.ConferenceParticipant = JsonConvert.DeserializeObject<List<ConferenceParticipantView>>(model.ConferenceParticipantData);

                var res = await _conferenceServices.UpdateConference(model, userid);

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



        #region GetAllConferences
        [Authorize]
        [HttpPost]
        [Route("GetAllConferences")]
        public async Task<IActionResult> GetAllConferences([FromBody] ConferenceFilter filter)
        {
            try
            {
                Guid userid = Guid.Parse(this.GetUserId());
                var isAdmin = await _cService.CheckRoleExists(userid, "Admin");
                if (!isAdmin)
                {
                    filter.UserId = userid;
                }
                if (filter.ShowMyConference)
                {
                    filter.UserId = userid;
                }
                var list = _conferenceServices.GetAllConferences(filter);

                return Ok(UtilService.GetResponse(list));
            }
            catch (Exception ex)
            {
                return Ok(UtilService.GetExResponse<Exception>(ex));
            }
        }
        [Authorize]
        [HttpGet]
        [Route("GetConferencesList")]
        public async Task<IActionResult> GetConferencesList()
        {
            try
            {
                var list = _conferenceServices.GetConferencesList();

                return Ok(UtilService.GetResponse(list));
            }
            catch (Exception ex)
            {
                return Ok(UtilService.GetExResponse<Exception>(ex));
            }
        }

        #endregion


        #region AddVenue
        [HttpPost]
        [Route("AddVenue")]
        public IActionResult AddVenue([FromForm] ConferenceVenueDTO model)
        {
            try
            {
                int res = 0;

                ///
                string userid = this.GetUserId();

                res = _conferenceServices.AddVenue(model, userid);

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

        #region AddOrganizer
        [HttpPost]
        [Route("AddOrganizer")]
        public IActionResult AddOrganizer([FromForm] ConferenceOrganizerDTO model)
        {
            try
            {
                int res = 0;

                ///
                string userid = this.GetUserId();

                res = _conferenceServices.AddOrganizer(model, userid);

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


        #region GetConferenceOrganizerList
        [HttpGet]
        [Route("GetConferenceOrganizerList")]
        public IActionResult GetConferenceOrganizerList()
        {
            try
            {
                return Ok(UtilService.GetResponse<List<tbl_MeetingOrganizer>>(_conferenceServices.GetConferenceOrganizerList()));
            }
            catch (Exception ex)
            {
                return Ok(UtilService.GetExResponse<Exception>(ex));
            }
        }
        #endregion



        #region GetConferenceVenueList
        [HttpGet]
        [Route("GetConferenceVenueList")]
        public IActionResult GetConferenceVenueList()
        {
            try
            {
                return Ok(UtilService.GetResponse<List<tbl_MeetingVenue>>(_conferenceServices.GetConferenceVenueList()));
            }
            catch (Exception ex)
            {
                return Ok(UtilService.GetExResponse<Exception>(ex));
            }
        }
        #endregion

        #region GetConferenceStatus
        [HttpGet]
        [Route("GetConferenceStatus")]
        public IActionResult GetConferenceStatus()
        {
            try
            {
                return Ok(UtilService.GetResponse<List<DDLConferenceStatusModel>>(_conferenceServices.GetConferenceStatus()));
            }
            catch (Exception ex)
            {
                return Ok(UtilService.GetExResponse<Exception>(ex));
            }
        }
        #endregion



        #region GetConferenceListsByStatus
        //[Authorize]
        [HttpGet]
        [Route("GetConferenceListsByStatus")]
        public IActionResult GetConferenceListsByStatus(int status)
        {
            try
            {
                ///Get userid
                Guid userid = Guid.Parse(this.GetUserId());


                List<ConferenceViewDTO> TaskList = _conferenceServices.GetConferenceListsByStatus(userid, status);

                return Ok(UtilService.GetResponse(TaskList));
            }
            catch (Exception ex)
            {
                return Ok(UtilService.GetExResponse<Exception>(ex));
            }
        }

        #endregion



        #region GetConferenceCount

        [HttpGet]
        [Route("GetConferenceCount")]
        public async Task<IActionResult> GetConferenceCount()
        {
            try
            {
                ///Get userid
                Guid userid = Guid.Parse(this.GetUserId());




                ConferenceCount ConferenceCount;

                if (await _cService.CheckRoleExists(userid, "Admin"))
                {
                    ConferenceCount = _conferenceServices.GetConferenceCount(null);
                }
                else
                {
                    ConferenceCount = _conferenceServices.GetConferenceCount(userid);
                }


                return Ok(UtilService.GetResponse(ConferenceCount));
            }
            catch (Exception ex)
            {
                return Ok(UtilService.GetExResponse<Exception>(ex));
            }
        }

        #endregion





        #region UpdateConferenceStatus
        [HttpPost]
        [Route("UpdateConferenceStatus")]
        public IActionResult UpdateConferenceStatus([FromForm] ConferenceDetailDTO model)
        {
            try
            {
                int res = 0;


                string userid = this.GetUserId();
                //string userid = "4474BD83-AF36-42F8-3BB1-08D9CAAF2517";


                res = _conferenceServices.UpdateConferenceStatus(model, userid);

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



        #region DeleteConference
        //[Authorize]
        [HttpGet]
        [Route("DeleteConference")]
        public IActionResult DeleteConference(int Id)
        {
            try
            {

                _conferenceServices.DeleteConference(Id);

                return Ok(UtilService.GetResponse("Conference Deleted"));
            }
            catch (Exception ex)
            {
                return Ok(UtilService.GetExResponse<Exception>(ex));
            }
        }

        #endregion


        [HttpGet]
        [Route("DeleteConferenceParticipant")]
        public IActionResult DeleteConferenceParticipant(string ParticipantId, int ConferenceId)
        {
            try
            {

                _conferenceServices.DeleteConferenceParticipant(ParticipantId, ConferenceId);

                return Ok(UtilService.GetResponse("Conference Deleted"));
            }
            catch (Exception ex)
            {
                return Ok(UtilService.GetExResponse<Exception>(ex));
            }
        }


        [HttpGet]
        [Route("GetConferenceParticipants/{ConferenceId}")]
        public IActionResult GetConferenceParticipants(int ConferenceId)
        {
            try
            {
                var res = _conferenceServices.GetConferenceParticipants(ConferenceId);

                return Ok(UtilService.GetResponse(res));
            }
            catch (Exception ex)
            {
                return Ok(UtilService.GetExResponse<Exception>(ex));
            }
        }

        [HttpGet]
        [Route("GetConferenceParticipantList/{ConferenceId}")]
        public IActionResult GetConferenceParticipantList(int ConferenceId)
        {
            try
            {
                string userid = this.GetUserId();
                var res = _conferenceServices.GetConferenceParticipantList(ConferenceId, userid);

                return Ok(UtilService.GetResponse(res));
            }
            catch (Exception ex)
            {
                return Ok(UtilService.GetExResponse<Exception>(ex));
            }
        }

        //[HttpPost]
        //[Route("GetConferenceParticipantAttendance")]
        //public IActionResult GetConferenceParticipantAttendance(FingerPrintDTO fp)
        //{
        //    try
        //    {
        //        string userid = this.GetUserId();

        //        using (var db = new IDDbContext())
        //        {
        //            using (var trans = db.Database.BeginTransaction())
        //            {
        //                try
        //                {
        //                    if (fp != null)
        //                    {
        //                        var userFPs = db.tbl_UserFP.ToList();
        //                        Fmd fmdCompare = _cService.GetFmd(fp.FPString);
        //                        foreach (var userFp in userFPs)
        //                        {
        //                            var fmd = Fmd.DeserializeXml(userFp.FP);
        //                            if (_cService.compare(fmdCompare, fmd))
        //                            {
        //                                return Ok(UtilService.GetResponse(_conferenceServices.GetConferenceParticipantAttendance(fp, userFp, userid)));
        //                            }
        //                        }
        //                        var fpExist = userFPs.FirstOrDefault(x => x.UserId.Equals(fp.ParticipantId));
        //                        if(fpExist != null)
        //                        {
        //                            return Ok(UtilService.GetExResponse<Exception>("Finger Impression Mismatched!"));
        //                        }
        //                        tbl_UserFP tbl_UserFP = new tbl_UserFP();
        //                        tbl_UserFP.UserId = fp.ParticipantId;
        //                        tbl_UserFP.FP = Fmd.SerializeXml(fmdCompare);
        //                        tbl_UserFP.Datetime = DateTime.UtcNow.AddHours(5);
        //                        tbl_UserFP.CreatedByUserId = userid;
        //                        tbl_UserFP.IsActive = true;
        //                        db.tbl_UserFP.Add(tbl_UserFP);
        //                        db.SaveChanges();
        //                        trans.Commit();
        //                        return Ok(UtilService.GetResponse(_conferenceServices.GetConferenceParticipantAttendance(fp, tbl_UserFP, userid)));
        //                    }
        //                    return Ok(UtilService.GetResponse(""));
        //                }
        //                catch (Exception ex)
        //                {
        //                    trans.Rollback();
        //                    throw;
        //                }
        //            }

        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return Ok(UtilService.GetExResponse<Exception>(ex));
        //    }
        //}

    }




}

