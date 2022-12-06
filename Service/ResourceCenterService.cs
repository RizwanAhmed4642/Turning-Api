using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Meeting_App.Data.ResourceCenter.RCDbContexts;
using Meeting_App.Data.ResourceCenter.Tables;
using Meeting_App.Models.DTOs;
using Meeting_App.Service;


namespace Meeting_App.Service
{
    public class ResourceCenterService
    {

        
        public List<FlagshipDTO> GetResourceFlagshipSummaries()
        {
            try
            {

                using (var db = new RCDbContexts())
                {
                    var RCSummaries = db.RESOURCES.Where(x => x.RESOURCE_DPT == 75).OrderByDescending(x => x.RESOURCE_CREATED_ON).Select(x=>new FlagshipDTO { 
                        CreatedOn = x.RESOURCE_CREATED_ON,
                        Title = x.RESOURCE_TITLE,
                        Description = x.RESOURCE_DESCRIPTION,
                        FileUrl = "http://116.58.20.67:5544/resources/assets/uploads/flagship-summaries-of-primary-secondary-healthcare-department/" + x.RESOURCE_FILE_PDF
                    }).ToList();




                    return RCSummaries;


                }
            }
            catch
            {
                throw;
            }
        }
        public List<FlagshipDTO> GetResourceSummaries()
        {
            try
            {

                using (var db = new RCDbContexts())
                {
                    var RCSummaries = db.RESOURCES.Where(x => x.RESOURCE_DPT == 70).OrderBy(x => x.RESOURCE_ID).Select(x => new FlagshipDTO
                    {

                        Title = x.RESOURCE_TITLE,
                        Description = x.RESOURCE_DESCRIPTION,
                        FileUrl = "http://116.58.20.67:5544/resources/assets/uploads/summaries/" + x.RESOURCE_FILE_PDF
                    }).ToList();




                    return RCSummaries;


                }
            }
            catch
            {
                throw;
            }
        }



        public List<FlagshipDTO> GetResourceSummariesofchiefminister()
        {
            try
            {

                using (var db = new RCDbContexts())
                {
                    var RCSummaries = db.RESOURCES.Where(x => x.RESOURCE_DPT == 71).OrderBy(x => x.RESOURCE_ID).Select(x => new FlagshipDTO
                    {

                        Title = x.RESOURCE_TITLE,
                        Description = x.RESOURCE_DESCRIPTION,
                        FileUrl = "http://116.58.20.67:5544/resources/assets/uploads/summariesofchiefminister/" + x.RESOURCE_FILE_PDF
                    }).ToList();




                    return RCSummaries;


                }
            }
            catch
            {
                throw;
            }
        }

    }
}
