using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Meeting_App.Models.DTOs.AdminWing
{
    public class Lora
    {
        public Info Info { get; set; }
    }
    public class Info
    {

        public Info()
        {
            NCDClinic = new List<Proc_GetNCDClinicStaticticsReport_Result2>();
            NCDDesk = new List<Proc_GetNCDDeskStaticticsReport_Result1>();

        }

        public List<Proc_GetNCDClinicStaticticsReport_Result2> NCDClinic { get; set; }
        public List<Proc_GetNCDDeskStaticticsReport_Result1> NCDDesk { get; set; }
    }
    public partial class Proc_GetNCDClinicStaticticsReport_Result2
    {
        public string DistrictName { get; set; }
        public Nullable<int> Asthma { get; set; }
        public Nullable<int> COPD { get; set; }
        public Nullable<int> Diabetes { get; set; }
        public Nullable<int> Hypertension { get; set; }
        public Nullable<int> Malignancies { get; set; }
    }
    public partial class Proc_GetNCDDeskStaticticsReport_Result1
    {
        public Nullable<int> RegisteredPatient { get; set; }
        public string DistrictName { get; set; }
        public Nullable<int> BloodSugar { get; set; }
        public Nullable<int> Hypertension { get; set; }
        public Nullable<int> DerangedPEFR { get; set; }
    }

}

