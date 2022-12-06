using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Meeting_App.Models.DTOs.AdminWing
{
    public class AdpSchemeDTO
    {
        public string scheme_ID { get; set; }
        public string scheme_Name { get; set; }
        public string Financial_year { get; set; }
        public string Division_Name { get; set; }
        public string Dist_Name { get; set; }
        public float Estimated_Cost { get; set; }
        public string Scheme_MainType { get; set; }
        public int Scheme_MainType_ID { get; set; }
        public string Scheme_SubType { get; set; }
        public int TYPE_ID { get; set; }
        public string Competent_Forum { get; set; }
    }
}
