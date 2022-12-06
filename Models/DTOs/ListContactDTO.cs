using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Meeting_App.Models.DTOs
{
    public class ListContactDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public Guid? ParticipantId { get; set; }
        public string ParticipantName { get; set; }
        public string Name { get; set; }
        public string NameDisplay { get; set; }
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
        public string ProfilePic { get; set; }

        public int? DesignationId { get; set; }    
        public int? CategoryId { get; set; }    
        public int? DepartmentId { get; set; }    
        public int? CompanyId { get; set; }  
        
        public string DesignationIdName { get; set; }
        public string CategoryIdName { get; set; }
        public string  DepartmentIdName { get; set; }
        public string  CompanyIdName { get; set; }




    }
}
