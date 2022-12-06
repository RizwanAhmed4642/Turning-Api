using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Meeting_App.Models.DTOs.AdminWing
{
    public class GetProcurementTypeDTO
    {
        public GetProcurementTypeDTO()
        {
            Data = new Data();
            ProcurementDetail = new List<ProcurementDetail>();
            OutsourcingDetail = new List<OutsourcingDetail>();
        }

        public Data Data { get; set; }

        public List<ProcurementDetail> ProcurementDetail { get; set; }
        public List<OutsourcingDetail> OutsourcingDetail { get;set;}
    }


    public class Data
    {
        public int ProformaTypeId { get; set; }
        public string ProformaTypeName { get; set; }
        public int TotalCount { get; set; }
        public int counts { get; set; }
        public string StatusName { get; set; }
        public int Pro_Count { get; set; }
        public int OutSource_Count { get; set; }
        public int District_Count { get; set; }
        public List<Pro_list> Pro_list { get; set; }

    }

    public class Pro_list
    {
        public int ProformaTypeId { get; set; }
        public string ProformaTypeName { get; set; }
        public int TotalCount { get; set; }
        public int counts { get; set; }
        public string StatusName { get; set; }
        public int Pro_Count { get; set; }
        public int OutSource_Count { get; set; }
        public int District_Count { get; set; }
    }
    public class ProcurementDetail
    {
        public string Id { get; set; }
        public string UserDetail { get; set; }
        public string Procurement_Total_Entries { get; set; }
        public string In_Process { get; set; }
        public string Not_Awarded { get; set; }
        public string Awarded { get; set; }
    }
    public class OutsourcingDetail
    {
        public string Id { get; set; }
        public string UserDetail { get; set; }
        public string OutSourcing_Procurement_Total_Entries { get; set; }
    }
}
