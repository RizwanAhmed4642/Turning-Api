using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Meeting_App.Models.DTOs
{
    public class ContactDTO
    {
        public int? Id { get; set; }
        public string Title { get; set; }
        public string Name { get; set; }
        public string Designation { get; set; }
        public string Department { get; set; }
        public bool? RecordStatus { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreationDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdationDate { get; set; }
        public string Email { get; set; }
        public string MobileNo { get; set; }
        public string PhoneNo { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string CNIC { get; set; }
        public string Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string ResourceOFContact { get; set; }
        public string District { get; set; }
        public string Division { get; set; }
        public string Tehsil { get; set; }
        public IFormFile ProfilePicAttchment { get; set; }
        public int? DesignationId { get; set; }
        public int? CategoryId { get; set; }
        public int? DepartmentId { get; set; }
        public int? CompanyId { get; set; }
    }




    public class ContactFilterDTO
    {
        public Guid? userList { get; set; }
        public string Priority { get; set; }
        public string Division { get; set; }
        public string Status { get; set; }
        public string ContactDesignation { get; set; }
        public string ContactDesignationIds { get; set; }

    }

    public class ContactAllFilterDTO
    {
        public Guid? userList { get; set; }
        public int? ContactCompanyIds { get; set; }
        public int? ContactDepartmentIds { get; set; }
        public int? ContactCategoryIds { get; set; }
        public int? ContactDesignationIds { get; set; }
    }
    public class ContactTypeDTO
    {
        public int Id { get; set; }
        public string Designation { get; set; }
        public int? OderBy { get; set; }
        public bool? RecordStatus { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreationDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdationDate { get; set; }

        public int? DepartmentId { get; set; }
        public int? CategoryId { get; set; }
    }

    //-----------------Multi Dropdown --------------
    public class ContactMultiFilterDTO
    {
      
        public string ContactCompany { get; set; }
        public string ContactCompanyIds { get; set; }
        public string ContactDepartment { get; set; }
        public string ContactDepartmentIds { get; set; }
        public string ContactCategory { get; set; }
        public string ContactCategoryIds { get; set; }
        public string ContactDesignation { get; set; }
        public string ContactDesignationIds { get; set; }

    }
    //-----------------Multi Dropdown --------------

}
