using Meeting_App.Models;
using Meeting_App.Service;
using Meeting_App.Service.Common;
using Meeting_App.SIgnalRHub;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Meeting_App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class NotificationController : ControllerBase
    {
        #region Fields
        private NotificationService _notificationService;
   
        #endregion

        #region Constructor
        public NotificationController()
        {
            _notificationService = new NotificationService();

        }
        #endregion

        #region GetNotificationsList
        [HttpGet]
        [Route("GetNotificationsList")]
        public IActionResult GetNotificationsList()
        {
            try
            {
                
                List<NotificationModel> NotificationList = _notificationService.GetNotifications("*",this.GetUserId(),true);

                return Ok(UtilService.GetResponse(NotificationList));
            }
            catch (Exception ex)
            {
                return Ok(UtilService.GetExResponse<Exception>(ex));
            }
        }

        #endregion
    }
}
