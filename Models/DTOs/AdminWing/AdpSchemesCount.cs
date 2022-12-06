using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Meeting_App.Models.DTOs.AdminWing
{
    //public class AdpSchemesCountresponse
    //{
    //    public AdpSchemesCountresponse()
    //    {
    //        AdpSchemesCount = new AdpSchemesCount();
    //    }
    //    public AdpSchemesCount AdpSchemesCount { get; set; }
    //}
    public class AdpSchemesCount
    {
        public int Ongoing { get; set; }
        public int New { get; set; }
        public int Other { get; set; }
        public int totalCount { get; set; }
    }
}
