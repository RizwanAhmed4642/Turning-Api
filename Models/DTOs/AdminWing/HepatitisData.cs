using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Meeting_App.Models.DTOs.AdminWing
{
    public class HepatitisresponseData
    {
        public HepatitisresponseData()
        {
            HepatitisData = new List<HepatitisData>();
        }
        public List< HepatitisData> HepatitisData { get; set; }
    }
    public class HepatitisData
    {
      
        public string district_name { get; set; }
        public long total_registration_count { get; set; }
        public long total_pre_diagnosed_count { get; set; }
        public long total_new_patient_count { get; set; }
        public long total_new_assessment_count { get; set; }
       


    
    }
}
