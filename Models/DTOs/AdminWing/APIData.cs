using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Meeting_App.Models.DTOs.AdminWing
{
  
    
    public class apidata
    {
        public List<APIData> APIData { get; set; }
    }
    public class APIData
    {
        public string DistrictName { get; set; }
        public string InspectionAwaited { get; set; }
        public string InspectionInProcess { get; set; }
        public string FinalSucrutiny { get; set; }
        public string Dispatched { get; set; }
        public string Printed { get; set; }
        public string Invalid { get; set; }
        public string Rejected { get; set; }
    }
}
