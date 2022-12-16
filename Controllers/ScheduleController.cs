using AutoMapper;
using Meeting_App.Models;
using Meeting_App.Service.Common;
using Meeting_App.Service;
using Meeting_App.SIgnalRHub;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using System;
using Meeting_App.Data.Database.Context;
using System.Linq;

namespace Meeting_App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ScheduleController : ControllerBase
    {
        #region Fields
        private EventServices _eventServices;
        private ScheduleServices _scheduleServices;
        private CommonService _cService;
        private NotificationService _notificationService = new NotificationService();

        private IHubContext<NotificationHub> _hub;

        #endregion

        #region Constructor

        //private readonly IDDbContext db;
        public ScheduleController(UserManager<Applicationuser> user, EventServices eventServices, IMapper mapper, IHubContext<NotificationHub> hub)
        {
            _eventServices = eventServices;
            _hub = hub;
            _cService = new CommonService(user);
            //db = context;
        }
        #endregion
        #region Designation
        [Authorize]
        [HttpGet]
        [Route("GetDesignation")]
        public async Task<IActionResult> GetDesignation()
        {
            try
            {
                Guid userid = Guid.Parse(this.GetUserId());
                var isAdmin = await _cService.CheckRoleExists(userid, "Admin");

                var list = _scheduleServices.GetDesignation();

                return Ok(UtilService.GetResponse(list));
            }
            catch (Exception ex)
            {
                return Ok(UtilService.GetExResponse<Exception>(ex));
            }
        }
        #endregion

        #region ProfileFilter
        [HttpGet]
        [Route("GetDivisions")]
        public IActionResult GetDivisions()
        {
            using (var db = new IDDbContext())
            {
                using (var trans = db.Database.BeginTransaction())
                {
                    return Ok(UtilService.GetResponse(db.Divisions.ToList()));
                }
            }
        }
        [HttpGet]
        [Route("GetDistricts/{code}")]
        public IActionResult GetDistricts(string code)
        {
            using (var db = new IDDbContext())
            {
                using (var trans = db.Database.BeginTransaction())
                {
                    return Ok(UtilService.GetResponse((db.District.Where(x => x.Code.StartsWith(code)).ToList())));
                }
            }
        }

        [HttpGet]
        [Route("GetTehsils/{code}")]
        public IActionResult GetTehsils(string code)
        {
            using (var db = new IDDbContext())
            {
                using (var trans = db.Database.BeginTransaction())
                {
                    return Ok(UtilService.GetResponse((db.Tehsils.Where(x => x.Code.StartsWith(code)).ToList())));
                }
            }
        }
        [HttpGet]
        [Route("GetHfTypes")]
        public IActionResult GetHfTypes()
        {
            using (var db = new IDDbContext())
            {
                using (var trans = db.Database.BeginTransaction())
                {
                    return Ok(UtilService.GetResponse((db.Hftype.Where(x => x.Code == "011" || x.Code == "012" || x.Code == "013" || x.Code == "014").ToList())));
                }
            }
        }

        [HttpGet]
        [Route("GetServiceListFacility/{disCode}/{HfTypeCode}/{bhuName}")]
        public IActionResult GetServiceListFacility(string disCode, string HfTypeCode, string bhuName)
        {
            using (var db = new IDDbContext())
            {
                using (var trans = db.Database.BeginTransaction())
                {
                    if (bhuName == "null")
                    {
                        return Ok(UtilService.GetResponse((db.HflistWithMode.Where(x => x.HFMISCode.StartsWith(disCode) && HfTypeCode == x.HFTypeCode).ToList())));
                    }
                    return Ok(UtilService.GetResponse((db.HflistWithMode.Where(x => x.HFMISCode.StartsWith(disCode) && HfTypeCode == x.HFTypeCode && x.ModeName.Contains(bhuName)).ToList())));
                }
            }

        }

        [HttpGet]
        [Route("GetBhuList")]
        public IActionResult GetBhuList()
        {
            using (var db = new IDDbContext())
            {
                using (var trans = db.Database.BeginTransaction())
                {
                    return Ok(UtilService.GetResponse((db.Hfmode.GroupBy(n => new { n.ModeName }).Select(g => new
                    {
                        Name = g.Key.ModeName
                    })).ToList()));
                }
            }
        }
        #endregion
    }
}
