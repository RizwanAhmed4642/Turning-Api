using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Meeting_App.Models.DTOs.AdminWing
{
    public class IRMNCHPatientsDistrictWise
    {
       
            public long DistrictID { get; set; }
            public string DistrictName { get; set; }
            public int TotalPatients { get; set; }
            public int TotalVisits { get; set; }
            public long TotalDispenseMedicines { get; set; }
            public int TotalMother { get; set; }
            public int TotalChild { get; set; }
            public int TotalGP { get; set; }
            public int TotalMotherB { get; set; }
            public int TotalChildB { get; set; }

    }

    public class DistrictWiseLHWMonthlyCount
    {
        public string district { get; set; }
        public string DistrictCode { get; set; }
        public Nullable<int> TotalLHS { get; set; }
        public Nullable<int> TotalLHW { get; set; }
        public Nullable<int> TotalSubmited { get; set; }
        public Nullable<int> TotalPendingLHW { get; set; }
    }


    public class DSRComplianceDistrict
    {
        public string DistrictName { get; set; }
        public double TotalHF { get; set; }
        public double ReportsSubmitted { get; set; }
        public double TotalExpectedReports { get; set; }
        public double DistrictAvgCompliance { get; set; }

    }

    public class SummariesAndNotes
    {
        public int Id { get; set; }
        public int ApplicationSource_Id { get; set; }
        public int TrackingNumber { get; set; }
        public int? ApplicationType_Id { get; set; }
        public string IsSigned { get; set; }
        public string SignededAppAttachement_Id { get; set; }
        public string RemarksTime { get; set; }
        public int Status_Id { get; set; }
        public int StatusByOfficer_Id { get; set; }
        public string StatusByOfficerName { get; set; }
        public DateTime StatusTime { get; set; }
        public int ForwardingOfficer_Id { get; set; }
        public int FromOfficer_Id { get; set; }
        public string FromOfficerName { get; set; }
        public DateTime ForwardTime { get; set; }
        public string PandSOfficer_Id { get; set; }
        public string PandSOfficerName { get; set; }
        public bool IsPending { get; set; }
        public string RecieveTime { get; set; }
        public string FileRequested { get; set; }
        public string FileRequest_Id { get; set; }
        public string IsPersonAppeared { get; set; }
        public string PersonAppeared_Id { get; set; }
        public string RawText { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string TotalDays { get; set; }
        public string LeaveType_Id { get; set; }
        public string CurrentScale { get; set; }
        public string RetirementType_Id { get; set; }
        public string FromHF_Id { get; set; }
        public string ToHFCode { get; set; }
        public string ToHF_Id { get; set; }
        public string FromDept_Id { get; set; }
        public string ToDept_Id { get; set; }
        public string FromDesignation_Id { get; set; }
        public string ToDesignation_Id { get; set; }
        public string ToScale { get; set; }
        public string SeniorityNumber { get; set; }
        public string AdhocExpireDate { get; set; }
        public string ComplaintType_id { get; set; }
        public string DispatchNumber { get; set; }
        public DateTime DispatchDated { get; set; }
        public string DispatchFrom { get; set; }
        public string DispatchSubject { get; set; }
        public string Remarks { get; set; }
        public string EmployeeName { get; set; }
        public string FatherName { get; set; }
        public string CNIC { get; set; }
        public string DateOfBirth { get; set; }
        public string Gender { get; set; }
        public string Department_Id { get; set; }
        public string Designation_Id { get; set; }
        public string HealthFacility_Id { get; set; }
        public string HfmisCode { get; set; }
        public string FileNumber { get; set; }
        public string MobileNo { get; set; }
        public string EMaiL { get; set; }
        public string JoiningGradeBPS { get; set; }
        public string CurrentGradeBPS { get; set; }
        public string EmpMode_Id { get; set; }
        public string EmpStatus_Id { get; set; }
        public DateTime Created_Date { get; set; }
        public string Created_By { get; set; }
        public string Users_Id { get; set; }
        public bool IsActive { get; set; }
        public string Profile_Id { get; set; }
        public string ForwardingOfficerName { get; set; }
        public string OrderDate { get; set; }
        public int CurrentLog_Id { get; set; }
        public string FileRequestTime { get; set; }
        public string FileRequestStatus { get; set; }
        public string FileRequestStatus_Id { get; set; }
        public string FileUpdated_Id { get; set; }
        public string DDS_Id { get; set; }
        public string Reason { get; set; }
        public int ToOfficer_Id { get; set; }
        public string ToOfficerName { get; set; }
        public string VpMaster_Id { get; set; }
        public string VpHolder_Id { get; set; }
        public string MutualCNIC { get; set; }
        public string Mutual_Id { get; set; }
        public string Purpose_Id { get; set; }
        public string DueDate { get; set; }
        public string ApplicationSourceName { get; set; }
        public string ApplicationTypeName { get; set; }
        public string StatusName { get; set; }
        public string PersonName { get; set; }
        public string PersonReferrence { get; set; }
        public string PersonRelation { get; set; }
        public string PersonMobile { get; set; }
        public string PersonConstituency { get; set; }
        public string PersonDistrictCode { get; set; }
        public string PersonCNIC { get; set; }
        public string PandSOfficerCode { get; set; }
        public string Purpose { get; set; }
        public string DepartmentName { get; set; }
        public string designationName { get; set; }
        public string HealthFacilityName { get; set; }
        public string fromHealthFacility { get; set; }
        public string fromDepartmentName { get; set; }
        public string fromDesignationName { get; set; }
        public string toHealthFacility { get; set; }
        public string toDepartmentName { get; set; }
        public string toDesignationName { get; set; }
        public string leaveType { get; set; }
        public string EmpModeName { get; set; }
        public string EmpStatusName { get; set; }
        public int Limit { get; set; }
        public int DateDiff { get; set; }
    }

    public class SummariesAndNotesDetail
    {
        
       
        public int Id { get; set; }
        public int? Application_Id { get; set; }
        public int? TrackingNumber { get; set; }
        public string Remarks { get; set; }
        public int? Action_Id { get; set; }
        public string ActionName { get; set; }
        public string SMS_SentToApplicant { get; set; }
        public string SMS_SentToOfficer { get; set; }
        public int? FromStatus_Id { get; set; }
        public string FromStatusName { get; set; }
        public int? ToStatus_Id { get; set; }
        public string ToStatusName { get; set; }
        public int? StatusByOfficer_Id { get; set; }
        public string StatusByName { get; set; }
        public string StatusByDesignation { get; set; }
        public string StatusByProgram { get; set; }
        public bool? IsReceived { get; set; }
        public string ReceivedTime { get; set; }
        public int? FromOfficer_Id { get; set; }
        public string FromOfficerName { get; set; }
        public string FromOfficerDesignation { get; set; }
        public string FromOfficerProgram { get; set; }
        public int? ToOfficer_Id { get; set; }
        public string ToOfficerName { get; set; }
        public string ToOfficerDesignation { get; set; }
        public string ToOfficerProgram { get; set; }
        public string FileRequest_Id { get; set; }
        public string FileRequestStatusName { get; set; }
        public string FileRequestByName { get; set; }
        public string FileRequestByDesignation { get; set; }
        public string FileRequestByProgram { get; set; }
        public string afrLogUser_Id { get; set; }
        public string afrLogUserName { get; set; }
        public string afrLogDateTime { get; set; }
        public string FileReqLogStatusName { get; set; }
        public string afrLogByName { get; set; }
        public string afrLogByDesignation { get; set; }
        public string afrLogByProgram { get; set; }
        public string Purpose_Id { get; set; }
        public string Purpose { get; set; }
        public string DueDate { get; set; }
        public string RemarksByOfficer_Id { get; set; }
        public string RemarksByOfficer { get; set; }
        public DateTime DateTime { get; set; }
        public string User_Id { get; set; }
        public string CreatedBy { get; set; }
        public bool? IsActive { get; set; }
    }

    public class SummariesAndNotesAttachment
    {
        public SummariesAndNotesAttachment()
        {
            applicationAttachments = new List<SummariesAndNotesAttachmentDetail>();
        }

        public List<SummariesAndNotesAttachmentDetail> applicationAttachments { get; set; }

    }
    public class SummariesAndNotesAttachmentDetail
    {
       

        public string UploadPath { get; set; }
        public string DocName { get; set; }

    }



}
