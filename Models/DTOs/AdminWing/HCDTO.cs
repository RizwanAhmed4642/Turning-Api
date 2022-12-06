using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Meeting_App.Models.DTOs.AdminWing
{
    public class HCDTO
    {
        public string? Id { get; set; }
        public string? Division { get; set; }
        public string? District { get; set; }
        public string? Tehsil { get; set; }
        public string? HFName { get; set; }
        public float? OpeningBalance { get; set; }
        public float? CurrentBalance { get; set; }
        public float? Recived { get; set; }
        public float? Expenditures { get; set; }
        public string? HFCode { get; set; }
        public string HFTypeName { get; set; }
    }
}
