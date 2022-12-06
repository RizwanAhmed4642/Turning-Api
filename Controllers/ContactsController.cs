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
    public class ContactsController : ControllerBase
    {
        #region Fields
        private ContactsService _contactsService;
        private CommonService _cService;
        private NotificationService _notificationService = new NotificationService();

        private IHubContext<NotificationHub> _hub;

        #endregion

        #region Constructor
        public ContactsController(UserManager<Applicationuser> user, ContactsService contactsService, IMapper mapper, IHubContext<NotificationHub> hub)
        {
            _contactsService = contactsService;
            _hub = hub;
            _cService = new CommonService(user);

        }
        #endregion



        #region AddContact
        [HttpPost]
        [Route("AddContact")]
        public async Task<IActionResult> AddContact([FromForm] ContactDTO model)
        {
            try
            {
                int res = 0;

                ///
                string userid = this.GetUserId();
                
                res = await _contactsService.AddContact(model, userid);

                string msg = "";

                if (res == 1)
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
        #region UpdateContact
        [HttpPost]
        [Route("UpdateContact")]
        public async Task<IActionResult> UpdateContact([FromForm] ContactDTO model)
        {
            try
            { 
                string userid = this.GetUserId();
                //string userid = "333C3DEB-F4E0-460F-0A8A-08D9C9F8DBEA";


                var res = await _contactsService.UpdateContact(model, userid);

                string msg = "";

                if (res!= 0)
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

        #region GetAllContacts
        [Authorize]
        [HttpGet]
        [Route("GetAllContacts")]
        public async Task<IActionResult> GetAllContacts()
        {
            try
            {
                Guid userid = Guid.Parse(this.GetUserId());

                List<ListContactDTO> TaskList = _contactsService.GetAllContacts(userid, await _cService.CheckRoleExists(userid, "Admin"));

                return Ok(UtilService.GetResponse(TaskList));
            }
            catch (Exception ex)
            {
                return Ok(UtilService.GetExResponse<Exception>(ex));
            }
        }

        #endregion

 #region GetContactListsByFilter
        //[Authorize]
        [HttpPost]
        [Route("GetContactListsByFilter")]
        public async Task<IActionResult> GetContactListsByFilter(ContactFilterDTO ContactFilterDTO)
        {
            try
            {
                ///Get userid
                Guid userid = Guid.Parse(this.GetUserId());

                List<ListContactDTO> ContactList = await _contactsService.GetContactListsByFilter(userid, ContactFilterDTO, await _cService.CheckRoleExists(userid, "Admin"));

                return Ok(UtilService.GetResponse(ContactList));
            }
            catch (Exception ex)
            {
                return Ok(UtilService.GetExResponse<Exception>(ex));
            }
        }

        #endregion

        #region GetContactListsByAllFilter
        //[Authorize]
        [HttpPost]
        [Route("GetContactListsByAllFilter")]
        public async Task<IActionResult> GetContactListsByAllFilter(ContactAllFilterDTO ContactAllFilterDTO)
        {
            try
            {
                ///Get userid
                Guid userid = Guid.Parse(this.GetUserId());

                List<ListContactDTO> ContactList = await _contactsService.GetContactListsByAllFilter(userid, ContactAllFilterDTO, await _cService.CheckRoleExists(userid, "Admin"));

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


                var ContactList = _contactsService.ReadJson();

                Json pendencyDTO = JsonConvert.DeserializeObject<Json>(ContactList);


                return Ok(UtilService.GetResponse<Json>(pendencyDTO));
            }
            catch (Exception ex)
            {
                return Ok(UtilService.GetExResponse<Exception>(ex));
            }
        }

        #endregion


        #region DeleteContact
        //[Authorize]
        [HttpGet]
        [Route("DeleteContact")]
        public IActionResult DeleteContact(int Id)
        {
            try
            {

                _contactsService.DeleteContact(Id);

                return Ok(UtilService.GetResponse("Contact Deleted"));
            }
            catch (Exception ex)
            {
                return Ok(UtilService.GetExResponse<Exception>(ex));
            }
        }

        #endregion




        #region AddContactType
        [HttpPost]
        [Route("AddContactType")]
        public IActionResult AddContactType([FromForm] ContactTypeDTO model)
        {
            try
            {
                int res = 0;

                ///
                string userid = this.GetUserId();

                res = _contactsService.AddContactType(model, userid);

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

        #region GetContactById
        //[Authorize]
        [HttpGet]
        [Route("GetContactById")]
        public IActionResult GetContactById(int taskId)
        {
            try
            {
                ///Get userid
                ///Guid userid = Guid.Parse(this.GetUserId());


                List<ListContactDTO> TaskList = _contactsService.GetContactById(taskId);

                return Ok(UtilService.GetResponse(TaskList));
            }
            catch (Exception ex)
            {
                return Ok(UtilService.GetExResponse<Exception>(ex));
            }
        }

        #endregion

        //#region GetDetailsContactList
        ////[Authorize]
        //[HttpGet]
        //[Route("GetDetailsContactList")]
        //public IActionResult GetDetailsContactList(int taskid)
        //{
        //    try
        //    {
        //        ///Get userid
        //        // Guid userid = Guid.Parse(this.GetUserId());


        //        List<ListContactDTO> TaskList = _contactsService.GetDetailsContactList(taskid);

        //        return Ok(UtilService.GetResponse(TaskList));
        //    }
        //    catch (Exception ex)
        //    {
        //        return Ok(UtilService.GetExResponse<Exception>(ex));
        //    }
        //}

        //#endregion
    }
}
