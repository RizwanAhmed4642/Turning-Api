
using Meeting_App.Data.Database.Tables;
using Meeting_App.Models;
using Meeting_App.Models.DTOs;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Meeting_App.Service.Common
{
    public class CommonService
    {
        private readonly UserManager<Applicationuser> _userManager;
        private const int PROBABILITY_ONE = 0x7fffffff;

        public CommonService(UserManager<Applicationuser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<bool> CheckRoleExists(Guid UserId, string Role)
        {
            var user = await _userManager.FindByIdAsync(UserId.ToString());
            return await _userManager.IsInRoleAsync(user, Role);
        }

        //#region fingerprint
        //public bool compare(fmd fmd1, fmd fmd2)
        //{
        //    var compareresult = comparison.compare(fmd1, 0, fmd2, 0);
        //    if ((compareresult.score < (probability_one / 100000)))
        //    {
        //        return true;
        //    }
        //    else
        //    {
        //        return false;
        //    }
        //}
        //public Fmd GetFmd(string base64string)
        //{
        //    var finger = base64string;
        //    byte[] data = Convert.FromBase64String(finger);
        //    var stream = new MemoryStream(data, 0, data.Length);
        //    Image image = Image.FromStream(stream);
        //    byte[] arr = Encoding.ASCII.GetBytes(finger);


        //    Reader reader = new Reader();
        //    var fmd =
        //        DPUruNet.FeatureExtraction.CreateFmdFromRaw(
        //                ExtractByteArray(new Bitmap(image)),
        //                0,
        //                0,
        //                image.Width,
        //                image.Height,
        //                500,
        //                DPUruNet.Constants.Formats.Fmd.ANSI)
        //            .Data;
        //    return fmd;
        //}
        //public string Getbase64FromFMD(string dbObj)
        //{
        //    if (dbObj == null) return null;
        //    var fmd = Fmd.DeserializeXml(dbObj);
        //    var fmdBytes = Fmd.DeserializeXml(dbObj).Bytes;
        //    return Convert.ToBase64String(fmdBytes);
        //}

        public Bitmap CreateBitmap(byte[] bytes, int width, int height)
        {
            byte[] rgbBytes = new byte[bytes.Length * 3];

            for (int i = 0; i <= bytes.Length - 1; i++)
            {
                rgbBytes[(i * 3)] = bytes[i];
                rgbBytes[(i * 3) + 1] = bytes[i];
                rgbBytes[(i * 3) + 2] = bytes[i];
            }
            Bitmap bmp = new Bitmap(width, height, PixelFormat.Format24bppRgb);

            BitmapData data = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.WriteOnly, PixelFormat.Format24bppRgb);

            for (int i = 0; i <= bmp.Height - 1; i++)
            {
                IntPtr p = new IntPtr(data.Scan0.ToInt64() + data.Stride * i);
                System.Runtime.InteropServices.Marshal.Copy(rgbBytes, i * bmp.Width * 3, p, bmp.Width * 3);
            }

            bmp.UnlockBits(data);

            return bmp;
        }


        private static byte[] ExtractByteArray(Bitmap img)
        {
            byte[] rawData = null;
            byte[] bitData = null;
            //ToDo: CreateFmdFromRaw only works on 8bpp bytearrays. As such if we have an image with 24bpp then average every 3 values in Bitmapdata and assign it to bitdata
            if (img.PixelFormat == PixelFormat.Format8bppIndexed)
            {

                //Lock the bitmap's bits
                BitmapData bitmapdata = img.LockBits(new System.Drawing.Rectangle(0, 0, img.Width, img.Height), System.Drawing.Imaging.ImageLockMode.ReadWrite, img.PixelFormat);
                //Declare an array to hold the bytes of bitmap
                byte[] imgData = new byte[bitmapdata.Stride * bitmapdata.Height]; //stride=360, height 392

                //Copy bitmapdata into array
                Marshal.Copy(bitmapdata.Scan0, imgData, 0, imgData.Length);//imgData.length =141120

                bitData = new byte[bitmapdata.Width * bitmapdata.Height];//ditmapdata.width =357, height = 392

                for (int y = 0; y < bitmapdata.Height; y++)
                {
                    for (int x = 0; x < bitmapdata.Width; x++)
                    {
                        bitData[bitmapdata.Width * y + x] = imgData[y * bitmapdata.Stride + x];
                    }
                }

                rawData = new byte[bitData.Length];

                for (int i = 0; i < bitData.Length; i++)
                {
                    int avg = (img.Palette.Entries[bitData[i]].R + img.Palette.Entries[bitData[i]].G + img.Palette.Entries[bitData[i]].B) / 3;
                    rawData[i] = (byte)avg;
                }
            }

            else
            {
                bitData = new byte[img.Width * img.Height];//ditmapdata.width =357, height = 392, bitdata.length=139944
                for (int y = 0; y < img.Height; y++)
                {
                    for (int x = 0; x < img.Width; x++)
                    {
                        Color pixel = img.GetPixel(x, y);
                        bitData[img.Width * y + x] = (byte)((Convert.ToInt32(pixel.R) + Convert.ToInt32(pixel.G) + Convert.ToInt32(pixel.B)) / 3);
                    }
                }

            }

            return bitData;
        }

 

        #region GetAssignedToList
        public List<DDLModel> GetAssignedToList(string userId)
        {
            try
            {
                using var db = new IDDbContext();
                Guid userGuid = Guid.Parse(userId);
                var user = db.AspNetUsers.FirstOrDefault(x => x.Id == userGuid);

                return db.AspNetUsers.Where(x => x.Order > user.Order).OrderBy(x => x.Order).Select(x => new DDLModel { Id = x.Id, Name = x.FullName }).ToList();
            }
            catch (Exception)
            {

                throw;
            }
        }
        public List<EventParticipantView> GetParticepentList(string userId)
        {
            try
            {
                using var db = new IDDbContext();
                Guid userGuid = Guid.Parse(userId);
                var user = db.AspNetUsers.FirstOrDefault(x => x.Id == userGuid);

                return db.AspNetUsers.OrderBy(x => x.Order).Select(x => new EventParticipantView { ParticipantId = x.Id, ParticipantName = x.FullName }).ToList();
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region GetTaskStatus
        public List<DDLStatusModel> GetTaskStatus()
        {
            try
            {
                using var db = new IDDbContext();

                return db.tbl_Status.Select(x => new DDLStatusModel { Id = x.Id, Status = x.Status }).ToList();
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region AppVersion
        public ApplicationVersion AppVersion()
        {
            try
            {
                using (var db = new IDDbContext())
                {
                    var app = db.ApplicationVersion.Select(x =>
                    new ApplicationVersion
                    {
                        App_Version = x.App_Version,
                        Json_Version = x.Json_Version
                    }).FirstOrDefault();

                    return app;

                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region GetDivisionList
        public List<DDLModel> GetDivision(string geoLevel)
        {
            try
            {
                using (var db = new IDDbContext())
                {
                    if (geoLevel == "0" || geoLevel == null)
                        return db.AppLocations.Where(x => x.Type == "Division").Select(x => new DDLModel { Code = x.Code, Name = x.Name }).ToList();
                    else
                    {
                        var c = geoLevel.Substring(0, 3);
                        return db.AppLocations.Where(x => x.Type == "Division" && x.Code.StartsWith(c)).Select(x => new DDLModel { Code = x.Code, Name = x.Name }).ToList();

                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region GetDistrictList
        public List<DDLModel> GetDistrict(string geoLevel)
        {
            try
            {
                using (var db = new IDDbContext())
                {
                    if (geoLevel != null)
                        if (geoLevel.Length == 3)
                            return db.AppLocations.Where(x => x.Code.StartsWith(geoLevel) && x.Type == "District").Select(x => new DDLModel { Code = x.Code, Name = x.Name }).ToList();

                    if (geoLevel == null)
                        return db.AppLocations.Where(x => x.Type == "District").Select(x => new DDLModel { Code = x.Code, Name = x.Name }).ToList();
                    else
                    {
                        var c = geoLevel.Substring(0, 6);
                        return db.AppLocations.Where(x => x.Code.StartsWith(c) && x.Type == "District").Select(x => new DDLModel { Code = x.Code, Name = x.Name }).ToList();

                    }

                }
            }
            catch
            {
                throw;
            }

        }
        #endregion

        #region GetTehsil
        public List<DDLModel> GetTehsil(string geoLevel)
        {

            try
            {
                using (var db = new IDDbContext())
                {
                    if (geoLevel == null)
                        return db.AppLocations.Where(x => x.Type == "Tehsil").Select(x => new DDLModel { Code = x.Code, Name = x.Name }).ToList();

                    if (geoLevel.Length == 3)
                        return db.AppLocations.Where(x => x.Code.StartsWith(geoLevel) && x.Type == "Tehsil").Select(x => new DDLModel { Code = x.Code, Name = x.Name }).ToList();


                    if (geoLevel.Length == 6)
                        return db.AppLocations.Where(x => x.Code.StartsWith(geoLevel) && x.Type == "Tehsil").Select(x => new DDLModel { Code = x.Code, Name = x.Name }).ToList();
                    else
                    {
                        var c = geoLevel.Substring(0, 9);
                        return db.AppLocations.Where(x => x.Code.StartsWith(c) && x.Type == "Tehsil").Select(x => new DDLModel { Code = x.Code, Name = x.Name }).ToList();

                    }
                }
            }
            catch
            {
                throw;
            }
        }

        #endregion

        #region GetContactCompanyList
        public List<CommonClass> GetContactCompanyList()
        {
            try
            {
                using (var db = new IDDbContext())
                {
                    return db.tbl_ContactCompany.OrderBy(x => x.OderBy).Select(x => new CommonClass { Id = x.Id, Name = x.CompanyName ,NameDisplay=x.CompanyName}).ToList();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region GetContactCompanyListSPL
        public List<CommonClass> GetContactCompanyListSPL()
        {
            try
            {
                using (var db = new IDDbContext())
                {
                    return db.tbl_ContactCompany.OrderBy(x => x.OderBy).Select(x => new CommonClass { Id = x.Id, Name = x.CompanyName }).ToList();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        //-----------------Multi Dropdown --------------
        #region GetContactMultiDepartmentList
        public List<ContactDepartment> GetContactMultiDepartmentList(ContactMultiFilterDTO ContactMultiFilterDTO)
        {
            try
            {
                using (var db = new IDDbContext())
                {
                    List<ContactDepartment> res1 = new List<ContactDepartment>();
                    List<ContactDepartment> res2 = new List<ContactDepartment>();
                    if (ContactMultiFilterDTO.ContactCompanyIds != null)
                    {

                        //ContactkFilterDTO.ContactDesignation = ContactkFilterDTO.ContactDesignation.TrimEnd(',');
                        List<string> ContactCompanyList = ContactMultiFilterDTO.ContactCompanyIds.Split(",").ToList();

                        if (ContactCompanyList != null && ContactCompanyList.Count > 0)
                        {
                            foreach (var item in ContactCompanyList)
                            {
                                if (!string.IsNullOrEmpty(item))
                                {
                                    int? CompanyID = Convert.ToInt32(item);
                                    res1 = db.ContactDepartment.Where(a => a.RecordStatus == true && a.CompanyId == CompanyID).ToList();
                                    foreach (var item1 in res1)
                                    {
                                        item1.DepartmentName = item1.DepartmentName;
                                        item1.Id = item1.Id;
                                        //old way item1.FullName = item1.FullName + " ( " + item + " )";
                                    }
                                    res2.AddRange(res1);
                                }
                            }
                        }

                    }
                    return res2;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion
        //---------------------------SPL---------------------------------
        #region GetContactMultiDepartmentListSPL
        public List<ContactDepartment> GetContactMultiDepartmentListSPL(ContactMultiFilterDTO ContactMultiFilterDTO)
        {
            try
            {
                using (var db = new IDDbContext())
                {
                    List<ContactDepartment> res1 = new List<ContactDepartment>();
                    List<ContactDepartment> res2 = new List<ContactDepartment>();
                    if (ContactMultiFilterDTO.ContactCompanyIds != null)
                    {

                        //ContactkFilterDTO.ContactDesignation = ContactkFilterDTO.ContactDesignation.TrimEnd(',');
                        List<string> ContactCompanyList = ContactMultiFilterDTO.ContactCompanyIds.Split(",").ToList();

                        if (ContactCompanyList != null && ContactCompanyList.Count > 0)
                        {
                            foreach (var item in ContactCompanyList)
                            {
                                if (!string.IsNullOrEmpty(item))
                                {
                                    int? CompanyID = Convert.ToInt32(item);
                                    res1 = db.ContactDepartment.Where(a => a.RecordStatus == true && a.CompanyId == CompanyID).ToList();
                                    foreach (var item1 in res1)
                                    {
                                        item1.DepartmentName = item1.DepartmentName;
                                        item1.Id = item1.Id;
                                        //old way item1.FullName = item1.FullName + " ( " + item + " )";
                                    }
                                    res2.AddRange(res1);
                                }
                            }
                        }

                    }
                    return res2;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion
        //---------------------------SPL---------------------------------
        #region GetContactMultiCategoryList
        public List<tbl_ContactCategory> GetContactMultiCategoryList(ContactMultiFilterDTO ContactMultiFilterDTO)
        {
            try
            {
                using (var db = new IDDbContext())
                {
                    List<tbl_ContactCategory> res1 = new List<tbl_ContactCategory>();
                    List<tbl_ContactCategory> res2 = new List<tbl_ContactCategory>();
                    if (ContactMultiFilterDTO.ContactCompanyIds != null)
                    {

                        //ContactkFilterDTO.ContactDesignation = ContactkFilterDTO.ContactDesignation.TrimEnd(',');
                        List<string> ContactDepartmentList = ContactMultiFilterDTO.ContactDepartmentIds.Split(",").ToList();

                        if (ContactDepartmentList != null && ContactDepartmentList.Count > 0)
                        {
                            foreach (var item in ContactDepartmentList)
                            {
                                if (!string.IsNullOrEmpty(item))
                                {
                                    int? DepartmentID = Convert.ToInt32(item);
                                    res1 = db.tbl_ContactCategory.Where(a => a.RecordStatus == true && a.DepartmentId == DepartmentID).OrderBy(x => x.OderBy).ToList();
                                    foreach (var item1 in res1)
                                    {
                                        item1.CategoryName = item1.CategoryName;
                                        item1.Id = item1.Id;
                                        //old way item1.FullName = item1.FullName + " ( " + item + " )";
                                    }
                                    res2.AddRange(res1);
                                }
                            }
                        }

                    }
                    return res2;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion
        //-----------------Multi Dropdown --------------


        #region GetContactDepartmentList
        public List<CommonClass> GetContactDepartmentList(int? Id = 0, string Type = "Single", List<ContactsFilterDTO> Data = null)
        {
            try
            {
                using (var db = new IDDbContext())
                {
                    if (Type == "Single")
                        return db.ContactDepartment.Where(x => x.CompanyId == Id && x.RecordStatus == true).OrderBy(x => x.OderBy).Select(x => new CommonClass { Id = x.Id, Name = x.DepartmentName }).ToList();
                    else
                    {
                        List<CommonClass> AddRangeList = new List<CommonClass>();
                        List<CommonClass> res = new List<CommonClass>();
                        var CampanyList = db.tbl_ContactCompany.ToList();
                        List<CommonClass> res1 = new List<CommonClass>();
                        foreach (var item in Data)
                        {
                            res = db.ContactDepartment.Where(x => x.CompanyId == item.Id && x.RecordStatus == true).OrderBy(x => x.OderBy).Select(x => new CommonClass { Id = x.Id, Name = x.DepartmentName,CompanyId=x.CompanyId }).ToList();
                            var campanies = db.tbl_ContactCompany.ToList();

                            foreach (var r in res)
                            {

                                
                                    CommonClass model = new CommonClass();
                                    model.Id = r.Id;
                                    model.CompanyId = r.CompanyId;
                                    model.Name = r.Name;
                                    var Company = campanies.Where(x => x.Id == r.CompanyId).FirstOrDefault();
                                    model.NameCompany = Company.CompanyName;
                                    model.NameDisplay = model.Name + " " + "(" + model.NameCompany + ")";
                                    res1.Add(model);

                                
                                   


                            }
                            AddRangeList.AddRange(res1);
                        }
                        return AddRangeList;
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region GetContactCategoryList
        public List<CommonClass> GetContactCategoryList(int? Id = 0, string Type = "Single", List<ContactsFilterDTO> Data = null)
        {
            try
            {
                using (var db = new IDDbContext())
                {
                    if (Type == "Single")
                        return db.tbl_ContactCategory.Where(x => x.DepartmentId == Id && x.RecordStatus == true).OrderBy(x => x.OderBy).Select(x => new CommonClass { Id = x.Id, Name = x.CategoryName }).ToList();
                    else
                    {
                        List<CommonClass> AddRangeList = new List<CommonClass>();
                        List<CommonClass> res = new List<CommonClass>();

                        var CampanyList = db.ContactDepartment.ToList();
                        var CampanyList1 = db.tbl_ContactCompany.ToList();
                        List<CommonClass> res1 = new List<CommonClass>();
                        foreach (var item in Data)
                        {
                            res = db.tbl_ContactCategory.Where(x => x.DepartmentId == item.Id && x.RecordStatus == true).OrderBy(x => x.OderBy).Select(x => new CommonClass { Id = x.Id, Name = x.CategoryName, departmentId =x.DepartmentId}).ToList();
                            var departments = db.ContactDepartment.ToList();
                            var campanies = db.tbl_ContactCompany.ToList();
                          
                            foreach (var r in res)
                            {
                              
                                   
                                        CommonClass model = new CommonClass();

                                        model.Id = r.Id;


                                        model.Name = r.Name;
                                        var department = departments.Where(x => x.Id == r.departmentId).FirstOrDefault();
                                        var Company = campanies.Where(x => x.Id == department.CompanyId).FirstOrDefault();

                                        if (Company != null)
                                        {
                                            model.NameCompany = department.DepartmentName + " (" + Company.CompanyName + ")";
                                        }
                                       
                                        model.NameDisplay = model.Name + " " + "(" + model.NameCompany + ")";
                                        res1.Add(model);

                               
                             

                               


                            }

                            AddRangeList.AddRange(res1);
                        }
                        return AddRangeList;
                    }

                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion


        #region GetContactDesignationList
        public List<CommonClass> GetContactDesignation(int? Id)
        {
            try
            {
                using (var db = new IDDbContext())
                {


                    if (Id == 169 || Id == 172 || Id == 173 || Id == 174 || Id == 175 || Id == 176 || Id == 177 || Id == 178 || Id == 179 || Id == 180)
                    {

                        return db.tbl_ContactDesignation.Where(x => x.CategoryId == null && x.RecordStatus == true).OrderBy(x => x.OderBy).Select(x => new CommonClass { Id = x.Id, Name = x.Designation }).ToList();


                    }



                    return db.tbl_ContactDesignation.Where(x => x.CategoryId == Id && x.RecordStatus == true).OrderBy(x => x.OderBy).Select(x => new CommonClass { Id = x.Id, Name = x.Designation }).ToList();

                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region GetContactListByDesignation
        public List<ListContactDTO> GetContactListByDesignation(int? Id = 0, string Type = "Single", List<ContactsFilterDTO> Data = null)
        {
            try
            {

                using (var db = new IDDbContext())
                {
                    List<ListContactDTO> AddRangeList = new List<ListContactDTO>();
                    List<ListContactDTO> res = new List<ListContactDTO>();
                    if (Type == "Single")
                    {
                        res = (from contact in db.tbl_Contacts
                               join user in db.AspNetUsers on contact.Id equals user.ContactId
                               join locdiv in db.AppLocations on contact.Division equals locdiv.Code
                               join locdis in db.AppLocations on contact.District equals locdis.Code
                               join locteh in db.AppLocations on contact.Tehsil equals locteh.Code
                               where contact.RecordStatus == true && contact.CategoryId == Id
                               select new ListContactDTO
                               {
                                   Id = contact.Id,
                                   Name = contact.Name ,
                                   NameDisplay = contact.Name + " (" + contact.Designation + ")",
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
                                   RecordStatus = contact.RecordStatus,
                                   CreatedBy = contact.CreatedBy,
                                   DesignationId = contact.DesignationId,
                                   ParticipantId = user.Id,
                                   ParticipantName = user.FullName

                               }).ToList();
                    }
                    else
                    {

                        foreach (var item in Data)
                        {
                            res = (from contact in db.tbl_Contacts
                                   join user in db.AspNetUsers on contact.Id equals user.ContactId
                                   join locdiv in db.AppLocations on contact.Division equals locdiv.Code
                                   join locdis in db.AppLocations on contact.District equals locdis.Code
                                   join locteh in db.AppLocations on contact.Tehsil equals locteh.Code
                                   where contact.RecordStatus == true && contact.CategoryId == item.Id
                                   select new ListContactDTO
                                   {
                                       Id = contact.Id,
                                       Name = contact.Name,
                                       NameDisplay = contact.Name + " (" + contact.Designation + ")",
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
                                       RecordStatus = contact.RecordStatus,
                                       CreatedBy = contact.CreatedBy,
                                       DesignationId = contact.DesignationId,
                                       ParticipantId = user.Id,
                                       ParticipantName = user.FullName

                                   }).ToList();
                            AddRangeList.AddRange(res);
                        }
                        return AddRangeList;
                    }
                    return res;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region GetContactAllDesignationList
        public List<tbl_ContactDesignation> GetContactAllDesignationList()
        {
            try
            {
                using (var db = new IDDbContext())
                {

                    return db.tbl_ContactDesignation.ToList();

                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        //-------------NIU API Start--------------
        #region GetContactListByFilter
        public List<ListContactDTO> GetContactListByFilter(ContactsFilterDTO filterDTO)
        {
            try
            {
                List<ListContactDTO> res = new List<ListContactDTO>();
                using (var db = new IDDbContext())
                {
                    if (filterDTO.Id == 0 && filterDTO.Type == null)
                    {
                        res = db.ContactsDetailView.Select(x => new ListContactDTO
                        {
                            Id = x.ContactId,
                            Name = x.Name,
                            CategoryIdName = x.categoryName,
                            CategoryId = x.CategoryId,
                            DesignationId = x.DesignationId,
                            DesignationIdName = x.Designation,
                            DepartmentId = x.DepartmentId,
                            DepartmentIdName = x.departmentName,
                            CompanyId = x.CompanyId,
                            CompanyIdName = x.companyName,
                            Email = x.Email,
                            MobileNo = x.MobileNo,


                        }).ToList();
                    }
                    else if (filterDTO.Id > 0 && filterDTO.Type == "Company")
                    {
                        res = db.ContactsDetailView.Where(x => x.CompanyId == filterDTO.Id).Select(x => new ListContactDTO
                        {
                            Id = x.ContactId,
                            Name = x.Name,
                            CategoryIdName = x.categoryName,
                            CategoryId = x.CategoryId,
                            DesignationId = x.DesignationId,
                            DesignationIdName = x.Designation,
                            DepartmentId = x.DepartmentId,
                            DepartmentIdName = x.departmentName,
                            CompanyId = x.CompanyId,
                            CompanyIdName = x.companyName,
                            Email = x.Email,
                            MobileNo = x.MobileNo,

                        }).ToList();


                    }
                    else if (filterDTO.Id > 0 && filterDTO.Type == "Department")
                    {
                        res = db.ContactsDetailView.Where(x => x.DepartmentId == filterDTO.Id).Select(x => new ListContactDTO
                        {
                            Id = x.ContactId,
                            Name = x.Name,
                            CategoryIdName = x.categoryName,
                            CategoryId = x.CategoryId,
                            DesignationId = x.DesignationId,
                            DesignationIdName = x.Designation,
                            DepartmentId = x.DepartmentId,
                            DepartmentIdName = x.departmentName,
                            CompanyId = x.CompanyId,
                            CompanyIdName = x.companyName,
                            Email = x.Email,
                            MobileNo = x.MobileNo,

                        }).ToList();


                    }
                    else if (filterDTO.Id > 0 && filterDTO.Type == "Category")
                    {
                        res = db.ContactsDetailView.Where(x => x.CategoryId == filterDTO.Id).Select(x => new ListContactDTO
                        {
                            Id = x.ContactId,
                            Name = x.Name,
                            CategoryIdName = x.categoryName,
                            CategoryId = x.CategoryId,
                            DesignationId = x.DesignationId,
                            DesignationIdName = x.Designation,
                            DepartmentId = x.DepartmentId,
                            DepartmentIdName = x.departmentName,
                            CompanyId = x.CompanyId,
                            CompanyIdName = x.companyName,
                            Email = x.Email,
                            MobileNo = x.MobileNo,

                        }).ToList();


                    }
                    else if (filterDTO.Id > 0 && filterDTO.Type == "Designation")
                    {
                        res = db.ContactsDetailView.Where(x => x.DesignationId == filterDTO.Id).Select(x => new ListContactDTO
                        {
                            Id = x.ContactId,
                            Name = x.Name,
                            CategoryIdName = x.categoryName,
                            CategoryId = x.CategoryId,
                            DesignationId = x.DesignationId,
                            DesignationIdName = x.Designation,
                            DepartmentId = x.DepartmentId,
                            CompanyId = x.CompanyId,
                            CompanyIdName = x.companyName,
                            Email = x.Email,
                            MobileNo = x.MobileNo,

                        }).ToList();


                    }


                    return res;
                }


            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion
        //-------------NIU API End--------------
        #region GetContactDesignationList
        public List<ContactsDetailView> GetContactsDesignationList(ContactMultiFilterDTO ContactMultiFilterDTO)
        {
            try
            {
                using (var db = new IDDbContext())
                {
                    List<ContactsDetailView> res1 = new List<ContactsDetailView>();
                    List<ContactsDetailView> res2 = new List<ContactsDetailView>();
                    if (ContactMultiFilterDTO.ContactDesignation != null)
                    {

                        //ContactkFilterDTO.ContactDesignation = ContactkFilterDTO.ContactDesignation.TrimEnd(',');
                        //List<string> ContactDesignationList = ContactMultiFilterDTO.ContactDesignation.Split(",").ToList();
                        List<string> ContactCategoryList = ContactMultiFilterDTO.ContactDesignation.Split(",").ToList();

                        if (ContactCategoryList != null && ContactCategoryList.Count > 0)
                        {
                            foreach (var item in ContactCategoryList)
                            {
                                if (!string.IsNullOrEmpty(item))
                                {
                                    int? CategoryId = Convert.ToInt32(item);
                                    res1 = db.ContactsDetailView.Where(a => a.CategoryId == CategoryId).ToList();
                                    foreach (var item1 in res1)
                                    {
                                        item1.ParticipantName = item1.ParticipantName + " " + "( " + item1.Designation + ")";
                                        //item1.ParticipantName = item1.ParticipantName + " (" + res1.Select(x => x.Designation).FirstOrDefault() + ")";

                                        item1.ParticipantId = item1.ParticipantId;
                                        //old way item1.FullName = item1.FullName + " ( " + item + " )";
                                    }
                                    res2.AddRange(res1);
                                }
                            }
                        }


                        ////////Old Way  
                        /////  List<string> ContactDesignationList = ContactkFilterDTO.ContactDesignation.Contains(',') ? ContactkFilterDTO.ContactDesignation.Split(",").ToList() : null;
                        //if (ContactDesignationList != null && ContactDesignationList.Count > 0)
                        //{
                        //    foreach (var item in ContactDesignationList)
                        //    {
                        //        if (!string.IsNullOrEmpty(item))
                        //        {
                        //            res1 = db.AspNetUsers.Where(a => a.IsRecordStatus == true && a.IsDeleted == false && a.Designation.ToUpper().Contains(item.ToUpper())).ToList();
                        //            foreach (var item1 in res1)
                        //            {
                        //                item1.FullName = item1.FullName + " ( " + item + " )";
                        //            }
                        //            res2.AddRange(res1);
                        //        }
                        //    }
                        //}
                        /// old way end

                        //res = res.Where(x => x.AssignTo == TaskFilterDTO.userList).ToList();
                    }
                    return res2;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region GetContactWithTiers
        public dynamic GetContactWithTiers(ContactsFilterDTO ContactkFilterDTO)
        {
            dynamic Data = null;
            try
            {
                using (var db = new IDDbContext())
                {
                    try
                    {
                        if (ContactkFilterDTO != null)
                        {
                            switch (ContactkFilterDTO.Type.ToUpper())
                            {
                                case "COMPANY":
                                    Data = GetContactCompanyList();
                                    break;
                                case "DEPARTMENT":
                                    Data = GetContactDepartmentList(ContactkFilterDTO.Id);
                                    break;
                                case "CATEGORY":
                                    Data = GetContactCategoryList(ContactkFilterDTO.Id);
                                    break;
                                //case "DESIGNATION":
                                //    Data = GetContactDesignation(ContactkFilterDTO.Id);
                                //    break;
                                case "CONTACTS":
                                    Data = GetContactListByDesignation(ContactkFilterDTO.Id);
                                    break;
                                case "Default":
                                    Data = null;
                                    break;
                            }
                        }
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                    return Data;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion



        #region GetContactWithMultiTiers
        public dynamic GetContactWithMultiTiers(ContactMultiListDTO ContactkFilterDTO)
        {
            dynamic Data = null;
            try
            {
                using (var db = new IDDbContext())
                {
                    try
                    {
                        List<ListContactDTO> AddRangeList = new List<ListContactDTO>();
                        if (ContactkFilterDTO != null)
                        {
                            switch (ContactkFilterDTO.Type.ToUpper())
                            {
                                case "COMPANY":
                                    Data = GetContactCompanyList();
                                    break;
                                case "DEPARTMENT":
                                    Data = GetContactDepartmentList(0, "Multi", ContactkFilterDTO.Filter);
                                    break;
                                case "CATEGORY":
                                    Data = GetContactCategoryList(0, "Multi", ContactkFilterDTO.Filter);
                                    break;
                                //case "DESIGNATION":
                                //    Data = GetContactDesignation(ContactkFilterDTO.Id);
                                //    break;
                                case "CONTACTS":
                                    Data = GetContactListByDesignation(0, "Multi", ContactkFilterDTO.Filter);
                                    break;
                                case "Default":
                                    Data = null;
                                    break;
                            }
                        }
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                    return Data;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        //-------------NIU API Start--------------
        #region GetContactMultiListByDesignation
        public List<ListContactDTO> GetContactMultiListByDesignation(List<ContactsFilterDTO> contacts)
        {
            try
            {

                List<ListContactDTO> AddRangeList = new List<ListContactDTO>();
                List<ListContactDTO> res = new List<ListContactDTO>();
                using (var db = new IDDbContext())
                {

                    foreach (var item in contacts)
                    {
                        res = db.ContactsDetailView.Where(x => x.DesignationId == item.Id).Select(x => new ListContactDTO
                        {

                            ParticipantId = x.ParticipantId,
                            ParticipantName = x.ParticipantName,


                        }).ToList();
                        AddRangeList.AddRange(res);
                    }

                }
                return AddRangeList;

            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion
        //-------------NIU API End--------------

    }
}




public class CommonClass
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string NameDisplay { get; set; }
    public string NameCompany { get; set; }
    public int? CompanyId { get; set; }
    public int? departmentId { get; set; }
    public bool RecordStatus { get; set; }

}
public class ContactsFilterDTO
{

    public int Id { get; set; }
    public string Type { get; set; }

}

public class ContactMultiListDTO
{
    public string Type { get; set; }
    public List<ContactsFilterDTO> Filter { get; set; }


}
