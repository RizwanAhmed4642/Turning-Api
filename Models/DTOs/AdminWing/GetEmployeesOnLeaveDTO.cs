using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Meeting_App.Models.DTOs.AdminWing
{
    public class GetEmployeesOnLeaveDTOResponseDTO
    {
        public GetEmployeesOnLeaveDTOResponseDTO()
        {
            List = new List<GetEmployeesOnLeaveDTO>();
        }

        public List<GetEmployeesOnLeaveDTO> List { get; set; }


    }
    public class GetEmployeesOnLeaveDTO
    {
         public int Id { get; set; }
         public string Barcod { get; set; }
         public string  FileNumber { get; set; }
        public string  EmployeeName { get; set; }
        public string  FatherName { get; set; }
        public string  CNIC { get; set; }
        public string  Designation_Name { get; set; }
        public string  LeaveTypeName { get; set; }
        public string  FromDate { get; set; }
        public DateTime?  ToDate { get; set; }
        public int ? DateDiff { get; set; }
    }
}
