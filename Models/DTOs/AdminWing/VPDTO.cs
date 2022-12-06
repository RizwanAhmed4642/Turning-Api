using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Meeting_App.Models.DTOs.AdminWing
{
    public class VPDTO
    {
        public int? Id { get; set; }
        public int? TotalSanctioned { get; set; }
        public int? TotalWorking { get; set; }
        public int? Vacant { get; set; }
        public int? TotalApprovals { get; set; }
        public int? Locked { get; set; }
        public int? Profiles { get; set; }
        public int? WorkingProfiles { get; set; }
        public string HFMISCode { get; set; }
        public int? HF_Id { get; set; }
        public string DivisionCode { get; set; }
        public string DivisionName { get; set; }
        public string DistrictCode { get; set; }
        public string DistrictName { get; set; }
        public string TehsilCode { get; set; }
        public string TehsilName { get; set; }
        public string HFName { get; set; }
        public int? HFAC { get; set; }
        public int? PostType_Id { get; set; }
        public string PostTypeName { get; set; }
        public int? Desg_Id { get; set; }
        public string DsgName { get; set; }
        public int? BPSWorking { get; set; }
        public int?Cadre_Id { get; set; }
        public string CadreName { get; set; }
        public int? BPS { get; set; }
        public int? BPS2 { get; set; }
        public int? HFTypeId { get; set; }
        public string HFTypeCode { get; set; }
        public string HFTypeName { get; set; }
        public int? EntityLifeCycle_Id { get; set; }
        public string Created_By { get; set; }
        public DateTime Created_Date { get; set; }
        public string Last_Modified_By { get; set; }
        public string Users_Id { get; set; }
        public int? Adhoc { get; set; }
        public int? Contract { get; set; }
        public int? Regular { get; set; }
        public int? PHFMC { get; set; }
    }
}
