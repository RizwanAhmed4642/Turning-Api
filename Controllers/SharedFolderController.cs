using AutoMapper;
using Meeting_App.Models.DTOs;
using Meeting_App.Service;
using Meeting_App.Service.Common;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Meeting_App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SharedFolderController : ControllerBase
    {
        private FolderService _folderService;

        #region Constructor
        public SharedFolderController(FolderService folderService, IMapper mapper)
        {
            _folderService = folderService;
    
        }
        #endregion

        [HttpPost]
        [Route("AddFolder")]
        public ActionResult Save(FolderDTO model)
        {
            try
            {

                model.UserId = this.GetUserId();

                _folderService.Save(model);

                
                return Ok(UtilService.GetResponse<Json>(null, ""));


            }
            catch (Exception ex)
            {
                return BadRequest(UtilService.GetExResponse<Exception>(ex));
            }
        }

        [HttpGet]
        [Route("GetAllFolders")]
        public  IActionResult GetAllFolders()
        {
            try
            {
                Guid userid = Guid.Parse(this.GetUserId());

                List<FolderDTO> FolderList = _folderService.FetchFoldersAll();

                return Ok(UtilService.GetResponse(FolderList));
            }
            catch (Exception ex)
            {
                return Ok(UtilService.GetExResponse<Exception>(ex));
            }
        }


        [HttpGet]
        [Route("GetChildFolders")]
        public IActionResult GetChildFolders(int? id)
        {
            try
            {
                Guid userid = Guid.Parse(this.GetUserId());

                List<FolderDTO> FolderList = _folderService.FetchFolders(id, "");

                return Ok(UtilService.GetResponse(FolderList));
            }
            catch (Exception ex)
            {
                return Ok(UtilService.GetExResponse<Exception>(ex));
            }
        }

        #region AddFile
        [DisableRequestSizeLimit]
        [HttpPost]
        [Route("AddFile")]
        public async Task<IActionResult> AddFile([FromForm] FileDTO model)
        {
            try
            {
                int res = 0;

                string userid = this.GetUserId();

                res = await _folderService.AddFile(model, userid);

                string msg = "";

                if (res == 1)
                {
                    msg = "Saved Successfully";
                }
                else
                {
                    msg = "Folder Id Should not be 0 or empty";
                }

                return Ok(UtilService.GetResponse<Json>(null, msg));


            }
            catch (Exception ex)
            {
                return BadRequest(UtilService.GetExResponse<Exception>(ex));
            }
        }
        #endregion


        [HttpGet]
        [Route("GetFiles")]
        public IActionResult GetFiles(int id)
        {
            try
            {
                Guid userid = Guid.Parse(this.GetUserId());

                List<FileDTO> FilesList = _folderService.FetchFiles(id, "", userid);

                return Ok(UtilService.GetResponse(FilesList));
            }
            catch (Exception ex)
            {
                return Ok(UtilService.GetExResponse<Exception>(ex));
            }
        }


        #region Deletefolder
        //[Authorize]
        [HttpGet]
        [Route("Deletefolder")]
        public IActionResult Deletefolder(int Id)
        {
            try
            {

                _folderService.Deletefolder(Id);

                return Ok(UtilService.GetResponse("Folder Deleted"));
            }
            catch (Exception ex)
            {
                return Ok(UtilService.GetExResponse<Exception>(ex));
            }
        }
        #endregion

        #region DeleteFile
        //[Authorize]
        [HttpGet]
        [Route("DeleteFile")]
        public IActionResult DeleteFile(int Id)
        {
            try
            {

                _folderService.DeleteFile(Id);

                return Ok(UtilService.GetResponse("File Deleted"));
            }
            catch (Exception ex)
            {
                return Ok(UtilService.GetExResponse<Exception>(ex));
            }
        }
        #endregion

        
    }
}
