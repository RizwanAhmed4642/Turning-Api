using Meeting_App.Constants;
using Meeting_App.Models.DTOs;
using Meeting_App.Models.DTOs.AdminWing;
using Meeting_App.Service;
using Meeting_App.Service.Common;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Meeting_App.Models.DTOs.AdminWing.ProcurementDetaislDTO;

namespace Meeting_App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminWingController : Controller
    {


        #region Constructor
        public AdminWingController()
        {

        }
        #endregion

        
        [HttpPost]
        [Route("GetPendency")]
        public async Task<ActionResult> GetPendency([FromBody] ParameterDTO parameterDTO)
        {
            try
            {
                var obj = await new AdminWingService(PortalsURLs.BASE_HRMIS_LOCAL).GetpendancyData("DashboardHrmis/DashboardPendency3", parameterDTO);
                PendancyResponseDTO pendencyDTO  = JsonConvert.DeserializeObject<PendancyResponseDTO>(obj);

                return Ok(UtilService.GetResponse<PendancyResponseDTO>(pendencyDTO));
            }
            catch (Exception ex)
            {
                return BadRequest(UtilService.GetExResponse<Exception>(ex));
            }

        }

        [HttpGet]
        [Route("GetEmpOnLeaveSum")]
        public async Task<ActionResult> GetEmpOnLeaveSum()
        {
            try
            {
                var obj = await new AdminWingService(PortalsURLs.BASE_HRMIS_LOCAL).GetEmpOnLeaveSum("Public/GetEmpOnLeaveSum");
                List<EmpOnLeaveSumDTO> getEmpOnLeaveSumDTO = JsonConvert.DeserializeObject<List<EmpOnLeaveSumDTO>>(obj);

                return Ok(UtilService.GetResponse<List<EmpOnLeaveSumDTO>>(getEmpOnLeaveSumDTO));
            }
            catch (Exception ex)
            {
                return BadRequest(UtilService.GetExResponse<Exception>(ex));
            }

        }

        [HttpPost]
        [Route("GetEmployeesOnLeave")]
        public async Task<ActionResult> GetEmployeesOnLeave([FromBody] PaginationDTO parameterDTO)
        {
            try
            {
                var obj = await new AdminWingService(PortalsURLs.BASE_HRMIS_LOCAL).GetEmployeesOnLeave("Public/GetEmployeesOnLeave", parameterDTO);
                GetEmployeesOnLeaveDTOResponseDTO getEmployeesOnLeaveDTO = JsonConvert.DeserializeObject<GetEmployeesOnLeaveDTOResponseDTO>(obj);

                return Ok(UtilService.GetResponse<GetEmployeesOnLeaveDTOResponseDTO>(getEmployeesOnLeaveDTO));
            }
            catch (Exception ex)
            {
                return BadRequest(UtilService.GetExResponse<Exception>(ex));
            }

        }

        [HttpGet]
        [Route("GetEmpLeaveExpSum")]
        public async Task<ActionResult> GetEmpLeaveExpSum()
        {
            try
            {
                var obj = await new AdminWingService(PortalsURLs.BASE_HRMIS_LOCAL).GetEmpLeaveExpSum("Public/GetEmpLeaveExpSum");
                List<EmpOnLeaveSumDTO> getEmpOnLeaveSumDTO = JsonConvert.DeserializeObject<List<EmpOnLeaveSumDTO>>(obj);

                return Ok(UtilService.GetResponse<List<EmpOnLeaveSumDTO>>(getEmpOnLeaveSumDTO));
            }
            catch (Exception ex)
            {
                return BadRequest(UtilService.GetExResponse<Exception>(ex));
            }

        }

        [HttpPost]
        [Route("GetEmployeesLeaveExpired")]
        public async Task<ActionResult> GetEmployeesLeaveExpired([FromBody] PaginationDTO parameterDTO)
        {
            try
            {
                var obj = await new AdminWingService(PortalsURLs.BASE_HRMIS_LOCAL).GetEmployeesLeaveExpired("Public/GetEmployeesLeaveExpired", parameterDTO);
                GetEmployeesLeaveExpiredresponseDTO getEmployeesOnLeaveDTO = JsonConvert.DeserializeObject<GetEmployeesLeaveExpiredresponseDTO>(obj);

                return Ok(UtilService.GetResponse<GetEmployeesLeaveExpiredresponseDTO>(getEmployeesOnLeaveDTO));
            }
            catch (Exception ex)
            {
                return BadRequest(UtilService.GetExResponse<Exception>(ex));
            }

        }



        [HttpPost]
        [Route("GetLeavesExpired")]
        public async Task<ActionResult> GetLeavesExpired()
        {
            try
            {
                var obj = await new AdminWingService(PortalsURLs.BASE_HRMIS_LOCAL).GetLeavesExpired("Public/GetLeavesExpired");
                List<LeavesExpiredDTO> getLeavesExpiredDTO = JsonConvert.DeserializeObject<List<LeavesExpiredDTO>>(obj);

                return Ok(UtilService.GetResponse<List<LeavesExpiredDTO>>(getLeavesExpiredDTO));
            }
            catch (Exception ex)
            {
                return BadRequest(UtilService.GetExResponse<Exception>(ex));
            }

        }


        [HttpGet]
        [Route("AwaitingPostingSum")]
        public async Task<ActionResult> AwaitingPostingSum()
        {
            try
            {
                var obj = await new AdminWingService(PortalsURLs.BASE_HRMIS_LOCAL).AwaitingPostingSum("Public/AwaitingPostingSum");
                List<AwaitingpostingSumDTO> getAwaitingPostingSumDTO = JsonConvert.DeserializeObject<List<AwaitingpostingSumDTO>>(obj);

                return Ok(UtilService.GetResponse<List<AwaitingpostingSumDTO>>(getAwaitingPostingSumDTO));
            }
            catch (Exception ex)
            {
                return BadRequest(UtilService.GetExResponse<Exception>(ex));
            }

        }

        [HttpPost]
        [Route("GetVpByHF/{HFCode}")]
        public async Task<ActionResult> GetVpByHF(int HFCode)
        {
            try
            {
                var obj = await new AdminWingService(PortalsURLs.BASE_HRMIS_LOCAL).AwaitingPostingSum("Public/GetVpByHF/" + HFCode);
                List<VPDTO> GetVpByHF = JsonConvert.DeserializeObject<List<VPDTO>>(obj);

                return Ok(UtilService.GetResponse<List<VPDTO>>(GetVpByHF));
            }
            catch (Exception ex)
            {
                return BadRequest(UtilService.GetExResponse<Exception>(ex));
            }

        }

        [HttpPost]
        [Route("GetVp/{Code}")]
        public async Task<ActionResult> GetVp(int Code)
        {
            try
            {
                var obj = await new AdminWingService(PortalsURLs.BASE_HRMIS_LOCAL).GetVp("Public/GetVp/"+Code);
                List<VPDTO> getVPDTO = JsonConvert.DeserializeObject<List<VPDTO>>(obj);

                return Ok(UtilService.GetResponse<List<VPDTO>>(getVPDTO));
            }
            catch (Exception ex)
            {
                return BadRequest(UtilService.GetExResponse<Exception>(ex));
            }

        }

        [HttpPost]
        [Route("GetAwaitingPostingApps")]
        public async Task<ActionResult> GetAwaitingPostingApps([FromBody] PaginationDTO parameterDTO)
        {
            try
            {
                var obj = await new AdminWingService(PortalsURLs.BASE_HRMIS_LOCAL).GetAwaitingPostingApps("Public/GetAwaitingPostingApps", parameterDTO);
                GetAwaitingPostingAppsDTOResponseDTO getAwaitingPostingAppsDTO = JsonConvert.DeserializeObject<GetAwaitingPostingAppsDTOResponseDTO>(obj);

                return Ok(UtilService.GetResponse<GetAwaitingPostingAppsDTOResponseDTO>(getAwaitingPostingAppsDTO));
            }
            catch (Exception ex)
            {
                return BadRequest(UtilService.GetExResponse<Exception>(ex));
            }

        }

        [HttpPost]
        [Route("GetCrrSummary")]
        public async Task<ActionResult> GetCrrSummary([FromBody] ParameterDTO parameterDTO)
        {
            try
            {
                var obj = await new AdminWingService(PortalsURLs.BASE_HRMIS_LOCAL).GetCrrSummary("Public/GetCrrSummary", parameterDTO);
                List<GetCrrSummary> getCrrSummaries = JsonConvert.DeserializeObject<List<GetCrrSummary>>(obj);

                return Ok(UtilService.GetResponse<List<GetCrrSummary>>(getCrrSummaries));
            }
            catch (Exception ex)
            {
                return BadRequest(UtilService.GetExResponse<Exception>(ex));
            }

        }

        [HttpGet]
        [Route("VPReport")]
        public async Task<ActionResult> VPReport()
        {
            try
            {
                var obj = await new AdminWingService(PortalsURLs.BASE_HRMIS_LOCAL).VPReport("Public/VPReport/0");
                List<VPReportDTO> vpDto = JsonConvert.DeserializeObject<List< VPReportDTO >> (obj);

                vpDto = vpDto.OrderByDescending(a => a.BPS).ToList();

                return Ok(UtilService.GetResponse<List<VPReportDTO>>(vpDto));
            }
            catch (Exception ex)
            {
                return BadRequest(UtilService.GetExResponse<Exception>(ex));
            }

        }

        [HttpGet]
        [Route("GetProcurementCountByType")]
        public async Task<ActionResult> GetProcurementCountByType()
        {
            try
            {
                var obj = await new AdminWingService(PortalsURLs.BASE_PDIS_LOCAL).GetProcurementCountByType("Dashboard/GetProcurementCountByType");
                GetProcurementTypeDTO vpDto = JsonConvert.DeserializeObject<GetProcurementTypeDTO>(obj);

                return Ok(UtilService.GetResponse<GetProcurementTypeDTO>(vpDto));
            }
            catch (Exception ex)
            {
                return BadRequest(UtilService.GetExResponse<Exception>(ex));
            }

        }

        [HttpGet]
        [Route("GetDistrictWiseProcurementCount")]
        public async Task<ActionResult> GetDistrictWiseProcurementCount()
        {
            try
            {
                var obj = await new AdminWingService(PortalsURLs.BASE_PDIS_LOCAL).GetDistrictWiseProcurementCount("Dashboard/GetDistrictWiseProcurementCount");
                List<DistrictWiseProcurementDTO> vpDto = JsonConvert.DeserializeObject<List<DistrictWiseProcurementDTO>>(obj);

                return Ok(UtilService.GetResponse<List<DistrictWiseProcurementDTO>>(vpDto));
            }
            catch (Exception ex)
            {
                return BadRequest(UtilService.GetExResponse<Exception>(ex));
            }

        }


        [HttpPost]
        [Route("GetProcurementDetails")]
        public async Task<ActionResult> GetProcurementDetails([FromBody] ProcurementDetailDTO procurementDetailDTO)
        {
            try
            {
                var obj = await new AdminWingService(PortalsURLs.BASE_PDIS_LOCAL).GetProcurementDetail("Dashboard/GetProformaListDetail/" + procurementDetailDTO.userid+"/"+procurementDetailDTO.Perfomatytpe + "/" + procurementDetailDTO.type);

                List<proformalistDTO> getCrrSummaries = JsonConvert.DeserializeObject<List<proformalistDTO>>(obj);

                return Ok(UtilService.GetResponse<List<proformalistDTO>>(getCrrSummaries));
            }
            catch (Exception ex)
            {
                return BadRequest(UtilService.GetExResponse<Exception>(ex));
            }

        }

        [HttpGet]
        [Route("GetAdpSchemes/{Scheme_Type}")]
        public async Task<ActionResult> GetAdpSchemes(int Scheme_Type)
        {
            try
            {
                var obj = await new AdminWingService(PortalsURLs.BASE_PDIS_LOCAL).GetAdpSchemes("Scheme/GetAdpSchemes/"+Scheme_Type);
                List<AdpSchemeDTO> vpDto = JsonConvert.DeserializeObject<List<AdpSchemeDTO>>(obj);

                return Ok(UtilService.GetResponse<List<AdpSchemeDTO>>(vpDto));
            }
            catch (Exception ex)
            {
                return BadRequest(UtilService.GetExResponse<Exception>(ex));
            }

        }


        [HttpGet]
        [Route("GetHCReport")]
        public async Task<ActionResult> GetHCReport()
        {
            try
            {
                var obj = await new AdminWingService(PortalsURLs.BASE_HealthCouncil).GetHCReport("HCReport");
                List<HCDTO> vpDto = JsonConvert.DeserializeObject<List<HCDTO>>(obj);

                return Ok(UtilService.GetResponse<List<HCDTO>>(vpDto));
            }
            catch (Exception ex)
            {
                return BadRequest(UtilService.GetExResponse<Exception>(ex));
            }

        }



        [HttpGet]
        [Route("GetAdpSchemesCount")]
        public async Task<ActionResult> GetAdpSchemesCount()
        {
            try
            {
                var obj = await new AdminWingService(PortalsURLs.BASE_PDIS_LOCAL).GetAdpSchemesCount("Scheme/GetTotalSchemes");
                List<AdpSchemesCount> vpDto = JsonConvert.DeserializeObject<List<AdpSchemesCount>>(obj);

                return Ok(UtilService.GetResponse<List<AdpSchemesCount>>(vpDto));
            }
            catch (Exception ex)
            {
                return BadRequest(UtilService.GetExResponse<Exception>(ex));
            }

        }


        [HttpGet]
        [Route("GetHepatitasPatientCounts")]
        public async Task<ActionResult> GetHepatitasPatientCounts()
        {
            try
            {
                var obj = await new AdminWingService(PortalsURLs.BASE_Hepatitas_LOCAL).GetHepatitasPatientCounts("Dashboard_counts_api/GetPatientsData");
                HepatitisresponseData hepatitisData = JsonConvert.DeserializeObject<HepatitisresponseData>(obj);

                HepatitisData HepatitisDataObj = new HepatitisData();
                HepatitisDataObj.total_new_assessment_count = hepatitisData.HepatitisData.Sum(x => x.total_new_assessment_count);
                HepatitisDataObj.total_new_patient_count= hepatitisData.HepatitisData.Sum(x => x.total_new_patient_count);
                HepatitisDataObj.total_pre_diagnosed_count=  hepatitisData.HepatitisData.Sum(x => x.total_pre_diagnosed_count);
                HepatitisDataObj.total_registration_count= hepatitisData.HepatitisData.Sum(x => x.total_registration_count);

                HepatitisresponseData obj1 = new HepatitisresponseData();
                obj1.HepatitisData.Add ( HepatitisDataObj);

                //return Ok(hepatitisData);
                return Ok(UtilService.GetResponse<HepatitisresponseData>(obj1));
            }
            catch (Exception ex)
            {
                return BadRequest(UtilService.GetExResponse<Exception>(ex));
            }

        }


        [HttpGet]
        [Route("GetDRSDailyLabCount")]
        public async Task<ActionResult> GetDRSDailyLabCount()
        {
            try
            {
                var obj = await new AdminWingService(PortalsURLs.BASE_Aids_LOCAL).GetDRSDailyLabCount("GetDRSCountDailyLab");
                List<DRSDailyCount> vpDto = JsonConvert.DeserializeObject<List<DRSDailyCount>>(obj);

                return Ok(UtilService.GetResponse<List<DRSDailyCount>>(vpDto));
            }
            catch (Exception ex)
            {
                return BadRequest(UtilService.GetExResponse<Exception>(ex));
            }

        }

        [HttpGet]
        [Route("GetDRSDailyPatientCount")]
        public async Task<ActionResult> GetDRSDailyPatientCount()
        {
            try
            {
                var obj = await new AdminWingService(PortalsURLs.BASE_Aids_LOCAL).GetDRSDailyPatientCount("GetDRSCountDailyPatient");
                List<DRSDailyPatientCount> vpDto = JsonConvert.DeserializeObject<List<DRSDailyPatientCount>>(obj);

                return Ok(UtilService.GetResponse<List<DRSDailyPatientCount>>(vpDto));
            }
            catch (Exception ex)
            {
                return BadRequest(UtilService.GetExResponse<Exception>(ex));
            }

        }



        [HttpPost]
        [Route("GetIRMNCHPatientsDistrictWise")]
        public async Task<ActionResult> GetIRMNCHPatientsDistrictWise([FromBody] ParameterDTO parameterDTO)
        {
            try
            {
                var obj = await new AdminWingService(PortalsURLs.BASE_IRMNCH_LOCAL).GetIRMNCHPatientsDistrictWise("GetDistWisePatientsSummary", parameterDTO);
                List<IRMNCHPatientsDistrictWise> pendencyDTO = JsonConvert.DeserializeObject<List<IRMNCHPatientsDistrictWise>>(obj);

                return Ok(UtilService.GetResponse<List<IRMNCHPatientsDistrictWise>>(pendencyDTO));
            }
            catch (Exception ex)
            {
                return BadRequest(UtilService.GetExResponse<Exception>(ex));
            }

        }


        [HttpGet]
        [Route("GetNCDDailyReport")]
        public async Task<ActionResult> GetNCDDailyReport()
        {
            try
            {
                var obj = await new AdminWingService(PortalsURLs.BASE_NCD_LOCAL).GetNCDDailyReport("GetDailyReport");
                Lora vpDto = JsonConvert.DeserializeObject<Lora>(obj);

                return Ok(UtilService.GetResponse<Lora>(vpDto));
            }
            catch (Exception ex)
            {
                return BadRequest(UtilService.GetExResponse<Exception>(ex));
            }

        }


        [HttpPost]
        [Route("GetLHSReportingDistWise")]
        public async Task<ActionResult> GetLHSReportingDistWise([FromBody] ParameterDTO parameterDTO)
        {
            try
            {
                var obj = await new AdminWingService(PortalsURLs.BASE_IRMNCH_LOCAL).GetLHSReportingDistWise("GetLHSReportingDistWise", parameterDTO);
                List<DistrictWiseLHWMonthlyCount> pendencyDTO = JsonConvert.DeserializeObject<List<DistrictWiseLHWMonthlyCount>>(obj);

                return Ok(UtilService.GetResponse<List<DistrictWiseLHWMonthlyCount>>(pendencyDTO));
            }
            catch (Exception ex)
            {
                return BadRequest(UtilService.GetExResponse<Exception>(ex));
            }

        }

        [HttpPost]
        [Route("GetIrmnchEmrDsr")]
        public async Task<ActionResult> GetIrmnchEmrDsr([FromBody] ParameterDTO parameterDTO)
        {
            try
            {
                var obj = await new AdminWingService(PortalsURLs.BASE_IRMNCH_LOCAL).GetIrmnchEmrDsr("GetDSRCompliance", parameterDTO);
                List<DSRComplianceDistrict> pendencyDTO = JsonConvert.DeserializeObject<List<DSRComplianceDistrict>>(obj);

                return Ok(UtilService.GetResponse<List<DSRComplianceDistrict>>(pendencyDTO));
            }
            catch (Exception ex)
            {
                return BadRequest(UtilService.GetExResponse<Exception>(ex));
            }

        }

        [HttpGet]
        [Route("GetSummries")]
        public async Task<ActionResult> GetSummries()
        {
            try
            {
                var obj = await new AdminWingService(PortalsURLs.BASE_TOSAN_LOCAL_DATA_LIST).GetSummries(PortalsURLs.BASE_TOSAN_LOCAL_DATA_LIST);
                List<SummariesAndNotes> pendencyDTO = JsonConvert.DeserializeObject<List<SummariesAndNotes>>(obj);

                return Ok(UtilService.GetResponse<List<SummariesAndNotes>>(pendencyDTO));
            }
            catch (Exception ex)
            {
                return BadRequest(UtilService.GetExResponse<Exception>(ex));
            }

        }
        [HttpGet]
        [Route("GetSummriesDetail/{id}")]
        public async Task<ActionResult> GetSummriesDetail(int id)
        {
            try
            {
                var obj = await new AdminWingService(PortalsURLs.BASE_TOSAN_LOCAL_DATA_LIST_BY_ID).GetSummriesDetail(PortalsURLs.BASE_TOSAN_LOCAL_DATA_LIST_BY_ID+id);
                List<SummariesAndNotesDetail> pendencyDTO = JsonConvert.DeserializeObject<List<SummariesAndNotesDetail>>(obj);

                return Ok(UtilService.GetResponse<List<SummariesAndNotesDetail>>(pendencyDTO));
            }
            catch (Exception ex)
            {
                return BadRequest(UtilService.GetExResponse<Exception>(ex));
            }

        }
        [HttpGet]
        [Route("GetSummriesAttachment/{id}")]
        public async Task<ActionResult> GetSummriesAttachment(int id)
        {
            try
            {
                var obj = await new AdminWingService(PortalsURLs.BASE_TOSAN_LOCAL_DATA_LIST_BY_ID).GetSummriesAttachment(PortalsURLs.BASE_TOSAN_LOCAL_DATA + id);
                //List<SummariesAndNotesAttachment> pendencyDTO = JsonConvert.DeserializeObject<List<SummariesAndNotesAttachment>>(obj);

                return Ok(obj);
            }
            catch (Exception ex)
            {
                return BadRequest(UtilService.GetExResponse<Exception>(ex));
            }

        }
        [HttpPost]
        [Route("GetDSRReport")]
        public async Task<ActionResult> GetDSRReport()
        {
            try
            {
                var obj = await new AdminWingService(PortalsURLs.BASE_CDSL_LOCAL).GetDSRReport("DSLService.asmx/GetDSRReport");
                
                obj = obj.Replace("<string xmlns=\"http://tempuri.org/\">", "");
                obj = obj.Replace("</string>", "");
                obj = obj.Replace("<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n", "");

                // EXAMPLE 1

                //List<APIData> pendencyDTO = ser.Deserialize<List<APIData>>(obj);
                //xmlOutputData = ser.Serialize<Customer>(customer)



                apidata pendencyDTO = JsonConvert.DeserializeObject<apidata>(obj);

                return Ok(UtilService.GetResponse<apidata>(pendencyDTO));
            }
            catch (Exception ex)
            {
                return BadRequest(UtilService.GetExResponse<Exception>(ex));
            }

        }
    }
}
