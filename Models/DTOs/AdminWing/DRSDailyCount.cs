using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Meeting_App.Models.DTOs.AdminWing
{
    public class DRSDailyCount
    {
       
        public int Total_HIV_PCR { get; set; }
        public int Today_HIV_PCR_Detacted { get; set; }
        public int Today_HIV_PCR_Not_Detacted { get; set; }
        public int Total_SYPHILIS_ANTIBODIES_ELISA { get; set; }
        public int Today_SYPHILIS_ANTIBODIES_ELISA_Detacted { get; set; }
        public int Today_SYPHILIS_ANTIBODIES_ELISA_Not_Detacted { get; set; }
        public int Total_HBV_PCR { get; set; }
        public int Today_HBV_PCR_Detected { get; set; }
        public int Today_HBV_PCR_Not_Detected { get; set; }
        public int Total_HCV_PCR { get; set; }
        public int Today_HCV_PCR_Detected { get; set; }
        public int Today_HCV_PCR_Not_Detected { get; set; }
        public int Total_CD4_Test { get; set; }
        public int Today_CD4_Test { get; set; }

    }
}
