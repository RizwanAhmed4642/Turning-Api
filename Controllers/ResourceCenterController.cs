using AutoMapper;
using Meeting_App.Data.Database.Tables;
using Meeting_App.Models;
using Meeting_App.Models.DTOs;
using Meeting_App.Service;
using Meeting_App.Service.Common;
using Meeting_App.SIgnalRHub;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Meeting_App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
   
    public class ResourceCenterController : Controller
    {

        ResourceCenterService _service = new ResourceCenterService();

        [Authorize]
        [HttpGet]
        [Route("GetFlagshipSummaries")]
        public async Task<IActionResult> GetFlagshipSummaries()
        {
            try
            {
                
                var data = _service.GetResourceFlagshipSummaries();

                return Ok(UtilService.GetResponse(data));
            }
            catch (Exception ex)
            {
                return Ok(UtilService.GetExResponse<Exception>(ex));
            }
        }
        [Authorize]
        [HttpGet]
        [Route("GetSummaries")]
        public async Task<IActionResult> GetSummaries()
        {
            try
            {

                var data = _service.GetResourceSummaries();

                return Ok(UtilService.GetResponse(data));
            }
            catch (Exception ex)
            {
                return Ok(UtilService.GetExResponse<Exception>(ex));
            }
        }
        [Authorize]
        [HttpGet]
        [Route("GetSummariesofchiefminister")]
        public async Task<IActionResult> GetSummariesofchiefminister()
        {
            try
            {

                var data = _service.GetResourceSummariesofchiefminister();

                return Ok(UtilService.GetResponse(data));
            }
            catch (Exception ex)
            {
                return Ok(UtilService.GetExResponse<Exception>(ex));
            }
        }
    }
}
