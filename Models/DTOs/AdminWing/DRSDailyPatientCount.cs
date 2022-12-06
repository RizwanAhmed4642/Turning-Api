using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Meeting_App.Models.DTOs.AdminWing
{
    public class DRSDailyPatientCount
    {
        public int Total_Patient { get; set; }
        public int TodayRegister { get; set; }
        public int TodayVisitScreening { get; set; }
        public int TotalReactive { get; set; }
        public int TotalNONReactive { get; set; }
        public int TodayDeath { get; set; }
        public int TotalDeath { get; set; }


    }
}
