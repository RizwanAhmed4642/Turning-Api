using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Meeting_App.Models.DTOs.AdminWing
{
    public class ProcurementDetaislDTO
    {
        public ProcurementDetaislDTO()
        {
            proformalist = new List<proformalistDTO>();
        }

        public List<proformalistDTO> proformalist { get; set; }
    }
        public class proformalistDTO
        {
            public int? Proforma_Id { get; set; }
            public int? Proforma_Type_Id { get; set; }
            public string? Proforma_Type_Name { get; set; }
            public int? Wing_Id { get; set; }
            public string? Wing_Name { get; set; }
            public string? Scheme_ID { get; set; }
            public string? Scheme_Name { get; set; }
            public string? ItemName { get; set; }
            public int? ItemQuantity { get; set; }
            public int? Status_Award_Id { get; set; }
            public string? Status_Award_Name { get; set; }
            public int? Proc_Status_Id { get; set; }
            public string? Proc_Status_Name { get; set; }
            public string? Technical_Firm_Id { get; set; }
            public string? Technical_Firm_Name { get; set; }
            public string? Procurement_Amount { get; set; }
            public DateTime? Procurement_Contract_Date { get; set; }
            public DateTime? Technical_Evaluation_Date { get; set; }
            public string? Financial_Evaluation_Id { get; set; }
            public string? Advance_Acceptance_Issued { get; set; }
            public string? Performance_Guarantee_Amount { get; set; }
            public string? Outsourcing_Type_Id { get; set; }
            public string? Outsourcing_Type_Name { get; set; }
            public string? Service_Provider_Id { get; set; }
            public string? Service_Provider_Name { get; set; }
            public DateTime? Contract_Start_Date { get; set; }
            public DateTime? Contract_End_Date { get; set; }
            public string? Scope_Contract_Id { get; set; }
            public string? Scope_Contract_Name { get; set; }
            public string? Coverage_Id { get; set; }
            public string? Financial_Model_Id { get; set; }
            public string? Financial_Model_Name { get; set; }
            public string? Technical_Eval_Firm { get; set; }
            public float? Total_Contract_Amount { get; set; }
            public string? Payment_Made { get; set; }
            public string? Pending_Liabilities { get; set; }
            public string? Entered_By { get; set; }
            public DateTime? Entered_Date { get; set; }
            public string? Updated_By { get; set; }
            public DateTime? Updated_Date { get; set; }
            public Boolean IsActive { get; set; }
            public string? User_Id { get; set; }
            public string? UserName { get; set; }
            public string? UserDetail { get; set; }
            public string? Dist_District_Id { get; set; }
            public DateTime? Dist_Date_Of_Moving { get; set; }
            public DateTime? Dist_Date_Approval { get; set; }
            public string? Bids_Received { get; set; }
            public DateTime? Request_Proposal_Date { get; set; }
            public string? Firm_Awarded_Contract { get; set; }
            public int? Firms_Qualified { get; set; }
            public Boolean Performance_Guarantee_Verified { get; set; }
            public string? Contract_Scope { get; set; }
            public string? Remarks { get; set; }
            public string? Name { get; set; }
            public string? Proj_Name { get; set; }
            public int? Scheme_Type { get; set; }
            public string? SpecifyCoverage { get; set; }
            public string? Coverage { get; set; }
            public DateTime? Date_Of_Delivery { get; set; }
            public DateTime? Final_Inspection_Date { get; set; }
        }
    
}
