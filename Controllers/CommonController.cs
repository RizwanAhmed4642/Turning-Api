using Meeting_App.Data.Database.Context;
using Meeting_App.Data.Database.Tables;
using Meeting_App.Models;
using Meeting_App.Models.DTOs;
using Meeting_App.Service.Common;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Meeting_App.Service.Common.CommonService;

namespace Meeting_App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommonController : Controller
    {
        private CommonService _commonServices;
        private readonly UserManager<Applicationuser> userManager;
        public CommonController(UserManager<Applicationuser> user, UserManager<Applicationuser> userManager)
        {
            this.userManager = userManager;
            _commonServices = new CommonService(user);

        }

        [HttpGet]
        [Route("AppVersion")]
        public IActionResult AppVersion()
        {
            try
            {
                var app = _commonServices.AppVersion();

                return Ok(new { err = "N", res = app });
 
            }
            catch (Exception ex)
            {
                return Ok(UtilService.GetExResponse<Exception>(ex));
            }
        }
        //Startup New Work
        #region Departments
        [HttpGet]
        [Route("GetDepartments")]
        public IActionResult GetDepartments()
        {
            try
            {
                return Ok(UtilService.GetResponse<List<DepartmentsDto>>(_commonServices.GetDepartmentList()));
            }
            catch (Exception ex)
            {
                return Ok(UtilService.GetExResponse<Exception>(ex));
            }
        }


        #endregion



        #region AssignedList
        [HttpGet]
        [Route("AssignedToList")]
        public IActionResult AssignedToList()
        {
            try
            {
                string userid = this.GetUserId();


                return Ok(UtilService.GetResponse<List<DDLModel>>(_commonServices.GetAssignedToList(userid)));
            }
            catch (Exception ex)
            {
                return Ok(UtilService.GetExResponse<Exception>(ex));
            }
        }
        [HttpGet]
        [Route("ParticepentList")]
        public IActionResult ParticepentList()
        {
            try
            {
                string userid = this.GetUserId();


                return Ok(UtilService.GetResponse<List<EventParticipantView>>(_commonServices.GetParticepentList(userid)));
            }
            catch (Exception ex)
            {
                return Ok(UtilService.GetExResponse<Exception>(ex));
            }
        }
        #endregion


        #region TaskStatus
        [HttpGet]
        [Route("TaskStatus")]
        public IActionResult TaskStatus()
        {
            try
            {
                return Ok(UtilService.GetResponse<List<DDLStatusModel>>(_commonServices.GetTaskStatus()));
            }
            catch (Exception ex)
            {
                return Ok(UtilService.GetExResponse<Exception>(ex));
            }
        }
        #endregion

        #region DivisionList
        [HttpGet]
        [Route("GetDivisionList")]
        public IActionResult GetDivisionList(string geoLevel)
        {
            try
            {
                return Ok(UtilService.GetResponse<List<DDLModel>>(_commonServices.GetDivision(geoLevel)));
            }
            catch (Exception ex)
            {
                return Ok(UtilService.GetExResponse<Exception>(ex));
            }
        }
        #endregion

        #region DistrictList
        [HttpGet]
        [Route("GetDistrictList")]
        public IActionResult GetDistrictList(string geoLevel)
        {
            try
            {
                return Ok(UtilService.GetResponse<List<DDLModel>>(_commonServices.GetDistrict(geoLevel)));
            }
            catch (Exception ex)
            {
                return Ok(UtilService.GetExResponse<Exception>(ex));
            }

        }
        #endregion

        #region TehsilList
        [HttpGet]
        [Route("GetTehsilList")]
        public IActionResult GetTehsilList(string geoLevel)
        {
            try
            {
                return Ok(UtilService.GetResponse<List<DDLModel>>(_commonServices.GetTehsil(geoLevel)));
            }
            catch (Exception ex)
            {
                return Ok(UtilService.GetExResponse<Exception>(ex));
            }

        }
        #endregion


        //Startup New Work
        #region GetContactCompanyList
        [HttpGet]
        [Route("GetContactCompanyList")]
        public IActionResult GetContactCompanyList()
        {
            try
            {
                return Ok(UtilService.GetResponse<List<CommonClass>>(_commonServices.GetContactCompanyList()));
            }
            catch (Exception ex)
            {
                return Ok(UtilService.GetExResponse<Exception>(ex));
            }
        }


        #endregion

        #region GetContactCompanyListSPL
        [HttpGet]
        [Route("GetContactCompanyListSPL")]
        public IActionResult GetContactCompanyListSPL()
        {
            try
            {
                return Ok(UtilService.GetResponse<List<CommonClass>>(_commonServices.GetContactCompanyListSPL()));
            }
            catch (Exception ex)
            {
                return Ok(UtilService.GetExResponse<Exception>(ex));
            }
        }


        #endregion
        //-----------------Multi Dropdown --------------
        #region GetContactMultiDepartmentList
        [HttpPost]
        [Route("GetContactMultiDepartmentList")]
        public IActionResult GetContactMultiDepartmentList(ContactMultiFilterDTO ContactMultiFilterDTO)
        {
            try
            {
                return Ok(UtilService.GetResponse<List<ContactDepartment>>(_commonServices.GetContactMultiDepartmentList(ContactMultiFilterDTO)));
            }
            catch (Exception ex)
            {
                return Ok(UtilService.GetExResponse<Exception>(ex));
            }
        }
        #endregion
        //-------------------SPL-----------------------
        #region GetContactMultiDepartmentListSPL
        [HttpPost]
        [Route("GetContactMultiDepartmentListSPL")]
        public IActionResult GetContactMultiDepartmentListSPL(ContactMultiFilterDTO ContactMultiFilterDTO)
        {
            try
            {
                return Ok(UtilService.GetResponse<List<ContactDepartment>>(_commonServices.GetContactMultiDepartmentListSPL(ContactMultiFilterDTO)));
            }
            catch (Exception ex)
            {
                return Ok(UtilService.GetExResponse<Exception>(ex));
            }
        }
        #endregion

        //-------------------SPL-----------------------



        #region GetContactMultiCategoryList
        [HttpPost]
        [Route("GetContactMultiCategoryList")]
        public IActionResult GetContactMultiCategoryList(ContactMultiFilterDTO ContactMultiFilterDTO)
        {
            try
            {
                return Ok(UtilService.GetResponse<List<tbl_ContactCategory>>(_commonServices.GetContactMultiCategoryList(ContactMultiFilterDTO)));
            }
            catch (Exception ex)
            {
                return Ok(UtilService.GetExResponse<Exception>(ex));
            }
        }
        #endregion

        //-----------------Multi Dropdown --------------


        #region GetContactDepartmentList
        [HttpGet]
        [Route("GetContactDepartmentList")]
        public IActionResult GetContactDepartmentList(int? Id)
        //public IActionResult GetContactDepartmentList(int? Id, string Type = "Single", List<ContactsFilterDTO> Data = null)
        {
            try
            {
                return Ok(UtilService.GetResponse<List<CommonClass>>(_commonServices.GetContactDepartmentList(Id)));
                //return Ok(UtilService.GetResponse<List<CommonClass>>(_commonServices.GetContactDepartmentList(Id,"",null)));
            }
            catch (Exception ex)
            {
                return Ok(UtilService.GetExResponse<Exception>(ex));
            }
        }
        #endregion


        //End NEw Work
        #region GetContactDesignationList
        [HttpGet]
        [Route("GetContactDesignationList")]
        public IActionResult GetContactDesignationList(int? Id)
        {
            try
            {
                return Ok(UtilService.GetResponse<List<CommonClass>>(_commonServices.GetContactDesignation(Id)));
            }
            catch (Exception ex)
            {
                return Ok(UtilService.GetExResponse<Exception>(ex));
            }
        }
        #endregion


        #region GetContactCategoryList
        [HttpGet]
        [Route("GetContactCategoryList")]
        public IActionResult GetContactCategoryList(int? Id)
        {
            try
            {
                return Ok(UtilService.GetResponse<List<CommonClass>>(_commonServices.GetContactCategoryList(Id)));
            }
            catch (Exception ex)
            {
                return Ok(UtilService.GetExResponse<Exception>(ex));
            }
        }
        #endregion







        #region GetContactListByDesignation
        [HttpGet]
        [Route("GetContactListByDesignation")]
        public IActionResult GetContactListByDesignation(int? Id)
        {
            try
            {
                return Ok(UtilService.GetResponse<List<ListContactDTO>>(_commonServices.GetContactListByDesignation(Id)));
            }
            catch (Exception ex)
            {
                return Ok(UtilService.GetExResponse<Exception>(ex));
            }
        }



        #endregion

        #region GetContactAllDesignationList
        [HttpGet]
        [Route("GetContactAllDesignationList")]
        public IActionResult GetContactAllDesignationList()
        {
            try
            {
                return Ok(UtilService.GetResponse<List<tbl_ContactDesignation>>(_commonServices.GetContactAllDesignationList()));
            }
            catch (Exception ex)
            {
                return Ok(UtilService.GetExResponse<Exception>(ex));
            }
        }
        #endregion

        #region GetContactListByFilter
        [HttpPost]
        [Route("GetContactListByFilter")]
        public IActionResult GetContactListByFilter(ContactsFilterDTO filterDTO )
        {
            try
            {
                return Ok(UtilService.GetResponse<List<ListContactDTO>>(_commonServices.GetContactListByFilter(filterDTO)));
            }
            catch (Exception ex)
            {
                return Ok(UtilService.GetExResponse<Exception>(ex));
            }
        }



        #endregion
        #region GetContactsDesignationList
        [HttpPost]
        [Route("GetContactsDesignationList")]
        public IActionResult GetContactsDesignationList(ContactMultiFilterDTO ContactMultiFilterDTO)
        {
            try
            {
                return Ok(UtilService.GetResponse<List<ContactsDetailView>>(_commonServices.GetContactsDesignationList(ContactMultiFilterDTO)));
            }
            catch (Exception ex)
            {
                return Ok(UtilService.GetExResponse<Exception>(ex));
            }
        }
        #endregion

        #region GetContactWithTiers
        [HttpPost]
        [Route("GetContactWithTiers")]
        public IActionResult GetContactWithTiers(ContactsFilterDTO ContactkFilterDTO)
        {
            try
            {
                return Ok(UtilService.GetResponse<dynamic>(_commonServices.GetContactWithTiers(ContactkFilterDTO)));
            }
            catch (Exception ex)
            {
                return Ok(UtilService.GetExResponse<Exception>(ex));
            }
        }
        #endregion
        #region GetContactWithMultiTiers
        [HttpPost]
        [Route("GetContactWithMultiTiers")]
        public IActionResult GetContactWithMultiTiers(ContactMultiListDTO ContactkFilterDTO)
        {
            try
            {
                return Ok(UtilService.GetResponse<dynamic>(_commonServices.GetContactWithMultiTiers(ContactkFilterDTO)));
            }
            catch (Exception ex)
            {
                return Ok(UtilService.GetExResponse<Exception>(ex));
            }
        }
        #endregion
        //[HttpGet]
        //[Route("CreateLogins")]
        //public async Task<IActionResult> MyRegisterAsync()
        //{
        //    string messages = string.Empty;
        //    using (var db = new IDDbContext())
        //    {

        //        var towns = db.tbl_Contacts.Where(x =>x.RecordStatus==true).ToList();
        //        foreach (var t in towns)
        //        {
        //            var check = db.AspNetUsers.FirstOrDefault(x => x.ContactId == t.Id);
        //            if (check == null)
        //            {
        //                var user = new Applicationuser
        //                {
        //                    PhoneNumber = t.MobileNo,
        //                    Email = t.Email,
        //                    SecurityStamp = Guid.NewGuid().ToString(),
        //                    UserName = t.MobileNo,
        //                    FullName = t.Name,
        //                    Designation = t.Department,
        //                    NP = t.MobileNo + "@Mms",
        //                    ContactId = t.Id,
        //                    IsRecordStatus = true,
        //                    IsDeleted = false,
        //                };





        //                var result = await userManager.CreateAsync(user, t.MobileNo + "@Mms");

        //                if (result.Succeeded)
        //                {


        //                    //userManager.AssignUserRoles((user.Id, "Dashboard Button");
        //                    //userManager.AssignUserRoles(user.Id, "Dashboard Button");
        //                    //userManager.AssignUserRoles(user.Id, "Dashboard Button");





        //                }
        //            }
        //        }
        //    }

        //    return Ok("sdjakdas");
        //}


        #region GetContactMultiListByDesignation
        [HttpPost]
        [Route("GetContactMultiListByDesignation")]
        public IActionResult GetContactMultiListByDesignation(List<ContactsFilterDTO> ContactkFilterDTO)
        {
            try
            {
                return Ok(UtilService.GetResponse<List<ListContactDTO>>(_commonServices.GetContactMultiListByDesignation(ContactkFilterDTO)));
            }
            catch (Exception ex)
            {
                return Ok(UtilService.GetExResponse<Exception>(ex));
            }
        }

        #endregion



    }
}
