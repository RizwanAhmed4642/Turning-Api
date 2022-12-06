using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Meeting_App.Models.DTOs.AdminWing
{
    public class DistrictWiseProcurementDTO
    {
        public string Id { get; set; }
        public string  Name { get; set; }
       public int Total_Entries { get; set; }
        public int In_Process { get; set; }
        public int Not_Awarded { get; set; }
        public int Awarded { get; set; }
    }
}
