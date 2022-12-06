using Meeting_App.Models.DTOs;
using Meeting_App.Service;
using Meeting_App.Service.Common;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;

namespace Meeting_App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DHISController : Controller
    {
        



        DHISService _dhisService = new DHISService();

        [HttpPost]
        [Route("GetDHISData")]
        public async Task<ActionResult> GetDHISData([FromBody] ParameterDTO parameterDTO)
        {
            try
            {
                string toDay = DateTime.UtcNow.AddHours(5).ToString("yyyyMMdd", CultureInfo.InvariantCulture);
                if (parameterDTO.From.HasValue)
                {
                    toDay = parameterDTO.From.Value.ToString("yyyyMMdd", CultureInfo.InvariantCulture);
                }

                 
                var obj = await  _dhisService.GetDHISData(toDay);
                DHISDTO dHISDTOs = JsonConvert.DeserializeObject<DHISDTO>(obj);

                return Ok(UtilService.GetResponse<DHISDTO>(dHISDTOs));
            }
            catch (Exception ex)
            {
                return BadRequest(UtilService.GetExResponse<Exception>(ex));
            }

        }
    }
}
