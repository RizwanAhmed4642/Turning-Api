using AutoMapper;
using Meeting_App.Data.Database.Context;
using Meeting_App.Data.Database.Tables;
using Meeting_App.Models;
using Meeting_App.Models.DTOs;
using Meeting_App.Service.Common;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Meeting_App.Service
{
    public class ContactsService
    {
        #region Fields
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _env;
        private readonly NotificationService _notificationService = new NotificationService();
        private readonly UserManager<Applicationuser> userManager;
        #endregion

        #region Constructors
        public ContactsService(IMapper mapper, IWebHostEnvironment env, UserManager<Applicationuser> userManager)
        {
            this.userManager = userManager;
            _mapper = mapper;
            _env = env;
        }
        #endregion

        #region AddContact
        public async Task<int> AddContact(ContactDTO model, string userid)
        {

            using (var db = new IDDbContext())
            {
                using (var trans = db.Database.BeginTransaction())
                {
                    try
                    {
                        string userDesignationName = "";
                        string userProfilePic = "";
                        //var userDesignation = db.AspNetUsers.Where(x => x.Id.ToString() == model.Designation).FirstOrDefault().Designation;
                        //var userDesignation = db.AspNetUsers.Where(x => model.ContactAssignees.Any(x=>x.ContactAssigneeId.ToString().Contains(x.Id.ToString()))).Select(x=>x.).ToList();
                        if (model.Id == 0 || model.Id == null)
                        {
                            
                           

                            var newContact = this._mapper.Map<tbl_Contacts>(model);
                           
                            // newContact.Attachment = ;
                            newContact.RecordStatus = true;
                            newContact.CreatedBy = userid;
                            newContact.CreationDate = UtilService.GetPkCurrentDateTime();
                            
                            // newContact.Designation =String.Join(",",model.ContactAssignees.Select(x=>x.ContactAssigneeName).ToArray());
                            newContact.Title = model.Title;
                            newContact.Name = model.Name;
                            //newContact.Designation = model.Designation;
                            //newContact.Department = model.Department;
                            newContact.Email = model.Email;
                            newContact.MobileNo = model.MobileNo;
                            newContact.PhoneNo = model.PhoneNo;
                            newContact.CNIC = model.CNIC;
                            newContact.DateOfBirth = model.DateOfBirth;
                            newContact.ResourceOFContact = model.ResourceOFContact;
                            newContact.Gender = model.Gender;
                            newContact.Address = model.Address;
                            newContact.Division = model.Division;
                            newContact.District = model.District;
                            newContact.Tehsil = model.Tehsil;
                            newContact.DesignationId = model.DesignationId;
                          
                            newContact.CategoryId = model.CategoryId;
                            newContact.DepartmentId = model.DepartmentId;
                            newContact.CompanyId = model.CompanyId;
                            if (model.DesignationId > 0)
                            {
                                var tDesignationname = db.tbl_ContactDesignation.FirstOrDefault(a => a.Id == model.DesignationId);
                                if (tDesignationname != null)
                                {
                                    //newContact.Designation = tDesignationname.Designation;
                                    userDesignationName = tDesignationname.Designation;
                                }
                            }
                            newContact.Designation = userDesignationName;
                            if (model.DepartmentId > 0)
                            {
                                var tDepartmentname = db.ContactDepartment.FirstOrDefault(a => a.Id == model.DepartmentId);
                                if (tDepartmentname != null)
                                {
                                    newContact.Department = tDepartmentname.DepartmentName;
                                }
                            }
                          


                            if (model.ProfilePicAttchment != null)
                            {

                                newContact.ProfilePic = await UploadFile(model.ProfilePicAttchment);
                                userProfilePic = newContact.ProfilePic;


                            }



                            db.tbl_Contacts.Add(newContact);

                            db.SaveChanges();

                            Applicationuser user = new Applicationuser()
                            {
                                PhoneNumber = model.MobileNo,
                                Email = model.Email,
                                SecurityStamp = Guid.NewGuid().ToString(),
                                UserName = model.MobileNo+RandomString(3),
                                FullName = model.Name,
                                Designation = userDesignationName,
                                ContactDesignationId=model.DesignationId,
                                ContactCategoryId=model.CategoryId,
                                ContactDepartmentId=model.DepartmentId,
                                ContactCompanyId=model.CompanyId,
                                NP = model.MobileNo + "@Mms",
                                ContactId = newContact.Id,
                                IsRecordStatus = true,
                                IsDeleted = false,
                              ProfilePic = userProfilePic,


                            

                        };

                            ///
                            var result = await userManager.CreateAsync(user,  model.MobileNo + "@Mms" );



                            //-------------conferenceAssignee User Role------------
                            var userch = db.AspNetUsers.FirstOrDefault(a => a.Id == user.Id);
                            if (userch != null)
                            {
                                var Role = db.AspNetRoles.FirstOrDefault(a => a.Name.ToUpper() == "DAILYENGAGEMENTVIEW");
                                if (Role != null)
                                {
                                    var UserRoles = db.AspNetUserRoles.FirstOrDefault(a => a.UserId == userch.Id && a.RoleId == Role.Id);
                                    if (UserRoles == null)
                                    {
                                        var AssignRole = new AspNetUserRoles();
                                        AssignRole.UserId = userch.Id;
                                        AssignRole.RoleId = Role.Id;
                                        db.AspNetUserRoles.Add(AssignRole);
                                    }
                                }
                            }
                            //-------------conferenceAssignee User Role------------
                            db.SaveChanges();




                            if (result.Succeeded)
                            {
                                trans.Commit();

                            }
                           

                            return 1;

                        }
                        else
                        {
                            // Map Data of input model
                            var newTask = this._mapper.Map<tbl_Task>(model);
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


        #region UpdateContact
        public async Task<int> UpdateContact(ContactDTO model, string userid)
        {

            using (var db = new IDDbContext())
            {
                using (var trans = db.Database.BeginTransaction())
                {
                    try
                    {
                        string userDesignationName = "";
                        string userProfilePic = "";
                        //var userDesignation = db.AspNetUsers.Where(x => x.Id.ToString() == model.Designation).FirstOrDefault().Designation;
                        //var userDesignation = db.AspNetUsers.Where(x => model.ContactAssignees.Any(x=>x.ContactAssigneeId.ToString().Contains(x.Id.ToString()))).Select(x=>x.).ToList();
                        if (model.Id != 0)
                        {


                            var updateContact = db.tbl_Contacts.Where(x => x.Id == model.Id).FirstOrDefault();
                            if (model.Id != 0)
                            {
                                // newContact.Attachment = ;
                                updateContact.RecordStatus = true;
                                updateContact.UpdatedBy = userid;
                                updateContact.UpdationDate = UtilService.GetPkCurrentDateTime();

                                // newContact.Designation =String.Join(",",model.ContactAssignees.Select(x=>x.ContactAssigneeName).ToArray());

                                updateContact.Name = model.Name;
                                updateContact.Designation = model.Designation;
                                updateContact.Department = model.Department;
                                updateContact.Email = model.Email;
                                updateContact.MobileNo = model.MobileNo;

                                updateContact.Division = model.Division;
                                updateContact.District = model.District;
                                updateContact.Tehsil = model.Tehsil;
                                updateContact.DesignationId = model.DesignationId;
                                updateContact.CategoryId = model.CategoryId;
                                updateContact.DepartmentId = model.DepartmentId;
                                updateContact.CompanyId = model.CompanyId;

                                if (model.DesignationId > 0)
                                {
                                    var tDesignationname = db.tbl_ContactDesignation.FirstOrDefault(a => a.Id == model.DesignationId);
                                    if (tDesignationname != null)
                                    {
                                        //newContact.Designation = tDesignationname.Designation;
                                        userDesignationName = tDesignationname.Designation;
                                    }
                                }
                                updateContact.Designation = userDesignationName;
                                if (model.DepartmentId > 0)
                                {
                                    var tDepartmentname = db.ContactDepartment.FirstOrDefault(a => a.Id == model.DepartmentId);
                                    if (tDepartmentname != null)
                                    {
                                        updateContact.Department = tDepartmentname.DepartmentName;
                                    }
                                }
                                if (model.ProfilePicAttchment != null)
                                {

                                    updateContact.ProfilePic = await UploadFile(model.ProfilePicAttchment);
                                    userProfilePic = updateContact.ProfilePic;

                                }




                                db.SaveChanges();
                                var user = db.AspNetUsers.FirstOrDefault(a => a.ContactId == model.Id);
                                if(user != null)
                                {
                                    //AspNetUsers user = new AspNetUsers()
                                    //{
                                    user.PhoneNumber = model.MobileNo;
                                    user.Email = model.Email;
                                    user.FullName = model.Name;
                                    user.Designation = userDesignationName;
                                    user.ContactDesignationId = model.DesignationId;
                                    user.ContactCategoryId = model.CategoryId;
                                    user.ContactDepartmentId = model.DepartmentId;
                                    user.ContactCompanyId = model.CompanyId;
                                    user.ContactId = (int)model.Id;
                                    user.IsRecordStatus = true;
                                    user.IsDeleted = false;
                                    user.ProfilePic = userProfilePic;
                                    db.SaveChanges();
                                    //};
                                    db.AspNetUsers.Update(user);
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

        #region GetAllContacts
        public List<ListContactDTO> GetAllContacts(Guid userid, bool AllRecord)
        {
            try
            {
                using (var db = new IDDbContext())
                {

                    if (AllRecord)
                    {


                        List<ListContactDTO> res = new List<ListContactDTO>();

                        res = (from contact in db.tbl_Contacts
                               //join user in db.AspNetUsers on task.CreatedBy equals user.Id.ToString()
                               where contact.RecordStatus == true
                               select new ListContactDTO
                               {
                                   Id = contact.Id,
                                   
                                   Name = contact.Name,
                                   Designation = contact.Designation,
                                   Department = contact.Department,
                                   Email = contact.Email,
                                   MobileNo = contact.MobileNo,
                                   District = contact.District,
                                   Division = contact.Division,
                                   Tehsil = contact.Tehsil,
                                   RecordStatus = contact.RecordStatus,
                                   CreatedBy = contact.CreatedBy,
                                   DesignationId = contact.DesignationId,
                                   CompanyIdName = db.tbl_ContactCompany.Where(x => x.Id == contact.CompanyId).FirstOrDefault().CompanyName,
                                   DesignationIdName = db.tbl_ContactDesignation.Where(x => x.Id == contact.DesignationId).FirstOrDefault().Designation,
                                   DepartmentIdName = db.ContactDepartment.Where(x => x.Id == contact.DepartmentId).FirstOrDefault().DepartmentName,
                               }).ToList();

                        return res;
                    }
                    else
                    {
                        List<ListContactDTO> res = new List<ListContactDTO>();

                        res = (from contact in db.tbl_Contacts
                               //join user in db.AspNetUsers on task.CreatedBy equals user.Id.ToString()
                               where contact.RecordStatus == true
                               select new ListContactDTO
                               {
                                   Id = contact.Id,

                                   Name = contact.Name,
                                   Designation = contact.Designation,
                                   Department = contact.Department,
                                   Email = contact.Email,
                                   MobileNo = contact.MobileNo,
                                   District = contact.District,
                                   Division = contact.Division,
                                   Tehsil = contact.Tehsil,
                                   RecordStatus = contact.RecordStatus,
                                   CreatedBy = contact.CreatedBy,
                                   DesignationId= contact.DesignationId,
                                   CompanyIdName = db.tbl_ContactCompany.Where(x => x.Id == contact.CompanyId).FirstOrDefault().CompanyName,
                                   DesignationIdName = db.tbl_ContactDesignation.Where(x => x.Id == contact.DesignationId).FirstOrDefault().Designation,
                                   DepartmentIdName = db.ContactDepartment.Where(x => x.Id == contact.DepartmentId).FirstOrDefault().DepartmentName,
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


        #region GetContactListsByFilter
        public async Task<List<ListContactDTO>> GetContactListsByFilter(Guid userid, ContactFilterDTO ContactkFilterDTO, bool isAdmin)
        {
            try
            {

                using (var db = new IDDbContext())
                {
                    List<ListContactDTO> res = new List<ListContactDTO>();

                    if (isAdmin)

                        res = (from contact in db.tbl_Contacts
                               join user in db.AspNetUsers on contact.CreatedBy equals user.Id.ToString()
                               join locdiv in db.AppLocations on contact.Division equals locdiv.Code
                               join locdis in db.AppLocations on contact.District equals locdis.Code
                               join locteh in db.AppLocations on contact.Tehsil equals locteh.Code
                               where contact.RecordStatus == true
                               select new ListContactDTO
                               {
                                   Id = contact.Id,
                                  
                                   Name = contact.Name,
                                   Designation = contact.Designation,
                                   Department = contact.Department,
                                   Email = contact.Email,
                                   MobileNo = contact.MobileNo,
                                 
                                   District = locdis.Name,
                                   Division = locdiv.Name,
                                   Tehsil = locteh.Name,
                                   CreatedBy = contact.CreatedBy,
                                   CreationDate = contact.CreationDate,
                                   ProfilePic = contact.ProfilePic,
                                   DesignationId = contact.DesignationId,
                                   CompanyIdName = db.tbl_ContactCompany.Where(x => x.Id == contact.CompanyId).FirstOrDefault().CompanyName,
                                   DesignationIdName = db.tbl_ContactDesignation.Where(x => x.Id == contact.DesignationId).FirstOrDefault().Designation,
                                   DepartmentIdName = db.ContactDepartment.Where(x => x.Id == contact.DepartmentId).FirstOrDefault().DepartmentName,
                                   CategoryIdName = db.tbl_ContactCategory.Where(x => x.Id == contact.CategoryId).FirstOrDefault().CategoryName,
                               }
                            ).OrderBy(x=>x.DesignationId).ToList(); 
                    else
                        res = (from contact in db.tbl_Contacts
                               join user in db.AspNetUsers on contact.CreatedBy equals user.Id.ToString()
                               join locdiv in db.AppLocations on contact.Division equals locdiv.Code
                               join locdis in db.AppLocations on contact.District equals locdis.Code
                               join locteh in db.AppLocations on contact.Tehsil equals locteh.Code
                               where contact.RecordStatus == true 
                               //&& contact.CreatedBy == userid.ToString()

                               select new ListContactDTO
                               {
                                   Id = contact.Id,
                                
                                   Name = contact.Name,
                                   Designation = contact.Designation,
                                   Department = contact.Department,
                                   Email = contact.Email,
                                   MobileNo = contact.MobileNo,

                                   //District = contact.District,
                                   //Division = contact.Division,
                                   //Tehsil = contact.Tehsil,
                                   District = locdis.Name,
                                   Division = locdiv.Name,
                                   Tehsil = locteh.Name,
                                   CreatedBy = contact.CreatedBy,
                                   CreationDate = contact.CreationDate,
                                   ProfilePic = contact.ProfilePic,
                                   DesignationId=contact.DesignationId,
                                   CompanyIdName = db.tbl_ContactCompany.Where(x => x.Id == contact.CompanyId).FirstOrDefault().CompanyName,
                                   DesignationIdName = db.tbl_ContactDesignation.Where(x => x.Id == contact.DesignationId).FirstOrDefault().Designation,
                                   DepartmentIdName = db.ContactDepartment.Where(x => x.Id == contact.DepartmentId).FirstOrDefault().DepartmentName,
                                   CategoryIdName = db.tbl_ContactCategory.Where(x => x.Id == contact.CategoryId).FirstOrDefault().CategoryName,
                               }
                            ).OrderBy(x => x.DesignationId).ToList();


                    if (ContactkFilterDTO.ContactDesignation != null)
                    {

                      //old Way  res = res.Where(x => x.Designation.Contains(ContactkFilterDTO.ContactDesignation.ToString())).ToList();
                        res = res.Where(x => x.DesignationId== Convert.ToInt32( ContactkFilterDTO.ContactDesignation)).ToList();
                        //res = res.Where(x => x.AssignTo == TaskFilterDTO.userList).ToList();


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

        #region GetContactListsByAllFilter
        public async Task<List<ListContactDTO>> GetContactListsByAllFilter(Guid userid, ContactAllFilterDTO ContactAllFilterDTO, bool isAdmin)
        {
            try
            {

                using (var db = new IDDbContext())
                {
                    List<ListContactDTO> res = new List<ListContactDTO>();

                    if (isAdmin) { 

                        res = (from contact in db.tbl_Contacts
                               join user in db.AspNetUsers on contact.CreatedBy equals user.Id.ToString()
                               join locdiv in db.AppLocations on contact.Division equals locdiv.Code
                               join locdis in db.AppLocations on contact.District equals locdis.Code
                               join locteh in db.AppLocations on contact.Tehsil equals locteh.Code
                               where contact.RecordStatus == true &&
                               (
                                 ( contact.CompanyId == ( ContactAllFilterDTO.ContactCompanyIds <=0 ? contact.CompanyId : ContactAllFilterDTO.ContactCompanyIds)
                                  &&
                                  contact.DepartmentId == (ContactAllFilterDTO.ContactDepartmentIds <= 0 ? contact.DepartmentId : ContactAllFilterDTO.ContactDepartmentIds)
                                  &&
                                  contact.CategoryId == (ContactAllFilterDTO.ContactCategoryIds <= 0 ? contact.CategoryId : ContactAllFilterDTO.ContactCategoryIds)
                                  //&&
                                  //contact.DesignationId == (ContactAllFilterDTO.ContactDesignationIds <= 0 ? contact.DesignationId : ContactAllFilterDTO.ContactDesignationIds)
                                  )
                               )
                               select new ListContactDTO
                               {
                                   Id = contact.Id,

                                   Name = contact.Name,
                                   Designation = contact.Designation,
                                   Department = contact.Department,
                                   Email = contact.Email,
                                   MobileNo = contact.MobileNo,

                                   District = locdis.Name,
                                   Division = locdiv.Name,
                                   Tehsil = locteh.Name,
                                   CreatedBy = contact.CreatedBy,
                                   CreationDate = contact.CreationDate,
                                   ProfilePic = contact.ProfilePic,
                                   DesignationId = contact.DesignationId,
                                   CompanyIdName = db.tbl_ContactCompany.Where(x => x.Id == contact.CompanyId).FirstOrDefault().CompanyName,
                                   DesignationIdName = db.tbl_ContactDesignation.Where(x => x.Id == contact.DesignationId).FirstOrDefault().Designation,
                                   DepartmentIdName = db.ContactDepartment.Where(x => x.Id == contact.DepartmentId).FirstOrDefault().DepartmentName,
                                   CategoryIdName = db.tbl_ContactCategory.Where(x => x.Id == contact.CategoryId).FirstOrDefault().CategoryName,
                               }
                            ).OrderBy(x => x.DesignationId).ToList();
                    }
                    else
                    {
                        var DivisionCode = "";
                        int ContactId = db.AspNetUsers.FirstOrDefault(x => x.Id == userid).ContactId;
                        if (ContactId > 0)
                        {
                            DivisionCode = db.tbl_Contacts.FirstOrDefault(x => x.Id == ContactId).Division;

                        }
                        res = (from contact in db.tbl_Contacts
                               join user in db.AspNetUsers on contact.CreatedBy equals user.Id.ToString()
                               join locdiv in db.AppLocations on contact.Division equals locdiv.Code
                               join locdis in db.AppLocations on contact.District equals locdis.Code
                               join locteh in db.AppLocations on contact.Tehsil equals locteh.Code
                               where contact.RecordStatus == true &&
                               (
                                 (contact.CompanyId == (ContactAllFilterDTO.ContactCompanyIds <= 0 ? contact.CompanyId : ContactAllFilterDTO.ContactCompanyIds)
                                  &&
                                  contact.DepartmentId == (ContactAllFilterDTO.ContactDepartmentIds <= 0 ? contact.DepartmentId : ContactAllFilterDTO.ContactDepartmentIds)
                                  &&
                                  contact.CategoryId == (ContactAllFilterDTO.ContactCategoryIds <= 0 ? contact.CategoryId : ContactAllFilterDTO.ContactCategoryIds)
                                  &&
                                  contact.DesignationId == (ContactAllFilterDTO.ContactDesignationIds <= 0 ? contact.DesignationId : ContactAllFilterDTO.ContactDesignationIds)
                                  )
                               ) && contact.Division == DivisionCode

                               select new ListContactDTO
                               {
                                   Id = contact.Id,

                                   Name = contact.Name,
                                   Designation = contact.Designation,
                                   Department = contact.Department,
                                   Email = contact.Email,
                                   MobileNo = contact.MobileNo,

                                   //District = contact.District,
                                   //Division = contact.Division,
                                   //Tehsil = contact.Tehsil,
                                   District = locdis.Name,
                                   Division = locdiv.Name,
                                   Tehsil = locteh.Name,
                                   CreatedBy = contact.CreatedBy,
                                   CreationDate = contact.CreationDate,
                                   ProfilePic = contact.ProfilePic,
                                   DesignationId = contact.DesignationId,
                                   CompanyIdName = db.tbl_ContactCompany.Where(x => x.Id == contact.CompanyId).FirstOrDefault().CompanyName,
                                   DesignationIdName = db.tbl_ContactDesignation.Where(x => x.Id == contact.DesignationId).FirstOrDefault().Designation,
                                   DepartmentIdName = db.ContactDepartment.Where(x => x.Id == contact.DepartmentId).FirstOrDefault().DepartmentName,
                                   CategoryIdName = db.tbl_ContactCategory.Where(x => x.Id == contact.CategoryId).FirstOrDefault().CategoryName,
                               }
                            ).OrderBy(x => x.DesignationId).ToList();
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




        public string ReadJson()
        {
            using (StreamReader r = new StreamReader(_env.ContentRootPath + "/Json/Menu.json"))
            {
                string json = r.ReadToEnd();
                return json;
            }

        }



        public void DeleteContact(int Id)
        {

            using (var db = new IDDbContext())
            {
                using (var trans = db.Database.BeginTransaction())
                {
                    try
                    {

                        if (Id != 0)
                        {

                            var contact = db.tbl_Contacts.Where(x => x.Id == Id).FirstOrDefault();

                            contact.RecordStatus = false;

                            
                            var user = db.AspNetUsers.FirstOrDefault(a =>a.IsRecordStatus==true && a.IsDeleted == false && a.ContactId == contact.Id);
                            if (user != null)
                            {
                                user.IsRecordStatus = false;
                                user.IsDeleted = true;
                            }

                            //Applicationuser user = new Applicationuser()
                            //{

                            //    UserName = contact.MobileNo + RandomString(3),

                            //    ContactId = newContact.Id,
                            //    IsRecordStatus = true,
                            //    IsDeleted = false,

                            //};
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



        #region GetContactById
        public List<ListContactDTO> GetContactById(int taskId)
        {
            try
            {


                using (var db = new IDDbContext())
                {

                    List<ListContactDTO> res = new List<ListContactDTO>();

                    res = db.tbl_Contacts
                        .Where(t => t.Id == taskId && t.RecordStatus == true)
                        .Select(t => new ListContactDTO
                        {
                            Id = t.Id,
                                   
                                   Name = t.Name,
                                   Designation = t.Designation,
                                   Department = t.Department,
                                   Email = t.Email,
                                   MobileNo = t.MobileNo,
                                   District = t.District,
                                   Division = t.Division,
                                   Tehsil = t.Tehsil,
                                   RecordStatus = t.RecordStatus,
                                   CreatedBy = t.CreatedBy,
                                   
                                  CompanyId=t.CompanyId,
                                  DepartmentId=t.DepartmentId,
                                  CategoryId=t.CategoryId,
                                  DesignationId =t.DesignationId,
                            CompanyIdName = db.tbl_ContactCompany.Where(x => x.Id == t.CompanyId).FirstOrDefault().CompanyName,
                            DepartmentIdName = db.ContactDepartment.Where(x => x.Id == t.DepartmentId).FirstOrDefault().DepartmentName,
                            CategoryIdName = db.tbl_ContactCategory.Where(x => x.Id == t.CategoryId).FirstOrDefault().CategoryName,
                            DesignationIdName = db.tbl_ContactDesignation.Where(x => x.Id == t.DesignationId).FirstOrDefault().Designation,
                            
                            
                         
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


        //#region GetDetailsContactList
        //public List<ListContactDTO> GetDetailsContactList(int taskid)
        //{
        //    try
        //    {

        //        using (var db = new IDDbContext())
        //        {

        //            List<ListContactDTO> res = new List<ListContactDTO>();

        //            res = (from t in db.tbl_Contacts
        //                   join c in db.AspNetUsers on t.CreatedBy equals c.Id.ToString()

        //                   where (t.TaskId == taskid && t.RecordStatus == true)
        //                   select new ListContactDTO
        //                   {
        //                       Id = t.Id,
        //                       TaskStatusName = t.Task.TaskStatusNavigation.Status,
        //                       TaskId = (int)t.TaskId,
        //                       Attachment = t.Attachment
        //                       .Where(x => x.SourceName == "Comments")
        //                       .Select(x => "/Uploads/" + x.AttachmentName).ToList(),
        //                       Comments = t.Comments,
        //                       ReadDateTime = t.ReadDateTime,
        //                       IsRead = t.IsRead,
        //                       CreatedBy = c.Designation,
        //                       CreatedDate = t.CreatedDate,
        //                       TaskAssignee = String.Join(",", t.Task.tbl_TaskAssignee.Select(x => x.TaskAssignTo.FullName).ToList()),
        //                       TaskAssigneeId = String.Join(",", t.Task.tbl_TaskAssignee.Select(x => x.TaskAssignToID).ToList()),
        //                       ExtendedDate = t.Task.ExtendedDate


        //                   }).ToList();


        //            return res;

        //        }
        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //}

        //#endregion

        #region AddContactType
        public int AddContactType(ContactTypeDTO model, string userid)
        {

            using (var db = new IDDbContext())
            {
                using (var trans = db.Database.BeginTransaction())
                {
                    try
                    {
                        //var userDesignation = db.AspNetUsers.Where(x => x.Id.ToString() == model.Designation).FirstOrDefault().Designation;
                        //var userDesignation = db.AspNetUsers.Where(x => model.ContactAssignees.Any(x=>x.ContactAssigneeId.ToString().Contains(x.Id.ToString()))).Select(x=>x.).ToList();
                       

                       var newContactType = this._mapper.Map<tbl_ContactDesignation>(model);

                        newContactType.RecordStatus = true;
                        newContactType.CreatedBy = userid;
                        newContactType.CreationDate = UtilService.GetPkCurrentDateTime();
                        newContactType.DepartmentId = model.DepartmentId;
                        newContactType.CategoryId = model.CategoryId;

                        newContactType.Designation = model.Designation;
                        newContactType.OderBy = 100;
                      
                         






                            db.tbl_ContactDesignation.Add(newContactType);

                            db.SaveChanges();











                            trans.Commit();
                            return 1;

                        
                       

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






        #region HelperMethods
        private static Random random = new Random();

        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
        public async Task<string> UploadFile(IFormFile file)
        {
            try
            {
                string filename = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                filename = this.EnsureCorrectFilename($"{Guid.NewGuid().ToString("N")}_{ DateTime.UtcNow.AddHours(5).ToString("ddMMyyyyHHmmssffffff")}");
                filename = filename + ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                string path = _env.WebRootPath + "/Uploads/ProfilePictures/" + filename;


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







    }

}
